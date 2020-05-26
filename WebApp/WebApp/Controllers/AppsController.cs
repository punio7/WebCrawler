using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCrawler.WebApp.DbModel.Extensions;
using WebCrawler.WebApp.Logic;
using WebCrawler.WebApp.WebApp.Models.Apps;

namespace WebApp.Controllers
{
    public class AppsController : Controller
    {
        private readonly SessionManager sessionManager;

        public AppsController(SessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("ListApps");
        }

        [HttpGet]
        public ActionResult ListApps()
        {
            ListAppsViewModel model = new ListAppsViewModel();

            return View(model);
        }


        [HttpGet]
        public ActionResult ListSessions()
        {
            ListSessionsViewModel model = new ListSessionsViewModel();
            model.Sessions = sessionManager.GetActiveSessions();

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult SessionRoom(long? sessionId, string appName)
        {
            if (!sessionId.HasValue)
            {
                return RedirectToAction("ListSessions");
            }
            var session = sessionManager.GetSession(sessionId.Value);
            if (session == null || session.AppName != appName)
            {
                return NotFound($"SessionRoom not found for App: {appName} and Id: {sessionId}");
            }
            var sessionRoomViewModel = new SessionRoomViewModel
            {
                SessionId = sessionId.Value,
                AppName = session.AppName,
                IsOwner = session.CanSendCommands(User.GetUserId()),
            };

            return View(appName, sessionRoomViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult StartNewSession(string appName)
        {
            string userId = User.GetUserId();
            var session = sessionManager.StartNewSession(userId, appName);
            return RedirectToRoute("Apps", new { controller = "Apps", action = "SessionRoom", sessionId = session.Id, appName });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult JoinToSession(long? sessionId)
        {
            var session = sessionManager.GetSession(sessionId.Value);
            return RedirectToRoute("Apps", new { controller = "Apps", action = "SessionRoom", sessionId, appName = session?.AppName });
        }
    }
}