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
    public class ProcessSession
    {
        [Key]
        public long Id { get; set; }

        public static readonly string sessionGroupPrefix = "sessionGroup_";

        [ForeignKey(nameof(WorkerConnection))]
        [Index]
        public long? WorkerConnectionId { get; set; }

        public virtual WorkerConnection WorkerConnection { get; set; }

        [Index]
        public SessionState State { get; set; }

        [NotMapped]
        public string GroupName
        {
            get
            {
                return $"{sessionGroupPrefix}{Id}";
            }
        }

        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public string AppName { get; set; }

        public bool CanSendCommands(string userId)
        {
            return userId == CreatorId;
        }

        public bool CanSendOutput(string userId)
        {
            return userId == WorkerConnection.ConnectionId;
        }
    }
}
