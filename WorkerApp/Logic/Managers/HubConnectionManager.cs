using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using WebCrawler.HubModel.Arguments;
using WebCrawler.WorkerApp.Common.Model;

namespace WebCrawler.WorkerApp.Logic.Managers
{
    public class HubConnectionManager : IDisposable
    {
        private ProcessManager ProcessManager { get; set; }
        private HubConnection connection;

        public HubConnectionManager(ProcessManager processManager)
        {
            ProcessManager = processManager;
        }

        public async Task<HubConnection> CreateHubConnection(string hubUrl)
        {
            if (connection == null)
            {

                connection = new HubConnectionBuilder()
                    .WithUrl(hubUrl + "/appsHub")
                    .Build();
                connection.Closed += Connection_Closed;

                await connection.StartAsync();
                RegisterHubMethods();
                RegisterWorker();
            }
            return connection;
        }

        private Task Connection_Closed(Exception exception)
        {
            connection = null;
            return Task.CompletedTask;
        }

        public async Task Disconnect()
        {
            if (connection != null)
            {
                await connection.StopAsync();
                connection = null;
            }
        }

        private void RegisterHubMethods()
        {
            connection.On<StartProcessArguments>("StartProcess", args => StartProcess(args));
            connection.On<ExecuteProcessCommandArguments>("ExecuteProcessCommand", args => ExecuteProcessCommand(args));
            connection.On<GetProcessOutputArguments>("GetProcessOutput", args => GetProcessOutput(args));
        }

        #region Server to worker

        private void StartProcess(StartProcessArguments args)
        {
            var process = ProcessManager.CreateNewProcess(args);
            process.OutputRead += WorkerSendOutput;
            process.OnExit += WorkerProcessExit;
            process.StartReadingOutput();
        }

        private void ExecuteProcessCommand(ExecuteProcessCommandArguments args)
        {
            ProcessManager.ExecuteCommand(args.SessionId, args.Command);
        }

        private void GetProcessOutput(GetProcessOutputArguments args)
        {
            string processOutput = ProcessManager.GetProcessOutput(args.SessionId);
            if (CheckConnection())
            {
                WorkerSendOutputArguments argsSend = new WorkerSendOutputArguments()
                {
                    SessionId = args.SessionId,
                    Text = processOutput,
                    ConnectionId = args.ConnectionId,
                };
                connection.InvokeAsync("WorkerSendOutput", argsSend);
            }
        }

        #endregion

        #region Worker to server

        private void RegisterWorker()
        {
            if (CheckConnection())
            {
                RegisterWorkerArguments args = new RegisterWorkerArguments()
                {
                    ActiveSessionCount = ProcessManager.ProcessInstances.Count,
                    AllSessionCount = ProcessManager.ProcessInstances.Count,
                    MaxSessions = 20,
                    WorkerName = "WebCrawlerWorker1",
                    SessionIdList = ProcessManager.ProcessInstances.Keys,
                };
                connection.InvokeAsync("RegisterWorker", args); 
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
                connection.InvokeAsync("WorkerSendOutput", args).Wait();
            }
        } 

        private void WorkerProcessExit(ProcessInstance sender)
        {
            if (CheckConnection())
            {
                WorkerEndProcessArguments args = new WorkerEndProcessArguments()
                {
                    SessionId = sender.SessionId,
                };
                connection.InvokeAsync("WorkerEndProcess", args);
            }
        }

        #endregion

        private bool CheckConnection()
        {
            return connection != null && connection.State == HubConnectionState.Connected;
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.StopAsync();
            }
        }
    }
}
