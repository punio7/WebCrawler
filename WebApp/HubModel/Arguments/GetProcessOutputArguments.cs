using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.HubModel.Arguments
{
    public class GetProcessOutputArguments
    {
        public long SessionId { get; set; }
        public string ConnectionId { get; set; }

        public override string ToString()
        {
            return $"SessionId: {SessionId}, ConnectionId: {ConnectionId}";
        }
    }
}
