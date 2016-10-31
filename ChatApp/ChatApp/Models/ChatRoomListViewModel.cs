using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebCrawler.HubModel.ServerModels;

namespace WebCrawler.ChatApp.Models
{
    public class ChatRoomListViewModel
    {
        public Dictionary<int, string> ChatRooms { get; set; }
        public Dictionary<string, ProcessSession> Sessions { get; set; }
    }
}