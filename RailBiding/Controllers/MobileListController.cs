using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Mobile
{
    public class MobileListController : Controller
    {
        // GET: MobileList
        public ActionResult Index()
        {
            ViewBag.UserName = Session["UserName"];
            ViewBag.UserDepartment = Session["UserDepartment"];
            ViewBag.RoleId = Session["RoleId"];
            return View();
        }
    }
}