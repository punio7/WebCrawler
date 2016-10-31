using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using WebCrawler.HubModel.Arguments;
using WebCrawler.WorkerApp.Common.Model;

namespace WebCrawler.WorkerApp.Logic.Managers
{
    public class HubConnectionManager : IDisposable
    {
        private ProcessManager ProcessManager { get; set; }
        private IHubProxy hub;
        private HubConnection connection;

        public HubConnectionManager(ProcessManager processManager)
        {
            ProcessManager = processManager;
        }

        public HubConnection CreateHubConnection(string hubUrl)
        {
            if (connection == null)
            {
                connection = new HubConnection(hubUrl);
                hub = connection.CreateHubProxy("ChatHub");
                connection.Closed += Connection_Closed;
                connection.Start().Wait();
                RegisterHubMethods();
                RegisterWorker();
            }
            return connection;
        }

        private void Connection_Closed()
        {
            connection = null;
        }

        public void Disconnect()
        {
            if (connection != null)
            {
                connection.Stop();
                connection = null;
            }
        }

        private void RegisterHubMethods()
        {
            hub.On<StartProcessArguments>("StartProcess", args => StartProcess(args));
            hub.On<ExecuteProcessCommandArguments>("ExecuteProcessCommand", args => ExecuteProcessCommand(args));
        }

        #region Server to worker

        private void StartProcess(StartProcessArguments args)
        {
            var process = ProcessManager.CreateNewProcess(args);
            process.OutputRead += WorkerSendOutput;
            process.StartReadingOutput();
        }

        private void ExecuteProcessCommand(ExecuteProcessCommandArguments args)
        {
            ProcessManager.ExecuteCommand(args.SessionId, args.Command);
        }

        #endregion

        #region Worker to server

        private void RegisterWorker()
        {
            if (CheckConnection())
            {
                RegisterWorkerArguments args = new RegisterWorkerArguments()
                {
                    ActiveSessions = ProcessManager.ProcessInstances.Count,
                    AllSessions = ProcessManager.ProcessInstances.Count,
                    MaxSessions = 20,
                    WorkerName = "WebCrawler Worker 1",
                };
                hub.Invoke("RegisterWorker", args); 
            }
        }

        private void WorkerSendOutput(ProcessInstance sender, string text)
        {
            if (CheckConnection())
            {
                WorkerSendOutputArguments args = new WorkerSendOutputArguments()
                {
                    SessionId = sender.SessionId,
                    Text = text,
                };
                hub.Invoke("WorkerSendOutput", args).Wait();
            }
        } 

        #endregion

        private bool CheckConnection()
        {
            return connection != null && connection.State == ConnectionState.Connected;
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Stop();
            }
        }
    }
}
