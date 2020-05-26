using System.Collections.Generic;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.WebApp.Models.Apps
{
    public class ListSessionsViewModel
    {
        public IEnumerable<ProcessSession> Sessions { get; set; }
    }
}
