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

namespace ChatApp.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("ChatRoomList");
        }

        public ActionResult ChatRoomList()
        {
            ChatRoomListViewModel model = new ChatRoomListViewModel();
            model.Sessions = SessionManager.Instance.GetActiveSessions();

            return View(model);
        }

        [Authorize]
        public ActionResult SessionRoom(long? sessionId)
        {
            if (!sessionId.HasValue)
            {
                return RedirectToAction("ChatRoomList");
            }
            var session = SessionManager.Instance.GetSession(sessionId.Value);
            if (session == null)
            {
                return HttpNotFound();
            }
            var chatRoomModel = new ChatRoomViewModel
            {
                SessionId = sessionId.Value,
                ChatRoomName = session.AppName,
            };

            return View("ChatRoom", chatRoomModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult StartNewSession(string appName)
        {
            string userId = User.Identity.GetUserId();
            var session = SessionManager.Instance.StartNewSession(userId, appName);
            return RedirectToAction("SessionRoom", new RouteValueDictionary { { "sessionId", session.Id } });
        }

        [HttpPost]
        [Authorize]
        public ActionResult JoinToSession(long? sessionId)
        {
            return RedirectToAction("SessionRoom", new RouteValueDictionary { { "sessionId", sessionId } });
        }

        public ActionResult GetDialogMsg()
        {
            System.Threading.Thread.Sleep(500);
            return Json(new { msg = "Jakaś wiadomość."});
        }
    }
}