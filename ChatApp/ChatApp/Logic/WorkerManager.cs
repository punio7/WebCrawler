using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using ChatApp.HubModel;
using WebCrawler.HubModel.Arguments;
using WebCrawler.HubModel.Enums;
using WebCrawler.HubModel.ServerModels;

namespace WebCrawler.ChatApp.Logic
{
    public class WorkerManager
    {
        private static WorkerManager _instance;

        public static WorkerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WorkerManager();
                }
                return _instance;
            }
        }

        private WorkerManager()
        {
        }

        public void RegisterNewWorker(string connectionId, RegisterWorkerArguments args)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
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

                    SessionManager.Instance.UpdateWorkerConnectionCache(workerDb);
                    dbContext.Entry(workerDb).State = EntityState.Modified;
                }
                dbContext.SaveChanges();
            }
        }

        public void UnregisterAllWorkers()
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                var workerList = from worker in dbContext.WorkerConnections select worker;
                foreach (var worker in workerList)
                {
                    worker.State = WorkerConnectionState.Disconnected;
                    dbContext.Entry(worker).State = EntityState.Modified;
                }
                dbContext.SaveChanges();
            }
        }

        public WorkerConnection GetAvaliableWorker(string appName)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                return (from worker in dbContext.WorkerConnections
                        where worker.State == WorkerConnectionState.Connected
                        select worker).FirstOrDefault();
            }
        }

        public void UpdateWorker(WorkerConnection worker)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                dbContext.Entry(worker).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
        }

        public void WorkerDisconnection(string connectionId)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
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
}