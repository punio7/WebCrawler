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

        public long? WorkerConnectionId { get; set; }

        [ForeignKey(nameof(WorkerConnectionId))]
        public WorkerConnection WorkerConnection { get; set; }

        public SessionState State { get; set; }

        [NotMapped]
        public string GroupName
        {
            get
            {
                return $"{sessionGroupPrefix}{Id}";
            }
        }

        public string CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public virtual ApplicationUser CreatorUser { get; set; }

        public string AppName { get; set; }

        public bool CanSendCommands(string userId)
        {
            return userId == CreatorId;
        }
    }
}
