using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Mobile
{
    public class MobileResetPasswordController : Controller
    {
        // GET: MobileResetPassword
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }
    }
}