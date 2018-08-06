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
using AliMessage;

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

        public string GetBidingApproves(string pageSize, string pageIndex, string pname, string status)
        {
            BidContext bc = new BidContext();

            return bc.GetBidingApproves(Session["UserId"].ToString(), pageSize, pageIndex, pname, status);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemB")]
        public ActionResult BidingApproveDetail(string pid)
        {
            ViewBag.UserId = Session["UserId"].ToString();
            ViewBag.RoleId = Session["RoleId"].ToString();
            ViewBag.status = Request["status"].ToString();
            if (pid == null)
                return View("/Login");
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


                string removebtn = "";;
            if(Session["RoleId"].ToString()=="2"&& Request["status"].ToString()=="1")
            {
                ViewBag.InviteCompanyBtn = @"<a href='javascript:;' class='js-cancle-meet' id='invitebtn' onclick='inviteCompanys()' title='邀标'><i class='meet-icon icon-cancel icon-yb'>邀标</i></a>";
                ViewBag.addCompanysbtn = "<button type='submit' class='add-qy' style='width: 80px;'>"+
                                            "<a href='#' onclick=\"ShowDiv('MyDiv','fade')\" style='color: #fff'>"+
                                                "<img src='/img/icon-add3.png' style='vertical-align: middle' alt=''>添加企业"+
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
            cHtml.Append("<tr class='form-tr detail-user-con invitejoincom'><td colspan='2'><div class='detail-user-list1' style='overflow: auto;'><div class='meet-user-span' id='inviteCompany'>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var cid = "company" + dt.Rows[i]["id"].ToString();
                cHtml.Append("<span id='" + cid + "'>" + dt.Rows[i]["Name"].ToString() +string.Format(removebtn, cid) + "</span>");
            }
            cHtml.Append(@"</div></div></td></tr>");
            ViewBag.inviteJoinCompanys = cHtml.ToString();

            dt = bc.GetBidingCompanys(pid);
            var joinC = (from c in dt.AsEnumerable()
                         where c.Field<int>("CompanyResponse") == 1
                         select new { name = c["Name"].ToString() }).ToList();
            var noJoinC = (from c in dt.AsEnumerable()
                           where c.Field<int>("CompanyResponse") == 2
                           select new { name = c["Name"].ToString() }).ToList();
            var noResponseC = (from c in dt.AsEnumerable()
                               where c.Field<int>("CompanyResponse") == 0
                               select new { name = c["Name"].ToString() }).ToList();
            ViewBag.joinNum = joinC.Count;
            ViewBag.noJoinNum = noJoinC.Count;
            ViewBag.noResponseNum = noResponseC.Count;
            cHtml = new StringBuilder();
            foreach (var c in joinC)
            {
                cHtml.Append("<span>" + c.name + "</span>");
            }
            ViewBag.JoinCompanys = cHtml.ToString();
            cHtml.Clear();
            foreach (var c in noJoinC)
            {
                cHtml.Append("<span>" + c.name + "</span>");
            }
            ViewBag.NoJoinCompanys = cHtml.ToString();
            cHtml.Clear();
            foreach (var c in noResponseC)
            {
                cHtml.Append("<span>" + c.name + "</span>");
            }
            ViewBag.NoResponseCompanys = cHtml.ToString();


            if (Session["RoleId"].ToString() == "2")
            {
                if (ViewBag.joinNum < 3){
                    ViewBag.approvebtn = "";
                }else
                {
                    ViewBag.approvebtn = @"<div class='deal-apply-btn deal-apply-btn2'>
                                                <a class='meet-btn big-btn blue-btn js-deal green-btn blue-btn2' onclick='approveApplication()'>审核通过</a>
                                            </div>";
                }
            }else
            {
                ViewBag.approvebtn = @"<div class='deal-apply-btn deal-apply-btn2'>
                                                <a class='meet-btn big-btn blue-btn js-deal green-btn blue-btn2' onclick='approveApplication()'>审核通过</a>
                                            </div>";
            }

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
        public void InviteBiding(string cids, string pid, string pname)
        {
            string[] cid = cids.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            SendMessage sm = new SendMessage();
            string sql = "select ID, RepPhone from Company where Id in (" + cids+ ") and ID in (select CompanyId from BidingCompany where ProjId=" + pid + " and VerifyCode is null)";
            DataTable dt = DBHelper.GetDataTable(sql);
            string guid = "";
            sql = "";
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0,19);
                pname = pname.Length > 20 ? pname.Substring(0, 20) : pname;
                string res=sm.InviteCompany(dt.Rows[i][1].ToString(), pname, guid,pid, dt.Rows[i][0].ToString());
                
                Log l = new Log();
                l.UserId = Session["UserId"].ToString();
                l.OperType = OperateType.InviteBiding;
                l.OperateDate = DateTime.Now.ToShortDateString();
                l.Description = res;
                LogContext.WriteLog(l);
                sql += " update BidingCompany set VerifyCode='"+guid+"' where ProjId = "+pid+" and CompanyId = "+dt.Rows[i][0].ToString()+"; ";
            }
            if(sql!="")
                DBHelper.ExecuteNonQuery(sql);
        }

        public string CheckCompanyInvited(string cids, string pid)
        {
            string sql = "select count(1) from Company where Id in ("+cids+") and ID in (select CompanyId from BidingCompany where ProjId="+pid+" and VerifyCode is null)";
            return DBHelper.ExecuteScalar(sql);
        }
    }
}