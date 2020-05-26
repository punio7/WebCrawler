using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.HubModel.Arguments
{
    public class ClientSendOutputArguments
    {
        public long SessionId { get; set; }
        public string Output { get; set; }

        public override string ToString()
        {
            return $"SessionId: {SessionId}, Output length: {Output.Length}";
        }
    }
}
