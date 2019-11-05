using DAL.Models;
using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Mobile
{
    public class MobileBidingFileController : Controller
    {
        // GET: MobileBidingFile
        //[VerifyMobileLoginFilter]
        public ActionResult Index(string pid)
        {
            if (!string.IsNullOrEmpty(Request["userid"]))
            {
                ViewBag.UserId = Request["userid"].ToString();
                Session["UserId"] = Request["userid"].ToString();
            }
            else
                ViewBag.UserId = Session["UserId"].ToString();
            if (pid == null)
                return View("/MobileLogin");
            ViewBag.pid = pid;
            BidingFileContext bc = new BidingFileContext();
            DataTable dt = bc.getBidingFileDetail(pid);
            DataRow dr = dt.Rows[0];
            ViewBag.BidFileName = dr["Name"].ToString();
            ViewBag.BidFileContent = dr["Content"].ToString().Replace("\r\n", "<br/>");
            ViewBag.BidFileUserName = dr["Publisher"].ToString();
            ViewBag.BidFilePublishDate = dr["PublishDate"].ToString();
            ViewBag.BidFileProDescription = dr["ProDescription"].ToString();
            return View();
        }
        [VerifyMobileLoginFilter]
        public ActionResult AuditAction(string pid)
        {
            ViewBag.UserId = Session["UserId"].ToString();
            if (pid == null)
                return View("/MobileLogin");
            ViewBag.pid = pid;

            return View();
        }
    }
}