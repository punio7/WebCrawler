using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.HubModel.Arguments
{
    public class StartProcessArguments
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public long SessionId { get; set; }
        public string ApplicationName { get; set; }
    }
}
