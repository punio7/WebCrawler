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
        public int ActiveSessions { get; set; }
        public int MaxSessions { get; set; }
        public int AllSessions { get; set; }
        public override string ToString()
        {
            return $"WorkerName: {WorkerName} ActiveSessions: {ActiveSessions} MaxSessions: {MaxSessions} AllSessions: {AllSessions}";
        }
    }
}
