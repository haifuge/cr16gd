﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Tools;
using RailBiding.Common;

namespace RailBiding.Controllers
{
    public class BidingFileController : Controller
    {
        // GET: 招标文件
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemF")]
        public ActionResult Index()
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("BidingFile", Session["RoleId"].ToString());
            ViewBag.UserName = Session["UserName"];
            ViewBag.UserDepartment = Session["UserDepartment"];
            return View();
        }
        public string GetBidingFiles(string pageSize, string pageIndex, string pname) {
            BidingFileContext bc = new BidingFileContext();
            return bc.GetBidingFiles(pageSize, pageIndex, pname, Session["UserId"].ToString());
        }
        public string GetMyFileApprove(string pageSize, string pageIndex, string pname, string status)
        {
            BidingFileContext bc = new BidingFileContext();
            string userid = Session["UserId"].ToString();
            return bc.GetMyFileApprove(userid, pageSize, pageIndex, pname, status);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemF")]
        public ActionResult FileApplication()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemF")]
        public ActionResult FileApplicationDetail(string pid)
        {
            if (pid == null)
                return View("/Login");
            ViewBag.pid = pid;
            BidingFileContext bc = new BidingFileContext();
            DataTable dt = bc.getBidingFileDetail(pid);
            DataRow dr = dt.Rows[0];
            ViewBag.BidFileName = dr["Name"].ToString();
            ViewBag.BidFileContent = dr["Content"].ToString().Replace("\r\n", "<br/>");
            ViewBag.BidFileUserName = dr["UserName"].ToString();
            ViewBag.BidFilePublishDate = dr["PublishDate"].ToString();
            ViewBag.RoleId = Session["RoleId"].ToString();

            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemF")]
        public ActionResult FileDetail(string pid)
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("BidingFile", Session["RoleId"].ToString());
            if (pid == null)
                return View("/Login");
            ViewBag.pid = pid;
            BidingFileContext bc = new BidingFileContext();
            DataTable dt = bc.getBidingFileDetail(pid);
            DataRow dr = dt.Rows[0];
            ViewBag.BidFileName = dr["Name"].ToString();
            ViewBag.BidFileContent = dr["Content"].ToString().Replace("\r\n","<br/>");
            ViewBag.BidFileUserName = dr["Publisher"].ToString();
            ViewBag.BidFilePublishDate = dr["PublishDate"].ToString();
            ViewBag.BidFileProDescription = dr["ProDescription"].ToString();
            

            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemF")]
        public ActionResult FileApprove()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemF")]
        public ActionResult FileApproveDetail(string pid)
        {
            ViewBag.RoleId = Session["RoleId"].ToString();
            ViewBag.UserId = Session["UserId"].ToString();
            if (pid == null)
                return View("/Login");
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
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemF")]
        public ActionResult AddBidingFile()
        {
            return View();
        }

        [HttpPost]
        public void AddBidingFileContent()
        {

        }

        public ActionResult BidingFileApproveProcess()
        {
            return View();
        }

        public void DeleteApplication()
        {
            string pid = Request["pid"].ToString();
            string sql = "update Project set status=N'未发布' where Id=" + pid+"; delete BidingFile where ProjId="+pid+ "; delete AppProcessing where ObjId="+pid+" and AppProcId=2";
            DBHelper.ExecuteNonQuery(sql);
        }
    }
}