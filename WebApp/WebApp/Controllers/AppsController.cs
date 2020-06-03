using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCrawler.WebApp.DbModel.Extensions;
using WebCrawler.WebApp.Logic;
using WebCrawler.WebApp.WebApp.Models.Apps;

namespace WebApp.Controllers
{
    public class AppsController : Controller
    {
        private readonly AppManager appManager;
        private readonly SessionManager sessionManager;
        private readonly IMapper mapper;

        public AppsController(AppManager appManager, SessionManager sessionManager, IMapper mapper)
        {
            this.appManager = appManager;
            this.sessionManager = sessionManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction(nameof(ListApps));
        }

        [HttpGet]
        [Authorize]
        public ActionResult ListApps()
        {
            var apps = appManager.GetAppDefinitions();
            ListAppsViewModel model = mapper.Map<ListAppsViewModel>(apps);

            return View(model);
        }

        [HttpGet]
        public ActionResult ListSessions()
        {
            var sessions = sessionManager.GetActiveSessions();
            ListSessionsViewModel model = mapper.Map<ListSessionsViewModel>(sessions);

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult SessionRoom(string appName, long sessionId)
        {
            var session = sessionManager.GetSession(sessionId);
            if (session == null || session.App.Name != appName)
            {
                return NotFound($"SessionRoom not found for App: {appName} and Id: {sessionId}");
            }
            var model = mapper.Map<SessionRoomViewModel>(session);
            model.IsOwner = session.CanSendCommands(User.GetUserId());

            return View(appName, model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult StartNewSession([FromForm]long appId)
        {
            string userId = User.GetUserId();
            var session = sessionManager.StartNewSession(userId, appId);
            return RedirectToRoute("Apps", new { controller = "Apps", action = nameof(SessionRoom), appName = session?.App.Name, sessionId = session.Id });
        }

        [HttpGet]
        [Authorize]
        public ActionResult JoinToSession(long? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ListSessions));
            }
            var session = sessionManager.GetSession(id.Value);
            return RedirectToRoute("Apps", new { controller = "Apps", action = nameof(SessionRoom), appName = session?.App.Name, sessionId = id.Value });
        }
    }
}