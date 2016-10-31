using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.HubModel.Arguments
{
    public class ExecuteProcessCommandArguments
    {
        public long SessionId { get; set; }
        public string Command { get; set; }

        public override string ToString()
        {
            return $"SessionId: {SessionId}, Command: {Command}";
        }
    }
}
