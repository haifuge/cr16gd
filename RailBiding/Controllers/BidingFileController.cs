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
    public class BidingFileController : Controller
    {
        // GET: 招标文件
        public ActionResult Index()
        {
            ViewBag.ActiveMenu = Request["activeMenu"].ToString();
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

        public ActionResult FileApplication()
        {
            return View();
        }

        public ActionResult FileApplicationDetail(string fid)
        {
            if (fid == null)
                return View("/Login");
            BidingFileContext bc = new BidingFileContext();
            DataTable dt = bc.getBidingFileDetail(fid);
            DataRow dr = dt.Rows[0];
            ViewBag.BidFileName = dr["Name"].ToString();
            ViewBag.BidFileContent = dr["Content"].ToString().Replace("\r\n", "<br/>");
            ViewBag.BidFileUserName = dr["UserName"].ToString();
            ViewBag.BidFilePublishDate = dr["PublishDate"].ToString();

            return View();
        }

        public ActionResult FileDetail(string fid)
        {
            if (fid == null)
                return View("//Login");
            BidingFileContext bc = new BidingFileContext();
            DataTable dt = bc.getBidingFileDetail(fid);
            DataRow dr = dt.Rows[0];
            ViewBag.BidFileName = dr["Name"].ToString();
            ViewBag.BidFileContent = dr["Content"].ToString().Replace("\r\n","<br/>");
            ViewBag.BidFileUserName = dr["UserName"].ToString();
            ViewBag.BidFilePublishDate = dr["PublishDate"].ToString();

            return View();
        }

        public ActionResult FileApprove()
        {
            return View();
        }

        public ActionResult FileApproveDetail(string fid)
        {
            if (fid == null)
                return View("//Login");
            BidingFileContext bc = new BidingFileContext();
            DataTable dt = bc.getBidingFileDetail(fid);
            DataRow dr = dt.Rows[0];
            ViewBag.BidFileName = dr["Name"].ToString();
            ViewBag.BidFileContent = dr["Content"].ToString().Replace("\r\n", "<br/>");
            ViewBag.BidFileUserName = dr["UserName"].ToString();
            ViewBag.BidFilePublishDate = dr["PublishDate"].ToString();

            return View();
        }

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