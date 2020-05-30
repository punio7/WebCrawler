using System.Collections.Generic;
using WebCrawler.WebApp.DbModel.Enums;

namespace WebCrawler.WebApp.DbModel.Models
{
    public class WorkerConnection
    {
        public long Id { get; set; }

        public string WorkerName { get; set; }

        public string ConnectionId { get; set; }

        public int ActiveSessions { get; set; }

        public int MaxSessions { get; set; }

        public int AllSessions { get; set; }

        public WorkerConnectionState State { get; set; }

        public List<ProcessSession> ProcesSessions { get; set; }
    }
}
