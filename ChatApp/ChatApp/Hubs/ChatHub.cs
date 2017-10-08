using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using WebCrawler.ChatApp.Logic;
using WebCrawler.HubModel.Arguments;
using WebCrawler.HubModel.Enums;
using WebCrawler.HubModel.ServerModels;

namespace WebCrawler.ChatApp.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        protected ILog logger = log4net.LogManager.GetLogger(typeof(ChatHub));

        public async Task RegisterWorker(RegisterWorkerArguments args)
        {
            await HubOperation(nameof(RegisterWorker), args, async () =>
            {
                WorkerManager.Instance.RegisterNewWorker(Context.ConnectionId, args);
            });
        }

        [Authorize]
        public async Task ClientJoinSession(long sessionId)
        {
            await HubOperation(nameof(ClientJoinSession), sessionId, async () =>
            {
                ProcessSession session = SessionManager.Instance.GetSession(sessionId);
                var user = Context.User.Identity;
                await Groups.Add(Context.ConnectionId, session.GroupName);

                Clients.Group(session.GroupName).AddSystemMessage($"Użytkownik {user.Name} dołączył do sesji.");

                if (session.State == SessionState.NotStarted)
                {
                    Clients.Group(session.GroupName).AddSystemMessage("Rozpoczynanie procesu zdalnego.");

                    WorkerConnection worker = WorkerManager.Instance.GetAvaliableWorker(session.AppName);
                    StartProcessArguments args = new StartProcessArguments()
                    {
                        UserName = Context.User.Identity.Name,
                        SessionId = session.Id,
                        ApplicationName = session.AppName,
                        UserId = Context.User.Identity.GetUserId(),
                    };
                    await Clients.Client(worker.ConnectionId).StartProcess(args);
                    session.State = SessionState.Active;
                    session.WorkerConnection = worker;
                    session.WorkerConnectionId = worker.Id;
                    SessionManager.Instance.Update(session);
                }
            });
        }

        [Authorize]
        public async Task ClientSendCommand(ExecuteProcessCommandArguments args)
        {
            await HubOperation(nameof(ClientSendCommand), args, async () =>
            {
                ProcessSession session = SessionManager.Instance.GetSession(args.SessionId);
                var user = Context.User.Identity;

                if (session.CanSendCommands(user.GetUserId()))
                {
                    await Clients.Client(session.WorkerConnection.ConnectionId).ExecuteProcessCommand(args);
                }
            });
        }

        public async Task WorkerSendOutput(WorkerSendOutputArguments args)
        {
            await HubOperation(nameof(WorkerSendOutput), args, async () =>
            {
                ProcessSession session = SessionManager.Instance.GetSession(args.SessionId);

                await Clients.Group(session.GroupName).AddMessage(args.Text);
            });
        }

        public async Task WorkerEndSession(object args)
        {
            await HubOperation(nameof(WorkerEndSession), args, async () =>
            {

            });
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return HubOperation(nameof(OnDisconnected), stopCalled, () =>
            {
                WorkerManager.Instance.WorkerDisconnection(Context.ConnectionId);
                return base.OnDisconnected(stopCalled);
            });
        }

        #region Utils

        protected async Task HubOperation<T>(string method, T arguments, Func<Task> action)
        {
            string log;
            var user = Context.User.Identity;
            string argumentsString = arguments != null ? arguments.ToString() : "";
            if (user != null)
            {
                log = $"ChatHub: User: {user.Name} Method: {method} Args: {argumentsString}";
            }
            else
            {
                log = $"ChatHub: Method: {method} Args: {argumentsString}";
            }

            logger.Info(log);
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                logger.Error("Błąd w SignalRHub", ex);
                throw;
            }
        }

        #endregion
    }
}