using RailBiding.Common;
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
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult Index()
        {
            ViewBag.UserName = Session["UserName"];
            ViewBag.UserDepartment = Session["UserDepartment"];
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult FileDetail()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult AddFile()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult FileApplication()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult FileApprove()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult FileApproveDetail(string pid)
        {
            ViewBag.pid = pid;
            return View();
        }
    }
}