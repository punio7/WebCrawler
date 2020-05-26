using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.HubModel.Arguments
{
    public class RegisterWorkerArguments
    {
        public string WorkerName { get; set; }
        public int ActiveSessionCount { get; set; }
        public int MaxSessions { get; set; }
        public int AllSessionCount { get; set; }
        public IEnumerable<long> SessionIdList { get; set; }
        public override string ToString()
        {
            return $"WorkerName: {WorkerName} ActiveSessions: {ActiveSessionCount} MaxSessions: {MaxSessions} AllSessions: {AllSessionCount}";
        }
    }
}
