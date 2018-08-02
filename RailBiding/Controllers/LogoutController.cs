using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            Session.Abandon();
            if (Request["mobile"] != null)
                return View("\\MobileLogin");
            else
                return View("\\Login");
        }
    }
}