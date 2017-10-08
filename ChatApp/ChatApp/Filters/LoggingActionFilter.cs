using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCrawler.ChatApp.Filters
{
    public class LoggingActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var logger = log4net.LogManager.GetLogger(filterContext.Controller.GetType());
            var user = filterContext.HttpContext.User;
            string userName = user != null ? user.Identity.Name : String.Empty;
            string controllerName = filterContext.Controller.GetType().Name;
            string actionName = filterContext.ActionDescriptor.ActionName;

            logger.Info($"User: {userName} Controller: {controllerName} Action: {actionName}");

            base.OnActionExecuting(filterContext);
        }
    }
}