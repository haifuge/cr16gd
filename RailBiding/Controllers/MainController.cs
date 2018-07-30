using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        [VerifyLoginFilter]
        public ActionResult Index()
        {
            ViewBag.UserName = Session["UserName"];
            ViewBag.UserDepartment = Session["UserDepartment"];
            ViewBag.RoleId = Session["RoleId"];
            return View();
        }
    }
}