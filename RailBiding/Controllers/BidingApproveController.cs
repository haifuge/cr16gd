using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Tools;
using RailBiding.Common;

namespace RailBiding.Controllers
{
    public class BidingApproveController : Controller
    {
        // GET: BidingApprove
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemB")]
        public ActionResult Index()
        {
            return View();
        }

        public string GetBidingApproves(string pageSize, string pageIndex)
        {
            BidContext bc = new BidContext();

            return bc.GetBidingApproves(Session["UserId"].ToString(), pageSize, pageIndex);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemB")]
        public ActionResult BidingApproveDetail(string pid)
        {
            if (pid == null)
                return View("\\Login");
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
            
            if(Session["RoleId"].ToString()=="2")
            {
                ViewBag.InviteCompanyBtn = @"<a href='javascript:;' class='js-cancle-meet' id='invitebtn' onclick='inviteCompanys()' title='邀标'><i class='meet-icon icon-cancel icon-yb'>邀标</i></a>";
                ViewBag.addCompanysbtn = @"<button type='submit' class='add-qy' style='width: 80px;'>
                                            <a href='#' onclick='ShowDiv('MyDiv','fade')' style='color: #fff'>
                                                <img src='~/img/icon-add3.png' style='vertical-align: middle' alt=''>添加企业
                                            </a>
                                        </button>";
            }
            else
            {
                ViewBag.InviteCompanyBtn = "";
                ViewBag.addCompanysbtn = "";
            }
            dt = bc.GetBidingCompanys(pid);
            StringBuilder cHtml = new StringBuilder();
            cHtml.Append("<tr class='form-tr detail-user-con'><td colspan='2'><div class='detail-user-list' style='min-height:215px;overflow: auto;'><div class='meet-user-span'>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var cid = "company" + dt.Rows[i]["id"].ToString();
                cHtml.Append("<span id='" + cid + "'>" + dt.Rows[i]["Name"].ToString() + "<i><img src='/img/icon-del.png'  onclick=\"removeCompany('" + cid + "')\"></i></span>");
            }
            cHtml.Append(@"</div></div></td></tr>");
            ViewBag.JoinCompanys = cHtml.ToString();

            return View();
        }

        public string GetBidingApproveDetail(string bid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidingApproveDetail(bid);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string GetBidApplicationAuditComment(string bid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidApplicationAuditComment(bid);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string GetBidApplicationTransferInfo(string bid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidApplicationTransferInfo(bid);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string ProcessBidApplication()
        {
            string operation = Request["operation"].ToString();
            string comment = Request["comment"] ?? "";
            BidContext bc = new BidContext();
            bool suc = bc.ProcessBidApplication(operation, comment);
            return suc.ToString();
        }
        public void AddBidingCompany()
        {
            string pid = Request["pid"].ToString();
            string cid = Request["cid"].ToString();
            BidContext bc = new BidContext();
            bc.AddBidingCompany(pid, cid);
        }
        public void RemoveBidingCompany()
        {
            string pid = Request["pid"].ToString();
            string cid = Request["cid"].ToString();
            BidContext bc = new BidContext();
            bc.RemoveBidingCompany(pid, cid);
        }
    }
}