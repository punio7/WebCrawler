﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCrawler.ChatApp.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("GenericError", new HandleErrorInfo(new HttpException(403, "Dont allow access the error pages"), "ErrorController", "Index"));
        }

        public ViewResult GenericError(HandleErrorInfo exception)
        {
            return View("Error", exception);
        }

        public ViewResult NotFound(HandleErrorInfo exception)
        {
            ViewBag.Title = "Page Not Found";
            return View("Error", exception);
        }
    }
}