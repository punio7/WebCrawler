using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebCrawler.HubModel.Arguments;
using WebCrawler.WebApp.DbModel;
using WebCrawler.WebApp.DbModel.Enums;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.Logic
{
    public class WorkerManager
    {
        private readonly WebCrawlerDbContext dbContext;
        private readonly SessionManager sessionManager;

        public WorkerManager(WebCrawlerDbContext dbContext, SessionManager sessionManager)
        {
            this.dbContext = dbContext;
            this.sessionManager = sessionManager;
        }

        public void RegisterNewWorker(string connectionId, RegisterWorkerArguments args)
        {
            WorkerConnection workerDb =
                (from worker in dbContext.WorkerConnections
                 where worker.WorkerName == args.WorkerName
                 select worker).FirstOrDefault();

            if (workerDb == null)
            {
                WorkerConnection worker = new WorkerConnection()
                {
                    ConnectionId = connectionId,
                    WorkerName = args.WorkerName,
                    MaxSessions = args.MaxSessions,
                    ActiveSessions = args.ActiveSessionCount,
                    AllSessions = args.AllSessionCount,
                    State = WorkerConnectionState.Connected,
                };
                dbContext.WorkerConnections.Add(worker);
            }
            else
            {
                workerDb.ConnectionId = connectionId;
                workerDb.MaxSessions = args.MaxSessions;
                workerDb.AllSessions = args.ActiveSessionCount;
                workerDb.ActiveSessions = args.AllSessionCount;
                workerDb.State = WorkerConnectionState.Connected;

                sessionManager.UpdateWorkerConnectionCache(workerDb);
                dbContext.Entry(workerDb).State = EntityState.Modified;
            }
            dbContext.SaveChanges();
        }

        public void UnregisterAllWorkers()
        {
            var workerList = from worker in dbContext.WorkerConnections select worker;
            foreach (var worker in workerList)
            {
                worker.State = WorkerConnectionState.Disconnected;
                dbContext.Entry(worker).State = EntityState.Modified;
            }
            dbContext.SaveChanges();
        }

        public WorkerConnection GetAvaliableWorker(string appName)
        {
            return (from worker in dbContext.WorkerConnections
                    where worker.State == WorkerConnectionState.Connected
                    select worker).FirstOrDefault();
        }

        public void UpdateWorker(WorkerConnection worker)
        {
            dbContext.Entry(worker).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void WorkerDisconnection(string connectionId)
        {
            var workerDb = (from worker in dbContext.WorkerConnections
                            where worker.ConnectionId == connectionId
                            select worker).FirstOrDefault();
            if (workerDb != null)
            {
                workerDb.State = WorkerConnectionState.Disconnected;
                dbContext.Entry(workerDb).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }
    }
}