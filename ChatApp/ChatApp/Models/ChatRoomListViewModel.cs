﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebCrawler.HubModel.ServerModels;

namespace WebCrawler.ChatApp.Models
{
    public class ChatRoomListViewModel
    {
        public IEnumerable<ProcessSession> Sessions { get; set; }
    }
}