using System.ComponentModel.DataAnnotations;
using WebCrawler.WebApp.DbModel.Enums;

namespace WebCrawler.WebApp.DbModel.Models
{
    public class WorkerConnection
    {
        [Key]
        public long Id { get; set; }

        //[Index]
        //[Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string WorkerName { get; set; }

        public string ConnectionId { get; set; }

        public int ActiveSessions { get; set; }

        public int MaxSessions { get; set; }

        public int AllSessions { get; set; }

        public WorkerConnectionState State { get; set; }
    }
}
