using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using ChatApp.HubModel;
using WebCrawler.HubModel.Enums;
using WebCrawler.HubModel.ServerModels;

namespace WebCrawler.ChatApp.Logic
{
    public class SessionManager
    {
        public static SessionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SessionManager();
                }
                return _instance;
            }
        }
        private static SessionManager _instance;

        private ConcurrentDictionary<long, ProcessSession> sessionCache;

        private SessionManager()
        {
            sessionCache = new ConcurrentDictionary<long, ProcessSession>();
        }

        public ProcessSession StartNewSession(string userId, string appName)
        {
            ProcessSession session = new ProcessSession()
            {
                State = SessionState.NotStarted,
                CreatorId = userId,
                AppName = appName,
            };
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                dbContext.ProcessSessions.Add(session);
                dbContext.SaveChanges();
            }
            sessionCache[session.Id] = session;
            return session;
        }

        internal void CheckActiveSessions(string workerName, IEnumerable<long> sessionIdList, out IEnumerable<ProcessSession> terminatedSessions)
        {
            List<ProcessSession> terminatedSessionsList = new List<ProcessSession>();
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                var worker = dbContext.WorkerConnections.First(w => w.WorkerName == workerName);
                var sessions = dbContext.ProcessSessions.Where(s => s.WorkerConnectionId == worker.Id);
                foreach (ProcessSession session in sessions)
                {
                    if (!sessionIdList.Contains(session.Id))
                    {
                        session.State = SessionState.Finished;
                        terminatedSessionsList.Add(session);
                        if (sessionCache.ContainsKey(session.Id))
                        {
                            sessionCache[session.Id] = session;
                        }
                    }
                }
                dbContext.SaveChanges();
            }
            terminatedSessions = terminatedSessionsList;
        }

        public ProcessSession GetSession(long id)
        {
            if (sessionCache.ContainsKey(id))
            {
                return sessionCache[id];
            }
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                ProcessSession processSession =
                        (from session in dbContext.ProcessSessions.Include(s => s.WorkerConnection)
                         where session.Id == id
                         select session).SingleOrDefault();
                if (processSession != null)
                {
                    sessionCache[id] = processSession; 
                }
                return processSession;
            }
        }

        public IEnumerable<ProcessSession> GetActiveSessions()
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                var processSessionList =
                        (from session in dbContext.ProcessSessions.Include(ps => ps.Creator)
                         where session.State == SessionState.Active
                         select session);
                return processSessionList.ToList();
            }
        }

        public void Update(ProcessSession session)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                dbContext.Entry(session).State = EntityState.Modified;
                dbContext.SaveChanges();
                sessionCache[session.Id] = session;
            }
        }

        public void UnregisterWorker(WorkerConnection worker)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                var sessionList = (from session in dbContext.ProcessSessions
                                   where session.WorkerConnectionId == worker.Id
                                   select session).ToList();
                foreach (var session in sessionList)
                {
                    session.WorkerConnection = null;
                    session.WorkerConnectionId = null;
                    dbContext.Entry(session).State = EntityState.Modified;
                }
                dbContext.SaveChanges();
            }
        }

        public void UpdateWorkerConnectionCache(WorkerConnection connection)
        {
            foreach (ProcessSession session in sessionCache.Values.Where(s => s.WorkerConnectionId == connection.Id))
            {
                session.WorkerConnection = connection;
            }
        }
    }
}