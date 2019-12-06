using DAL.Models;
using DAL.Tools;
using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Mobile
{
    public class MobileBidController : Controller
    {
        // GET: MobileBid
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

          
            if (Session["RoleId"].ToString() == "2" && Request["status"].ToString() == "1")
            {
                ViewBag.InviteCompanyBtn = @"<a href='javascript:;' class='js-cancle-meet' id='invitebtn' onclick='inviteCompanys()' title='邀标'><i class='meet-icon icon-cancel icon-yb'>邀标</i></a>";
                ViewBag.addCompanysbtn = "<button type='submit' class='add-qy' style='width: 80px;'>" +
                                            "<a href='#' onclick=\"ShowDiv('MyDiv','fade')\" style='color: #fff'>" +
                                                "<img src='/img/icon-add3.png' style='vertical-align: middle' alt=''>添加企业" +
                                            "</a></button>";
               
            }
            else
            {
                ViewBag.InviteCompanyBtn = "";
                ViewBag.addCompanysbtn = "";
            }
            //单位反馈
            dt = bc.GetBidingCompanys(pid);
            var joinC = (from c in dt.AsEnumerable()
                         where c.Field<int>("CompanyResponse") == 1
                         select new { id = c["id"].ToString(), name = c["Name"].ToString() }).ToList();

            //单位反馈-单位显示框
            StringBuilder comHtml = new StringBuilder();
            foreach (var c in joinC)
            {
                comHtml.Append("<li id='" + c.id + "'>" + c.name + "</li>");
            }
            ViewBag.JoinCompanys = comHtml.ToString();
    

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