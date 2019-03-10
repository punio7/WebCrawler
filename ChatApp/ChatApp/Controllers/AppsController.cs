using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebCrawler.ChatApp.Models;
using WebCrawler.ChatApp.Logic;
using ChatApp.HubModel;
using WebCrawler.HubModel.ServerModels;
using WebCrawler.ChatApp.Filters;

namespace ChatApp.Controllers
{
    public class AppsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            AppsListViewModel model = new AppsListViewModel();
            model.Sessions = SessionManager.Instance.GetActiveSessions();

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult SessionRoom(long? sessionId, string appName)
        {
            if (!sessionId.HasValue)
            {
                return RedirectToAction("List");
            }
            var session = SessionManager.Instance.GetSession(sessionId.Value);
            if (session == null || session.AppName != appName)
            {
                throw new HttpException(404, $"SessionRoom not found for App: {appName} and Id: {sessionId}");
            }
            var appRoomModel = new AppViewModel
            {
                SessionId = sessionId.Value,
                AppName = session.AppName,
                IsOwner = session.CanSendCommands(User.Identity.GetUserId()),
            };

            return View(appName, appRoomModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult StartNewSession(string appName)
        {
            string userId = User.Identity.GetUserId();
            var session = SessionManager.Instance.StartNewSession(userId, appName);
            return RedirectToRoute("Apps", new { controller = "Apps", action = "SessionRoom", sessionId = session.Id, appName } );
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult JoinToSession(long? sessionId)
        {
            var session = SessionManager.Instance.GetSession(sessionId.Value);
            return RedirectToRoute("Apps", new { controller = "Apps", action = "SessionRoom", sessionId, appName = session?.AppName } );
        }

        public ActionResult GetDialogMsg()
        {
            System.Threading.Thread.Sleep(500);
            return Json(new { msg = "Jakaś wiadomość."});
        }
    }
}