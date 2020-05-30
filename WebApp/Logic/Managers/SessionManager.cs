using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using WebCrawler.WebApp.DbModel;
using WebCrawler.WebApp.DbModel.Enums;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.Logic
{
    public class SessionManager
    {
        private static Lazy<ConcurrentDictionary<long, ProcessSession>> sessionCache;
        private readonly WebCrawlerDbContext dbContext;

        static SessionManager()
        {
            sessionCache = new Lazy<ConcurrentDictionary<long, ProcessSession>>(LazyThreadSafetyMode.PublicationOnly);
        }

        public SessionManager(WebCrawlerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ProcessSession StartNewSession(string userId, long appId)
        {
            ProcessSession session = new ProcessSession()
            {
                State = SessionState.NotStarted,
                CreatorId = userId,
                AppId = appId,
            };
            dbContext.ProcessSessions.Add(session);
            dbContext.SaveChanges();
            sessionCache.Value[session.Id] = session;
            return session;
        }

        public void CheckActiveSessions(string workerName, IEnumerable<long> sessionIdList, out IEnumerable<ProcessSession> terminatedSessions)
        {
            List<ProcessSession> terminatedSessionsList = new List<ProcessSession>();
            var worker = dbContext.WorkerConnections.First(w => w.WorkerName == workerName);
            var sessions = dbContext.ProcessSessions.Where(s => s.WorkerConnectionId == worker.Id);
            foreach (ProcessSession session in sessions)
            {
                if (!sessionIdList.Contains(session.Id))
                {
                    session.State = SessionState.Finished;
                    terminatedSessionsList.Add(session);
                    if (sessionCache.Value.ContainsKey(session.Id))
                    {
                        sessionCache.Value[session.Id] = session;
                    }
                }
            }
            dbContext.SaveChanges();
            terminatedSessions = terminatedSessionsList;
        }

        public ProcessSession GetSession(long id)
        {
            if (sessionCache.Value.ContainsKey(id))
            {
                return sessionCache.Value[id];
            }
            ProcessSession processSession =
                    (from session in dbContext.ProcessSessions.Include(s => s.WorkerConnection)
                     where session.Id == id
                     select session).SingleOrDefault();
            if (processSession != null)
            {
                sessionCache.Value[id] = processSession;
            }
            return processSession;
        }

        public IEnumerable<ProcessSession> GetActiveSessions()
        {
            return dbContext.ProcessSessions
                .Where(s => s.State == SessionState.Active);
        }

        public void Update(ProcessSession session)
        {
            dbContext.Entry(session).State = EntityState.Modified;
            dbContext.SaveChanges();
            sessionCache.Value[session.Id] = session;
        }

        public void UnregisterWorker(WorkerConnection worker)
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

        public void UpdateWorkerConnectionCache(WorkerConnection connection)
        {
            foreach (ProcessSession session in sessionCache.Value.Values.Where(s => s.WorkerConnectionId == connection.Id))
            {
                session.WorkerConnection = connection;
            }
        }
    }
}