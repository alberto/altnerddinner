﻿using System.Web.Mvc;

namespace NerdDinner.Controllers {

    [HandleError]
    public class HomeController : Controller {
    
        public ActionResult Index() {
            return View();
        }

        public ActionResult About() {
            return View();
        }
    }
}
