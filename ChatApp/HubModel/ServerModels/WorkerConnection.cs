using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.HubModel.Enums;

namespace WebCrawler.HubModel.ServerModels
{
    public class WorkerConnection
    {
        [Key]
        public long Id { get; set; }

        [Index]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string WorkerName { get; set; }

        public string ConnectionId { get; set; }

        public int ActiveSessions { get; set; }

        public int MaxSessions { get; set; }

        public int AllSessions { get; set; }

        public WorkerConnectionState State { get; set; }
    }
}
