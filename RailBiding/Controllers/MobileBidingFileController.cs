using DAL.Models;
using DAL.Tools;
using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            if (!string.IsNullOrEmpty(Request["lcode"]))
            {
                string code = Request["lcode"].ToString();
                ViewBag.UserId = Request["userid"].ToString();
                SqlParameter[] paras = new SqlParameter[2];
                paras[0] = new SqlParameter("@uid", ViewBag.UserId);
                paras[1] = new SqlParameter("@code", code);
                string s = DBHelper.ExecuteSP("CheckLoginStatus", paras).Tables[0].Rows[0][0].ToString();
                if (s == "1")
                    Session["UserId"] = Request["userid"].ToString();
                else
                    Response.Redirect("/MobileLogin");
            }
            else if (Session["UserId"] != null)
            {
                ViewBag.UserId = Session["UserId"].ToString();
            }
            else
            {
                Response.Redirect("/MobileLogin");
            }
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