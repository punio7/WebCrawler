﻿using WebCrawler.WebApp.DbModel.Enums;

namespace WebCrawler.WebApp.DbModel.Models
{
    public class ProcessSession
    {
        public long Id { get; set; }

        public static readonly string sessionGroupPrefix = "sessionGroup_";

        public long? WorkerConnectionId { get; set; }

        public WorkerConnection WorkerConnection { get; set; }

        public SessionState State { get; set; }

        public string GroupName
        {
            get
            {
                return $"{sessionGroupPrefix}{Id}";
            }
        }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public string AppName { get; set; }

        public bool CanSendCommands(string userId)
        {
            return userId == CreatorId && State == SessionState.Active;
        }

        public bool CanSendOutput(string userId)
        {
            return userId == WorkerConnection.ConnectionId;
        }
    }
}
