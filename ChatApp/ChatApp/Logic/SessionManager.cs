using System;
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

        private Dictionary<long, ProcessSession> sessionCache;

        private SessionManager()
        {
            sessionCache = new Dictionary<long, ProcessSession>();
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
            lock (sessionCache)
            {
                sessionCache[session.Id] = session;
            }
            return session;
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
                        (from session in dbContext.ProcessSessions
                        where session.Id == id
                        select session).SingleOrDefault();
                lock (sessionCache)
                {
                    sessionCache[id] = processSession;
                }
                return processSession;
            }
        }

        public void Update(ProcessSession session)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                dbContext.Entry(session).State = EntityState.Modified;
                dbContext.SaveChanges();
                lock (sessionCache)
                {
                    sessionCache[session.Id] = session;
                }
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
            lock (sessionCache)
            {
                foreach (ProcessSession session in sessionCache.Values.Where(s => s.WorkerConnectionId == connection.Id))
                {
                    session.WorkerConnection = connection;
                } 
            }
        }
    }
}