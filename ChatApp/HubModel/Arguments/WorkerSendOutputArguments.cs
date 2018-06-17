using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.HubModel.Arguments
{
    public class WorkerSendOutputArguments
    {
        public long SessionId { get; set; }
        public string Text { get; set; }
        public string ConnectionId { get; set; }

        public override string ToString()
        {
            return $"SessionId: {SessionId} Text.Length: {Text.Length}";
        }
    }
}
