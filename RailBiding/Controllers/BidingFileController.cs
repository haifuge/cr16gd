using System;
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
            ViewBag.UserName = Session["UserName"];
            ViewBag.UserDepartment = Session["UserDepartment"];
            return View();
        }
        public string GetBidingFiles() {
            BidingFileContext bc = new BidingFileContext();
            DataTable dt = bc.GetBidingFiles();
            string str = JsonHelper.DataTableToJSON(dt);
            return str;
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

            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemF")]
        public ActionResult FileDetail(string fid)
        {
            if (fid == null)
                return View("//Login");
            BidingFileContext bc = new BidingFileContext();
            DataTable dt = bc.getBidingFileDetail(fid);
            DataRow dr = dt.Rows[0];
            ViewBag.BidFileName = dr["Name"].ToString();
            ViewBag.BidFileContent = dr["Content"].ToString().Replace("\r\n","<br/>");
            ViewBag.BidFileUserName = dr["Publisher"].ToString();
            ViewBag.BidFilePublishDate = dr["PublishDate"].ToString();

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
            if (pid == null)
                return View("//Login");
            ViewBag.pid = pid;
            BidingFileContext bc = new BidingFileContext();
            DataTable dt = bc.getBidingFileDetail(pid);
            DataRow dr = dt.Rows[0];
            ViewBag.BidFileName = dr["Name"].ToString();
            ViewBag.BidFileContent = dr["Content"].ToString().Replace("\r\n", "<br/>");
            ViewBag.BidFileUserName = dr["Publisher"].ToString();
            ViewBag.BidFilePublishDate = dr["PublishDate"].ToString();

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
    }
}