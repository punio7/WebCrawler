using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebCrawler.HubModel.ServerModels;

namespace WebCrawler.ChatApp.Models
{
    public class AppViewModel
    {
        public string AppName { get; set; }
        public long SessionId { get; set; }
        public bool IsOwner { get; set; }
    }
}