using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace WebCrawler.ChatApp.Controllers
{
    public class ErrorController : Controller
    {
        protected ILog logger = log4net.LogManager.GetLogger(typeof(ErrorController));

        public ActionResult Index()
        {
            return RedirectToAction("GenericError", new HandleErrorInfo(new HttpException(403, "Dont allow access the error pages"), "ErrorController", "Index"));
        }

        public ViewResult GenericError(HandleErrorInfo exception)
        {
            logger.Error($"Error on {exception.ControllerName}.{exception.ActionName}", exception.Exception);
            Response.StatusCode = 500;
            ViewBag.Message = "500 - Wewnętrzny błąd serwera";
            return View("Error", exception);
        }

        public ViewResult NotFound()
        {
            return NotFoundException(null);
        }

        public ViewResult NotFoundException(HandleErrorInfo exception)
        {
            logger.Error($"404 on {HttpContext.Request.RawUrl}");
            logger.Error($"Error on {exception?.ControllerName}.{exception?.ActionName}", exception?.Exception);
            ViewBag.Title = "Strona nie została znaleziona";
            ViewBag.Message = "404 - szukana strona nie została znaleziona";
            Response.StatusCode = 404;
            return View("Error", exception);
        }
    }
}