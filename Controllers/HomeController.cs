﻿using System.Web.Mvc;

namespace WebAPISuite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult Contact()
        {
            return View();
        }
    }
}
