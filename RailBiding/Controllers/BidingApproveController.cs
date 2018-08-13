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
            
            //单位反馈
            dt = bc.GetBidingCompanys(pid); 
            var joinC = (from c in dt.AsEnumerable()
                         where c.Field<int>("CompanyResponse") == 1
                         select new { id=c["id"].ToString(), name = c["Name"].ToString() }).ToList();
            var noJoinC = (from c in dt.AsEnumerable()
                           where c.Field<int>("CompanyResponse") == 2
                           select new { id = c["id"].ToString(), name = c["Name"].ToString() }).ToList();
            var noResponseC = (from c in dt.AsEnumerable()
                               where c.Field<int>("CompanyResponse") == 0
                               select new { id = c["id"].ToString(), name = c["Name"].ToString() }).ToList();

            //单位反馈-单位显示框
            StringBuilder comHtml = new StringBuilder();
            foreach (var c in joinC)
            {
                comHtml.Append("<span><a href='/Companys/Details?id=" + c.id + "' target='_blank' >" + c.name + "</a></span>");
            }
            ViewBag.JoinCompanys = comHtml.ToString();
            comHtml.Clear();
            foreach (var c in noJoinC)
            {
                comHtml.Append("<span><a href='/Companys/Details?id=" + c.id + "' target='_blank'>" + c.name + "</a></span>");
            }
            ViewBag.NoJoinCompanys = comHtml.ToString();
            comHtml.Clear();
            foreach (var c in noResponseC)
            {
                comHtml.Append("<span><a href='/Companys/Details?id=" + c.id + "' target='_blank'>" + c.name + "</a></span>");
            }
            ViewBag.NoResponseCompanys = comHtml.ToString();

            StringBuilder cHtml = new StringBuilder();

            
            if (Session["RoleId"].ToString()=="2")
            {
                if (Request["status"].ToString() == "1")
                {
                    string removebtn ="";
                    removebtn = "<i><img src='/img/icon-del.png'  onclick=\"removeCompany('{0}')\"></i>";
                    ViewBag.InviteCompanyBtn = @"<a href='javascript:;' class='js-cancle-meet' id='invitebtn' onclick='inviteCompanys()' title='邀标'><i class='meet-icon icon-cancel icon-yb'>邀标</i></a>";
                    ViewBag.addCompanysbtn = "<tr class='form-tr detail-user-con tr-border ifortd'>" +
                                              "<td class='form-lable label2'>" +
                                                  "<span style ='color:#008cd6' > 参与单位：</span>" +
                                              "</td>" +
                                              "<td>" +
                                                    "<div class='detail-user'>" +
                                                            //"<button type='submit'  style=''>" +
                                                            "<a href='#' onclick=\"ShowDiv()\" class='add-qy' style='width: 80px;color: #fff'>" +
                                                                "<img src='/img/icon-add3.png' style='vertical-align: middle' alt=''>添加企业" +
                                                            "</a>" +
                                                         //"</button>" +
                                                     "</div>" +
                                              "</td >" +
                                           "</tr>";
                    //参与单位-单位显示框
                    dt = bc.GetBidingCompanys(pid);
                    cHtml.Append("<tr class='form-tr detail-user-con invitejoincom'><td colspan='2'><div class='detail-user-list1' style='overflow: auto;'><div class='meet-user-span' id='inviteCompany'>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var cid = "company" + dt.Rows[i]["id"].ToString();
                        cHtml.Append("<span id='" + cid + "'>" + dt.Rows[i]["Name"].ToString() + string.Format(removebtn, cid) + "</span>");
                    }
                    cHtml.Append(@"</div></div></td></tr>");
                    ViewBag.inviteJoinCompanys = cHtml.ToString();
                }
                else
                {
                    ViewBag.InviteCompanyBtn = "";
                    ViewBag.addCompanysbtn = " <tr class='form-tr detail-user-con tr-border ifortd'>" +
                              "<td class='form-lable label2'>" +
                                  "<span style ='color:#008cd6' > 参与单位：</span>" +
                              "</td>" +
                              "<td>" +
                                    "<div class='detail-user'>" +
                                     "</div>" +
                              "</td >" +
                           "</tr>";
                    //参与单位-单位显示框
                    dt = bc.GetBidingCompanys(pid);
                    cHtml.Append("<tr class='form-tr detail-user-con invitejoincom'><td colspan='2'><div class='detail-user-list1' style='overflow: auto;'><div class='meet-user-span' id='inviteCompany'>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var cid = "company" + dt.Rows[i]["id"].ToString();
                        cHtml.Append("<span id='" + cid + "'>" + dt.Rows[i]["Name"].ToString() + "</span>");
                    }
                    cHtml.Append(@"</div></div></td></tr>");
                    ViewBag.inviteJoinCompanys = cHtml.ToString();
                }
                
                ViewBag.comjion = @"<tr class='form-tr detail-user-con tr-border ifortd'>" +
                               "<td class='form-lable label2'>" +
                                   "<span style = 'color: #008cd6'> 单位反馈：</span>" +
                               "</td>" +
                               "<td>" +
                                   "<div class='detail-user'>" +
                                        "<a class='meet-btn medium-btn active'>参加<span>" + joinC.Count + "</span></a>" +
                                        "<a class='meet-btn medium-btn'>不参加<span>" + noJoinC.Count + "</span></a>" +
                                        "<a class='meet-btn medium-btn'>未响应<span>" + noResponseC.Count + "</span></a>" +
                                   "</div>" +
                               "</td>" +
                           "</tr>";
                ViewBag.ResponseCompanysHtml = @"<tr class='form-tr detail-user-con'>" +
                                            "<td colspan ='2' >" +
                                                "<div class='detail-user-list detail-user-list6' style='overflow: auto;'>" +
                                                    "<div class='meet-user-span' style='display: block;'>" + ViewBag.JoinCompanys + "</div>" +
                                                    "<div class='meet-user-span'>" + ViewBag.NoJoinCompanys + "</div>" +
                                                    "<div class='meet-user-span'>" + ViewBag.NoResponseCompanys + "</div>" +
                                                "</div>" +
                                            "</td>" +
                                        "</tr>";
            }
            else if (Session["RoleId"].ToString() == "1" )
            {
                //参与单位-单位显示框
                dt = bc.GetBidingCompanys(pid);
                cHtml.Append("<tr class='form-tr detail-user-con invitejoincom'><td colspan='2'><div class='detail-user-list1' style='overflow: auto;'><div class='meet-user-span' id='inviteCompany'>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var cid = "company" + dt.Rows[i]["id"].ToString();
                    cHtml.Append("<span id='" + cid + "'>" + dt.Rows[i]["Name"].ToString() +"</span>");
                }
                cHtml.Append(@"</div></div></td></tr>");
                ViewBag.inviteJoinCompanys = cHtml.ToString();
                ViewBag.InviteCompanyBtn ="";
                ViewBag.addCompanysbtn = "<tr class='form-tr detail-user-con tr-border ifortd'>" +
                                              "<td class='form-lable label2'>" +
                                                  "<span style ='color:#008cd6' > 参与单位：</span>" +
                                              "</td>" +
                                              "<td>" +
                                                    "<div class='detail-user'>" +
                                                    "</div>" +
                                              "</td >" +
                                           "</tr>";
                ViewBag.comjion = @"<tr class='form-tr detail-user-con tr-border ifortd'>" +
                               "<td class='form-lable label2'>" +
                                   "<span style = 'color: #008cd6'> 单位反馈：</span>" +
                               "</td>" +
                               "<td>" +
                                   "<div class='detail-user'>" +
                                        "<a class='meet-btn medium-btn active'>参加<span>" + joinC.Count + "</span></a>" +
                                        "<a class='meet-btn medium-btn'>不参加<span>" + noJoinC.Count + "</span></a>" +
                                        "<a class='meet-btn medium-btn'>未响应<span>" + noResponseC.Count + "</span></a>" +
                                   "</div>" +
                               "</td>" +
                           "</tr>";
                ViewBag.ResponseCompanysHtml = @"<tr class='form-tr detail-user-con'>" +
                                            "<td colspan ='2' >" +
                                                "<div class='detail-user-list detail-user-list6' style='overflow: auto;'>" +
                                                    "<div class='meet-user-span' style='display: block;'>" + ViewBag.JoinCompanys + "</div>" +
                                                    "<div class='meet-user-span'>" + ViewBag.NoJoinCompanys + "</div>" +
                                                    "<div class='meet-user-span'>" + ViewBag.NoResponseCompanys + "</div>" +
                                                "</div>" +
                                            "</td>" +
                                        "</tr>";
            }
            else
            {
                ViewBag.inviteJoinCompanys ="";
                ViewBag.InviteCompanyBtn = "";
                ViewBag.addCompanysbtn = "";
                ViewBag.comjion = "";
                ViewBag.ResponseCompanysHtml = "";
            }

     //判断审核通过按钮显示与隐藏
            //if (Session["RoleId"].ToString() == "2")
            //{
            //    if (joinC.Count < 5){
            //        ViewBag.approvebtn = "";
            //    }else
            //    {
            //        ViewBag.approvebtn = @"<div class='deal-apply-btn deal-apply-btn2'>
            //                                    <a class='meet-btn big-btn blue-btn js-deal green-btn blue-btn2' onclick='approveApplication()'>审核通过</a>
            //                                </div>";
            //    }
            //}else
            //{
                ViewBag.approvebtn = @"<div class='deal-apply-btn deal-apply-btn2'>
                                                <a class='meet-btn big-btn blue-btn js-deal green-btn blue-btn2' onclick='approveApplication()'>审核通过</a>
                                            </div>";
            //}

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