using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebCrawler.HubModel.Arguments;
using WebCrawler.WebApp.DbModel.Enums;
using WebCrawler.WebApp.DbModel.Extensions;
using WebCrawler.WebApp.DbModel.Models;
using WebCrawler.WebApp.Logic;

namespace WebApp.Hubs
{
    public class AppsHub : Hub
    {
        protected ILog logger = log4net.LogManager.GetLogger(typeof(AppsHub));
        private readonly UserManager userManager;
        private readonly WorkerManager workerManager;
        private readonly SessionManager sessionManager;

        public AppsHub(UserManager userManager, WorkerManager workerManager, SessionManager sessionManager)
        {
            this.userManager = userManager;
            this.workerManager = workerManager;
            this.sessionManager = sessionManager;
        }

        public async Task RegisterWorker(RegisterWorkerArguments args)
        {
            await HubOperation(nameof(RegisterWorker), args, async () =>
            {
                workerManager.RegisterNewWorker(Context.ConnectionId, args);
                sessionManager.CheckActiveSessions(args.WorkerName, args.SessionIdList, out IEnumerable<ProcessSession> terminatedSessions);
                foreach (var session in terminatedSessions)
                {
                    await Clients.Group(session.GroupName).SendAsync("AddSystemMessage", "Sesja zakończona przez serwer wykonawczy.");
                }
            });
        }

        [Authorize]
        public async Task ClientJoinSession(long sessionId)
        {
            await HubOperation(nameof(ClientJoinSession), sessionId, async () =>
            {
                ProcessSession session = sessionManager.GetSession(sessionId);
                var user = userManager.GetUserById(Context.User.GetUserId());
                await Groups.AddToGroupAsync(Context.ConnectionId, session.GroupName);

                _ = Clients.Group(session.GroupName).SendAsync("AddSystemMessage", $"Użytkownik {user.DisplayName} dołączył do sesji.");

                if (session.State == SessionState.NotStarted)
                {
                    _ = Clients.Group(session.GroupName).SendAsync("AddSystemMessage", "Rozpoczynanie procesu zdalnego.");

                    WorkerConnection worker = workerManager.GetAvaliableWorker(session.AppName);
                    StartProcessArguments args = new StartProcessArguments()
                    {
                        UserName = user.Email,
                        SessionId = session.Id,
                        ApplicationName = session.AppName,
                        UserId = user.Id,
                    };
                    dynamic workerSignalR = GetWorkerContext(worker);
                    await workerSignalR.StartProcess(args);
                    session.State = SessionState.Active;
                    session.WorkerConnection = worker;
                    session.WorkerConnectionId = worker.Id;
                    sessionManager.Update(session);
                }
                else
                {
                    await Clients.Client(session.WorkerConnection.ConnectionId).SendAsync("GetProcessOutput", new GetProcessOutputArguments()
                    {
                        SessionId = sessionId,
                        ConnectionId = Context.ConnectionId,
                    });
                }
            });
        }

        [Authorize]
        public async Task ClientSendCommand(ExecuteProcessCommandArguments args)
        {
            await HubOperation(nameof(ClientSendCommand), args, async () =>
            {
                ProcessSession session = sessionManager.GetSession(args.SessionId);
                var user = Context.User;

                if (session.CanSendCommands(user.GetUserId()))
                {
                    await Clients.GroupExcept(session.GroupName, Context.ConnectionId).SendAsync("AddMessage", args.Command);
                    var workerContext = GetWorkerContext(session.WorkerConnection);
                    await workerContext.ExecuteProcessCommand(args);
                }
            });
        }

        public async Task ClientSendOutput(ClientSendOutputArguments args)
        {
            await HubOperation(nameof(ClientSendCommand), args, async () =>
            {
                ProcessSession session = sessionManager.GetSession(args.SessionId);
                var user = Context.User;

                if (session.CanSendCommands(user.GetUserId()))
                {
                    await Clients.GroupExcept(session.GroupName, Context.ConnectionId).SendAsync("AddMessage", args.Output);
                }
            });
        }

        public async Task WorkerSendOutput(WorkerSendOutputArguments args)
        {
            await HubOperation(nameof(WorkerSendOutput), args, async () =>
            {
                ProcessSession session = sessionManager.GetSession(args.SessionId);

                if (session.CanSendOutput(Context.ConnectionId))
                {
                    if (args.ConnectionId == null)
                    {
                        await Clients.Group(session.GroupName).SendAsync("AddMessage", args.Text);
                    }
                    else
                    {
                        await Clients.Client(args.ConnectionId).SendAsync("AddMessage", args.Text);
                    }
                }
            });
        }

        public async Task WorkerEndProcess(WorkerEndProcessArguments args)
        {
            await HubOperation(nameof(WorkerEndProcess), args, async () =>
            {
                var session = sessionManager.GetSession(args.SessionId);
                await Clients.Group(session.GroupName).SendAsync("AddSystemMessage", "Sesja zakończona przez serwer wykonawczy.");
                session.State = SessionState.Finished;
                sessionManager.Update(session);
            });
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return HubOperation(nameof(OnDisconnectedAsync), string.Empty, () =>
            {
                workerManager.WorkerDisconnection(Context.ConnectionId);
                return base.OnDisconnectedAsync(exception);
            });
        }

        #region Utils

        private dynamic GetWorkerContext(WorkerConnection worker)
        {
            if (worker == null)
            {
                throw new Exception("Brak dostępnych sererów wykonawczych.");
            }
            var workerContext = Clients.Client(worker.ConnectionId);
            if (workerContext == null)
            {
                throw new Exception("Brak połączenia z serwerem wykonawczym.");
            }
            return workerContext;
        }

        protected async Task HubOperation<T>(string methodName, T arguments, Func<Task> action)
        {
            string log;
            var user = Context.User.Identity;
            string argumentsString = arguments != null ? arguments.ToString() : "";
            if (user != null)
            {
                log = $"ChatHub: User: {user.Name} Method: {methodName} Args: {argumentsString}";
            }
            else
            {
                log = $"ChatHub: Method: {methodName} Args: {argumentsString}";
            }

            logger.Info(log);
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                logger.Error("Błąd w SignalR Hub", ex);
                throw;
            }
        }

        #endregion
    }
}
