using DAL.Models;
using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Mobile
{
    public class MobileBidController : Controller
    {
        // GET: MobileBid
        [VerifyMobileLoginFilter]
        public ActionResult Index(string pid)
        {
            ViewBag.UserId = Session["UserId"].ToString();
            if (pid == null)
                return View("\\MobileLogin");
            ViewBag.pid = pid;
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidDetail(pid);
            DataRow dr = dt.Rows[0];
            ViewBag.Name = dr["Name"].ToString();
            ViewBag.Location = dr["Location"].ToString();
            ViewBag.Content = dr["Content"].ToString();
            ViewBag.Type = dr["ProjType"].ToString();
            ViewBag.Publisher = dr["Publisher"].ToString();
            ViewBag.BidingNum = dr["BidingNum"].ToString();
            ViewBag.ApplyDate = dr["ApplyDate"].ToString();
            ViewBag.OpenDate = dr["OpenDate"].ToString();
            ViewBag.PublishDate = dr["PublishDate"].ToString();
            ViewBag.ProjDescription = dr["ProDescription"].ToString();
            ViewBag.Content = dr["Content"].ToString();

            string removebtn = ""; ;
            if (Session["RoleId"].ToString() == "2" && Request["status"].ToString() == "1")
            {
                ViewBag.InviteCompanyBtn = @"<a href='javascript:;' class='js-cancle-meet' id='invitebtn' onclick='inviteCompanys()' title='邀标'><i class='meet-icon icon-cancel icon-yb'>邀标</i></a>";
                ViewBag.addCompanysbtn = "<button type='submit' class='add-qy' style='width: 80px;'>" +
                                            "<a href='#' onclick=\"ShowDiv('MyDiv','fade')\" style='color: #fff'>" +
                                                "<img src='/img/icon-add3.png' style='vertical-align: middle' alt=''>添加企业" +
                                            "</a></button>";
                removebtn = "<i><img src='/img/icon-del.png'  onclick=\"removeCompany('{0}')\"></i>";
            }
            else
            {
                ViewBag.InviteCompanyBtn = "";
                ViewBag.addCompanysbtn = "";
            }
            dt = bc.GetBidingCompanys(pid);
            StringBuilder cHtml = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var cid = "company" + dt.Rows[i]["id"].ToString();
                cHtml.Append("<li id='" + cid + "'>" + dt.Rows[i]["Name"].ToString() + string.Format(removebtn, cid) + "</li>");
            }
            ViewBag.JoinCompanys = cHtml.ToString();
            return View();
        }
        [VerifyMobileLoginFilter]
        public ActionResult AuditAction(string pid)
        {
            ViewBag.UserId = Session["UserId"].ToString();
            if (pid == null)
                return View("\\MobileLogin");
            ViewBag.pid = pid;
            return View();
        }
    }
}