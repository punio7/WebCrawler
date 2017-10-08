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
        private static Dictionary<int, string> chatRoomNames;
        
        static ChatController()
        {
            chatRoomNames = new Dictionary<int, string>
            {
                {1, "Pokój 1" },
                {2, "Pokój 2" },
                {3, "Pokój 3" },
                {4, "Pokój 4" },
                {5, "Pokój 5" }
            };
        }

        // GET: Chat
        public ActionResult Index()
        {
            return RedirectToAction("ChatRoomList");
        }

        [Authorize]
        public ActionResult ChatRoom(int? chatRoomId)
        {
            if (!chatRoomId.HasValue)
            {
                return RedirectToAction("ChatRoomList");
            }

            var chatRoomInfo = new ChatRoomViewModel
            {
                ChatRoomId = chatRoomId.Value,
                ChatRoomName = chatRoomNames[chatRoomId.Value]
            };
            string userId = User.Identity.GetUserId();
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            chatRoomInfo.User = user;

            return View(chatRoomInfo);
        }

        [Authorize]
        public ActionResult SessionRoom(long? sessionId)
        {
            if (!sessionId.HasValue)
            {
                return RedirectToAction("ChatRoomList");
            }
            var session = SessionManager.Instance.GetSession(sessionId.Value);
            var chatRoomModel = new ChatRoomViewModel
            {
                SessionId = sessionId.Value,
                ChatRoomName = session.AppName,
            };

            return View("ChatRoom", chatRoomModel);
        }

        public ActionResult RandomChatRoom()
        {
            Random random = new Random();
            int chatRoomId = random.Next(0, chatRoomNames.Keys.Count);

            return RedirectToAction("ChatRoom", new RouteValueDictionary { { "chatRoomId", chatRoomId } });
        }

        public ActionResult ChatRoomList()
        {
            var model = new ChatRoomListViewModel();
            model.Sessions = SessionManager.Instance.GetActiveSessions();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddChatRoom(string newRoomName)
        {
            int newChatRoomId = chatRoomNames.Keys.Max() +1;
            chatRoomNames.Add(newChatRoomId, newRoomName);
            return RedirectToAction("ChatRoom", new RouteValueDictionary { { "chatRoomId", newChatRoomId } });
        }

        [HttpPost]
        public ActionResult StartNewSession(string appName)
        {
            string userId = User.Identity.GetUserId();
            var session = SessionManager.Instance.StartNewSession(userId, appName);
            return RedirectToAction("SessionRoom", new RouteValueDictionary { { "sessionId", session.Id } });
        }

        public ActionResult GetDialogMsg()
        {
            System.Threading.Thread.Sleep(500);
            return Json(new { msg = "Jakaś wiadomość."});
        }
    }
}