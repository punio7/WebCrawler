using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebCrawler.HubModel.ServerModels;

namespace WebCrawler.ChatApp.Models
{
    public class ChatRoomViewModel
    {
        public string ChatRoomName { get; set; }
        public int ChatRoomId { get; set; }
        public ApplicationUser User { get; set; }
        public long SessionId { get; set; }
    }
}