using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Tools;

namespace RailBiding.Controllers
{
    public class SystemSetupController : Controller
    {
        // GET: SystemSetup
        public ActionResult Index()
        {
            ViewBag.ActiveMenu = Request["activeMenu"].ToString();
            ViewBag.UserName = Session["UserName"];
            ViewBag.UserDepartment = Session["UserDepartment"];
            return View();
        }

        public ActionResult PersonalCenter()
        {
            return View();
        }

        public ActionResult SystemLog()
        {
            return View();
        }
        public string GetLogs()
        {
            LogRecordContext lr = new LogRecordContext();
            DataTable dt = lr.GetLogRecords();
            return JsonHelper.DataTableToJSON(dt);
        }

        public ActionResult CategoryManagement()
        {
            return View();
        }
        public string GetSystemTypes()
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetSystemTypes();
            return JsonHelper.DataTableToJSON(dt);
        }

        public ActionResult SystemUser()
        {
            return View();
        }
        public string GetAdmins()
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetAdmins();
            return JsonHelper.DataTableToJSON(dt);
        }

        public ActionResult ApproveProcessManagement()
        {
            return View();
        }

        public ActionResult ApproveProcessDetail(string apid)
        {
            return View();
        }

        public string GetApproveProcesses()
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetApproveProcesses();
            return JsonHelper.DataTableToJSON(dt);
        }
    }
}