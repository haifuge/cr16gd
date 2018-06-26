using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Controllers
{
    public class LogsController : Controller
    {
        // GET: Logs
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetLog()
        {
            int page = int.Parse(Request["page"].ToString());
            int pageSize = int.Parse(Request["pageSize"].ToString());
            LogContext lc = new LogContext();
            List<Log> logs = lc.GetLogsOfPage(page, pageSize);
            return Json(logs, JsonRequestBehavior.AllowGet);
        }
        public int GetTotalLogNum()
        {
            LogContext lc = new LogContext();
            return lc.GetTotalLogNum();
        }
    }
}