using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Controllers
{
    public class MakeBidFileController : Controller
    {
        // GET: 定标文件
        public ActionResult Index()
        {
            ViewBag.ActiveMenu = Request["activeMenu"].ToString();
            ViewBag.UserName = Session["UserName"];
            ViewBag.UserDepartment = Session["UserDepartment"];
            return View();
        }

        public ActionResult FileDetail()
        {
            return View();
        }

        public ActionResult AddFile()
        {
            return View();
        }

        public ActionResult FileApplication()
        {
            return View();
        }

        public ActionResult FileApprove()
        {
            return View();
        }

        public ActionResult FileApproveDetail()
        {
            return View();
        }
    }
}