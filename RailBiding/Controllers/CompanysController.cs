﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DAL.Models;
using DAL.Tools;
using System.Text;
using RailBiding.Common;
using OperateExcel;
using System.Data.SqlClient;

namespace RailBiding.Controllers
{
    public class CompanysController : Controller
    {
        // GET: Companys
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName= "itemC")]
        public ActionResult Index()
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("Companys", Session["RoleId"].ToString());
            if (Session["RoleId"].ToString() == "2"){
                ViewBag.delebtn = "<a class='meet-btn red-btn small-size sm-btn' onclick='deleCompany()'>" +
                                   "<i class='xs-meet-icon icon-dele'></i>删除</a>";
                ViewBag.createbtn = "";
            }
            else if(Session["RoleId"].ToString() == "4")
            {
                ViewBag.delebtn = "";
                ViewBag.createbtn = @"<a class='meet-btn green-btn small-size sm-btn' href='/Companys/Create?inout=1'>
                <i class='xs-meet-icon icon-add'></i>添加</a>";
            }else
            {
                ViewBag.delebtn = "";
                ViewBag.createbtn = "";
            }
            return View();
        }

        // GET: Companys/Details/5
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult Details(int id)
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("Companys", Session["RoleId"].ToString());
            CompanyContext cc = new CompanyContext();
            DataTable dt = cc.GetCompany(id);
            DataRow dr = dt.Rows[0];
           
            ViewBag.Name = dr["Name"].ToString();
            ViewBag.CreditNo = dr["CreditNo"].ToString();
            ViewBag.CorporateRepresentative = dr["CorporateRepresentative"].ToString();
            ViewBag.RepPhone = dr["RepPhone"].ToString();
            ViewBag.RegisteredCapital = dr["RegisteredCapital"].ToString();
            ViewBag.BusinessScope = dr["BusinessScope"].ToString();
            ViewBag.Contact = dr["Contact"].ToString();
            ViewBag.ContactPhone = dr["ContactPhone"].ToString();
            ViewBag.BusinessType = dr["BusinessType"].ToString();
            ViewBag.ContactAddress = dr["ContactAddress"].ToString();
            ViewBag.QualificationLevel = dr["QualificationLevel"].ToString();
            ViewBag.ConstructionContent = dr["ConstructionContent"].ToString();
            ViewBag.Referre = dr["Referre"].ToString();
            ViewBag.AuditStatus = dr["AuditStatus"].ToString();
            ViewBag.Type = dr["Type"].ToString();
            ViewBag.id = dr["id"].ToString();
            ViewBag.AddYear = int.Parse(dr["AddYear"].ToString());

            string rootPath = Server.MapPath("../");
            dt = cc.GetCompanyReferee(id);
            if (dt.Rows.Count > 0)
                ViewBag.RefereFile = "<a href='" + dt.Rows[0]["FilePath"].ToString().Replace(rootPath, "/") + "', target='_blank'>" + dt.Rows[0]["FileName"].ToString() + "</a>";
            else
                ViewBag.RefereFile = "";

            string picHtml = "";
            string pic= dr["ReferreIDPic"].ToString();
            //if (pic != "")
            //{
            //    pic = pic.Replace(rootPath, "/");
            //    picHtml += @"<div class='ab_tab2_img'><div>
            //                       <a href = '" + pic+ @"' rel='group' class='pirobox_gall' title='推荐书'><img src = '" + pic + @"'></a>
            //                  </div><p style='text-align: center; margin-bottom: 2px;'>推荐书</p><p style='text-align: center;'></p></div>";
            //}
            pic = dr["BusinessLicensePic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a class='pirobox_gall' title='营业执照'><img src = '" + pic + @"' onclick='showPic(this)'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>营业执照</p><p style='text-align: center;'></p></div>";
            }//<a href = '" + pic + @"' rel='group' class='pirobox_gall' title='营业执照'></a>
            pic = dr["SecurityCertificatePic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a class='pirobox_gall' title='安全证书'><img src = '" + pic + @"' onclick='showPic(this)'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>安全证书</p><p style='text-align: center;'>" + dr["SecurityCertificateNo"].ToString() + @"<p></div>";
            }//href = '" + pic + @"' rel='group' 
            pic = dr["RepIDPic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a class='pirobox_gall' title='法人身份证'><img src = '" + pic + @"' onclick='showPic(this)'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>法人身份证</p><p style='text-align: center;'></p></div>";
            }//href = '" + pic + @"' rel='group' 
            pic = dr["ContactIDPic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a class='pirobox_gall' title='现场负责人身份证'><img src = '" + pic + @"' onclick='showPic(this)'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>现场负责人身份证</p><p style='text-align: center;'></p></div>";
            }//href = '" + pic + @"' rel='group' 
            dt = cc.GetZiZhiPics(id);
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                pic = dt.Rows[i]["PicPath"].ToString();
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                                    <a class='pirobox_gall' title='" + dt.Rows[i]["ZZName"].ToString() + @"'><img src = '" + pic + @"' onclick='showPic(this)'></a>
                                 </div><p style='text-align:center; margin-bottom: 2px;'>" + dt.Rows[i]["ZZName"].ToString() + @"</p><p style='text-align: center;'>" + dt.Rows[i]["ZZCode"].ToString() + @"</p></div>";
            }// href = '" + pic + @"' rel='group'
            ViewBag.CompanyPics = picHtml;
            dt = cc.GetWorkHistory(id);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr class='white-bg'>");
                sb.Append("<td>" + dt.Rows[i]["ProjectName"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ContractAmount"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["StartDate"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["EndDate"].ToString() + "</td>");
                sb.Append("<td class=’gray-time>" + dt.Rows[i]["DelayStatus"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["SettlementAmount"].ToString() + "</td>");
                sb.Append("<td class='gray-time'><a href='"+ dt.Rows[i]["FilePath"].ToString().Replace(rootPath, "/") + "' target='_blank'>" + dt.Rows[i]["TestifyFile"].ToString() + "</a></td>");
                sb.Append("</tr>");
            }
            ViewBag.WorkHistory = sb.ToString();

            if (Session["RoleId"].ToString() == "2") {
                ViewBag.editbtn = @"<a href ='/Companys/EditCompany?cid=" + id + @"' class='js-cancle-meet' title='编辑'>
                                    <i class='meet-icon icon-cancel icon-edits'>编辑</i>
                                  </a>";
            } else {
                ViewBag.editbtn = "";

            }

            if (ViewBag.AuditStatus == "0"&& Session["RoleId"].ToString() != "2")
            {
                ViewBag.addeditbtn = @"<a href ='#' class='js-cancle-meet' onclick='submitCompany()' title='提交'>
                                    <i class='meet-icon icon-cancel icon-edits'>提交</i>
                                  </a><a href ='/Companys/EditCompany?cid=" + id + @"' class='js-cancle-meet' title='编辑'>
                                    <i class='meet-icon icon-cancel icon-edits'>编辑</i>
                                  </a>";
            }
            else
            {
                ViewBag.addeditbtn = "";
            }
            if (ViewBag.AuditStatus == "0")
            {
                ViewBag.deletebtn = @"<a href ='#' class='js-cancle-meet' onclick='deleteCompany()' title='删除'>
                                    <i class='meet-icon icon-cancel icon-edits delete-edit' style='background:#ff0000'>删除</i>";
            }

            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult OutCompanyDetail(int id)
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("Companysout", Session["RoleId"].ToString());
            CompanyContext cc = new CompanyContext();
            DataTable dt = cc.GetCompany(id);
            DataRow dr = dt.Rows[0];
            ViewBag.Name = dr["Name"].ToString();
            ViewBag.id = dr["id"].ToString();
            ViewBag.CreditNo = dr["CreditNo"].ToString();
            ViewBag.CorporateRepresentative = dr["CorporateRepresentative"].ToString();
            ViewBag.RepPhone = dr["RepPhone"].ToString();
            ViewBag.RegisteredCapital = dr["RegisteredCapital"].ToString();
            ViewBag.BusinessScope = dr["BusinessScope"].ToString();
            ViewBag.Contact = dr["Contact"].ToString();
            ViewBag.ContactPhone = dr["ContactPhone"].ToString();
            ViewBag.BusinessType = dr["BusinessType"].ToString();
            ViewBag.ContactAddress = dr["ContactAddress"].ToString();
            ViewBag.QualificationLevel = dr["QualificationLevel"].ToString();
            ViewBag.ConstructionContent = dr["ConstructionContent"].ToString();
            ViewBag.Referre= dr["Referre"].ToString();
            ViewBag.AddYear = int.Parse(dr["AddYear"].ToString());

            string rootPath = Server.MapPath("../");
            string picHtml = "";
            string pic = dr["ReferreIDPic"].ToString();
            //if (pic != "")
            //{
            //    pic = pic.Replace(rootPath, "/");
            //    picHtml += @"<div class='ab_tab2_img'><div>
            //                 <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='推荐书'><img src = '" + pic + @"'></a>
            //                  </div><p style='text-align: center; margin-bottom: 2px;'>推荐书</p><p style='text-align: center;'></p></div>";
            //}
            pic = dr["BusinessLicensePic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='营业执照'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>营业执照</p><p style='text-align: center;'></p></div>";
            }
            pic = dr["SecurityCertificatePic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='安全证书'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>安全证书</p><p style='text-align: center;'>" + dr["SecurityCertificateNo"].ToString() + @"</p></div>";
            }
            pic = dr["RepIDPic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='法人身份证'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>法人身份证</p><p style='text-align: center;'></p></div>";
            }
            pic = dr["ContactIDPic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='现场负责人身份证'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>现场负责人身份证</p><p style='text-align: center;'></p></div>";
            }
            dt = cc.GetZiZhiPics(id);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pic = dt.Rows[i]["PicPath"].ToString();
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall'  title='" + dt.Rows[i]["ZZName"].ToString() + @"'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>" + dt.Rows[i]["ZZName"].ToString() + @"</p><p style='text-align: center;'>" + dt.Rows[i]["ZZCode"].ToString() + @"</p></div>";
            }
            ViewBag.CompanyPics = picHtml;

            dt = cc.GetWorkHistory(id);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr class='white-bg'>");
                sb.Append("<td>" + dt.Rows[i]["ProjectName"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ContractAmount"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["StartDate"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["EndDate"].ToString() + "</td>");
                sb.Append("<td class=’gray-time>" + dt.Rows[i]["DelayStatus"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["SettlementAmount"].ToString() + "</td>");
                sb.Append("<td class='gray-time'><a href='" + dt.Rows[i]["FilePath"].ToString().Replace(rootPath, "/") + "' target='_blank'>" + dt.Rows[i]["TestifyFile"].ToString() + "</a></td>");
                sb.Append("</tr>");
            }
            ViewBag.WorkHistory = sb.ToString();
            if (Session["RoleId"].ToString() == "2")
            {
                ViewBag.editbtn = @"<a href ='/Companys/EditCompany?cid=" + id + @"' class='js-cancle-meet' title='编辑'>
                                    <i class='meet-icon icon-cancel icon-edits'>编辑</i>
                                  </a>";
            }
            else
            {
                ViewBag.editbtn = "";
            }
            dt = cc.GetCompanyReferee(id);
            if (dt.Rows.Count > 0)
                ViewBag.RefereFile = "<a href='" + dt.Rows[0]["FilePath"].ToString().Replace(rootPath, "/") + "', target='_blank'>" + dt.Rows[0]["FileName"].ToString() + "</a>";
            else
                ViewBag.RefereFile = "";
            return View();
        }

        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult CompanysOut()
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("Companysout", Session["RoleId"].ToString());
            if (Session["RoleId"].ToString() == "2")
            {
                ViewBag.delebtn = "<a class='meet-btn red-btn small-size sm-btn' onclick='deleCompany()'>" +
                                   "<i class='xs-meet-icon icon-dele'></i>删除</a>";
                ViewBag.createbtn = "";
            }
            else if (Session["RoleId"].ToString() == "4")
            {
                ViewBag.delebtn = "";
                ViewBag.createbtn = @"<a class='meet-btn green-btn small-size sm-btn' href='/Companys/Create?inout=0'>
                                                    <i class='xs-meet-icon icon-add'></i>添加</a>
                                    <a class='meet-btn green-btn small-size sm-btn' style='width:90px' onclick='addProj()'>
                                                    <i class='xs-meet-icon icon-add'></i>添加项目</a>";
            }
            else
            {
                ViewBag.delebtn = "";
                ViewBag.createbtn = "";
            }
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult MyRecommend()
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("CompanysMyRecommend", Session["RoleId"].ToString());
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult RecommendDetail(int id)
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("CompanysMyRecommend", Session["RoleId"].ToString());
            ViewBag.cid = id;
            CompanyContext cc = new CompanyContext();
            DataTable dt = cc.GetCompany(id);
            DataRow dr = dt.Rows[0];
            ViewBag.Name = dr["Name"].ToString();
            ViewBag.CreditNo = dr["CreditNo"].ToString();
            ViewBag.CorporateRepresentative = dr["CorporateRepresentative"].ToString();
            ViewBag.RepPhone = dr["RepPhone"].ToString();
            ViewBag.RegisteredCapital = dr["RegisteredCapital"].ToString();
            ViewBag.BusinessScope = dr["BusinessScope"].ToString();
            ViewBag.Contact = dr["Contact"].ToString();
            ViewBag.ContactPhone = dr["ContactPhone"].ToString();
            ViewBag.BusinessType = dr["BusinessType"].ToString();
            ViewBag.ContactAddress = dr["ContactAddress"].ToString();
            ViewBag.QualificationLevel = dr["QualificationLevel"].ToString();
            ViewBag.ConstructionContent = dr["ConstructionContent"].ToString();
            ViewBag.Note = dr["Note"].ToString();
            ViewBag.Referre = dr["Referre"].ToString();
            ViewBag.apid = dr["Type"].ToString() == "1" ? "5" : "1";
            ViewBag.inout= dr["Type"].ToString();
            ViewBag.AuditStatus = dr["AuditStatus"].ToString();
            ViewBag.AddYear = dr["AddYear"].ToString();


            string rootPath = Server.MapPath("../");
            string picHtml = "";
            string pic = dr["ReferreIDPic"].ToString();
            ViewBag.moretime = "";
            if (Request["auditstatus"].ToString() == "3") {
                ViewBag.moretime = "<a href = 'javascript:;' class='js-cancle-meet' title='再次申请' onclick='moretime()'><i class='meet-icon icon-cancel icon-daooutbtn'>再次申请</i></a>";
            }
            //if (pic != "")
            //{
            //    pic = pic.Replace(rootPath, "/");
            //    picHtml += @"<div class='ab_tab2_img'><div>
            //                 <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='推荐书'><img src = '" + pic + @"'></a>
            //                  </div><p style='text-align: center; margin-bottom: 2px;'>推荐书</p><p style='text-align: center;'></p></div>";
            //}
            pic = dr["BusinessLicensePic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='营业执照'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>营业执照</p><p style='text-align: center;'></p></div>";
            }
            pic = dr["SecurityCertificatePic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='安全证书'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>安全证书</p><p style='text-align: center;'>" + dr["SecurityCertificateNo"].ToString() + @"</p></div>";
            }
            pic = dr["RepIDPic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='法人身份证'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>法人身份证</p><p style='text-align: center;'></p></div>";
            }
            pic = dr["ContactIDPic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='现场负责人身份证'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>现场负责人身份证</p><p style='text-align: center;'></p></div>";
            }
            dt = cc.GetZiZhiPics(id);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pic = dt.Rows[i]["PicPath"].ToString();
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall'  title='" + dt.Rows[i]["ZZName"].ToString() + @"'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>" + dt.Rows[i]["ZZName"].ToString() + @"</p><p style='text-align: center;'>" + dt.Rows[i]["ZZCode"].ToString() + @"</p></div>";
            }
            ViewBag.CompanyPics = picHtml;

            dt = cc.GetWorkHistory(id);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //sb.Append("<tr class='white-bg'><td>" + dt.Rows[i]["ProjectId"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ProjectName"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ContractAmount"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["StartDate"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["EndDate"].ToString() + "</td>");
                sb.Append("<td class=’gray-time>" + dt.Rows[i]["DelayStatus"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["SettlementAmount"].ToString() + "</td>");
                sb.Append("<td class='gray-time'><a href='" + dt.Rows[i]["FilePath"].ToString().Replace(rootPath, "/") + "' target='_blank'>" + dt.Rows[i]["TestifyFile"].ToString() + "</a></td>");
                sb.Append("</tr>");
            }
            ViewBag.WorkHistory = sb.ToString();

            dt = cc.GetCompanyReferee(id);
            if (dt.Rows.Count > 0)
                ViewBag.RefereFile = "<a href='" + dt.Rows[0]["FilePath"].ToString().Replace(rootPath, "/") + "', target='_blank'>" + dt.Rows[0]["FileName"].ToString() + "</a>";
            else
                ViewBag.RefereFile = "";
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult MyAudit()
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("CompanysMyAudit", Session["RoleId"].ToString());
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult AuditDetail(int id)
        {
            ViewBag.RoleId = Session["RoleId"].ToString();
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("CompanysMyAudit", Session["RoleId"].ToString());
            ViewBag.UserId = Session["UserId"].ToString();
            CompanyContext cc = new CompanyContext();
            DataTable dt = cc.GetCompany(id);
            DataRow dr = dt.Rows[0];
            ViewBag.cid = id;
            ViewBag.Name = dr["Name"].ToString();
            ViewBag.CreditNo = dr["CreditNo"].ToString();
            ViewBag.CorporateRepresentative = dr["CorporateRepresentative"].ToString();
            ViewBag.RepPhone = dr["RepPhone"].ToString();
            ViewBag.RegisteredCapital = dr["RegisteredCapital"].ToString();
            ViewBag.BusinessScope = dr["BusinessScope"].ToString();
            ViewBag.Contact = dr["Contact"].ToString();
            ViewBag.ContactPhone = dr["ContactPhone"].ToString();
            ViewBag.BusinessType = dr["BusinessType"].ToString();
            ViewBag.ContactAddress = dr["ContactAddress"].ToString();
            ViewBag.QualificationLevel = dr["QualificationLevel"].ToString();
            ViewBag.ConstructionContent = dr["ConstructionContent"].ToString();
            ViewBag.Note = dr["Note"].ToString();
            ViewBag.Referre = dr["Referre"].ToString();
            ViewBag.apid = dr["Type"].ToString() == "1" ? "5" : "1";
            ViewBag.AddYear = int.Parse(dr["AddYear"].ToString());

            string rootPath = Server.MapPath("../");
            string picHtml = "";
            string pic = dr["ReferreIDPic"].ToString();

            //if (pic != "")
            //{
            //    pic = pic.Replace(rootPath, "/");
            //    picHtml += @"<div class='ab_tab2_img'><div>
            //                 <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='推荐书'><img src = '" + pic + @"'></a>
            //                  </div><p style='text-align: center; margin-bottom: 2px;'>推荐书</p><p style='text-align: center;'></p></div>";
            //}
            pic = dr["BusinessLicensePic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a class='pirobox_gall' title='营业执照'><img src = '" + pic + @"' onclick='showPic(this)'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>营业执照</p><p style='text-align: center;'></p></div>";
            }// href = '" + pic + @"' rel='group'
            pic = dr["SecurityCertificatePic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a class='pirobox_gall' title='安全证书'><img src = '" + pic + @"' onclick='showPic(this)'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>安全证书</p><p style='text-align: center;'>" + dr["SecurityCertificateNo"].ToString() + @"</p></div>";
            }// href = '" + pic + @"' rel='group'
            pic = dr["RepIDPic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a class='pirobox_gall' title='法人身份证'><img src = '" + pic + @"' onclick='showPic(this)'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>法人身份证</p><p style='text-align: center;'></p></div>";
            }// href = '" + pic + @"' rel='group'
            pic = dr["ContactIDPic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a class='pirobox_gall' title='现场负责人身份证'><img src = '" + pic + @"' onclick='showPic(this)'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>现场负责人身份证</p><p style='text-align: center;'></p></div>";
            }// href = '" + pic + @"' rel='group'
            dt = cc.GetZiZhiPics(id);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pic = dt.Rows[i]["PicPath"].ToString();
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                                    <a class='pirobox_gall' title='" + dt.Rows[i]["ZZName"].ToString() + @"'><img src = '" + pic + @"' onclick='showPic(this)'></a>
                                 </div><p style='text-align: center; margin-bottom: 2px;'>" + dt.Rows[i]["ZZName"].ToString() + @"</p>
                                 <p style='text-align: center;'>" + dt.Rows[i]["ZZCode"].ToString() + @"</p>
                            </div>";
            }// href = '" + pic + @"' rel='group'
            ViewBag.CompanyPics = picHtml;


            dt = cc.GetCompanyReferee(id);
            if(dt.Rows.Count>0)
                ViewBag.RefereFile = "<a href='" + dt.Rows[0]["FilePath"].ToString().Replace(rootPath, "/") + "', target='_blank'>" + dt.Rows[0]["FileName"].ToString() + "</a>";

            dt = cc.GetWorkHistory(id);
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr class='white-bg'>");
                sb.Append("<td>" + dt.Rows[i]["ProjectName"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ContractAmount"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["StartDate"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["EndDate"].ToString() + "</td>");
                sb.Append("<td class=’gray-time>" + dt.Rows[i]["DelayStatus"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["SettlementAmount"].ToString() + "</td>");
                sb.Append("<td class='gray-time'><a href='" + dt.Rows[i]["FilePath"].ToString().Replace(rootPath,"/") + "' target='_blank'>" + dt.Rows[i]["TestifyFile"].ToString() + "</a></td>");
                sb.Append("</tr>");
            }
            ViewBag.WorkHistory = sb.ToString();
            return View();
        }

        public string GetMyRecommend(string pageSize, string pageIndex, string cname, string ctype)
        {
            CompanyContext cc = new CompanyContext();
            if (Session["UserName"] == null) { 
                return "no session";
            }
            string userName = Session["UserId"].ToString();
            return cc.GetMyRecommend(userName, pageSize, pageIndex, cname, ctype);
        }

        public string GetMyAudit(string pageSize, string pageIndex, string cname, string ctype)
        {
            CompanyContext cc = new CompanyContext();
            if (Session["UserId"] == null) { 
                return "no session";
            }
            string userId = Session["UserId"].ToString();
            string roleId = Session["RoleId"].ToString();

            return cc.GetMyAudit(userId, pageSize, pageIndex,cname, ctype, roleId);
        }

        /// <summary>
        /// 获取所有公司
        /// </summary>
        /// <param name="ctype">0:名录外；1:名录内</param>
        /// <returns></returns>
        public string GetCompanies(string ctype, string pageSize, string pageIndex, string cname, string cctype, string cy)
        {
            CompanyContext cc = new CompanyContext();

            return cc.GetAllCompanies(ctype, pageSize, pageIndex, cname, cctype, Session["RoleId"].ToString(), cy);
        }

        // GET: Companys/Create
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult Create()
        {
            if (Request["inout"].ToString() == "1")
            {
                ViewBag.inout = "1";
                ViewBag.InActive = " class=active";
                ViewBag.OutActive = "";
            }
            else
            {
                ViewBag.inout = "0";
                ViewBag.InActive = "";
                ViewBag.OutActive = " class=active";
            }
            return View();
        }
        // 名录外审批
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName ="itemC")]
        public ActionResult MyAuditOut()
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("MyAuditOut", Session["RoleId"].ToString());
            return View();
        }

        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult OutCompanys()
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("MyAuditOut", Session["RoleId"].ToString());
            string pid= Request["id"].ToString();
            string sql = "select ProjName from OutCompanyProject where ID="+pid;
            ViewBag.ProjectName = DBHelper.ExecuteScalar(sql);
            ViewBag.pid = pid;
            return View();
        }

        // POST: Companys/Create
        [HttpPost]
        public string CreateCompany()
        {
            CompanyContext cc = new CompanyContext();
            try
            {
                // TODO: Add insert logic here
                Company company = new Company();
                company.Name = Request["cname"].ToString();
                company.BusinessType = Request["btype"].ToString();
                company.Referrer = Request["rname"].ToString();
                company.CreditNo = Request["scno"].ToString();
                company.QualificationLevel = Request["qlevel"].ToString();
                company.SecurityCertificateNo = Request["lno"].ToString();
                company.BusinessScope = Request["bscope"].ToString();
                company.RegisteredCapital = Request["rmoney"].ToString() == ""?"0": Request["rmoney"].ToString();
                company.Type = int.Parse(Request["inout"]==null? "1": Request["inout"].ToString());
                company.AuditStatus= int.Parse(Request["auditstatus"].ToString());
                company.Status = int.Parse(Request["status"].ToString());
                company.OutCompanyProjId = int.Parse(Request["pi"].ToString());
                company.AddYear = int.Parse(Request["cyear"].ToString());
                
                Session["newCid"] = cc.CreateCompany(company, Session["UserId"].ToString());

                if(Request["id"]!=null)
                {
                    cc.DisableCompany(Request["id"].ToString());
                }
                
                return Session["newCid"].ToString();
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        public void AppendCompanyInfo()
        {
            Company company = new Company();
            company.Id = int.Parse(Session["newCid"].ToString());
            company.CorporateRepresentive = Request["rep"] == null ? "" : Request["rep"].ToString();
            company.RepPhone = Request["rtel"] == null ? "" : Request["rtel"].ToString().Trim();
            company.Contact = Request["contact"] == null ? "" : Request["contact"].ToString();
            company.ContactPhone = Request["cphone"] == null ? "" : Request["cphone"].ToString().Trim();
            company.ContactAddress = Request["caddress"] == null ? "" : Request["caddress"].ToString();
            company.ConstructionContent = Request["construction"] == null ? "" : Request["construction"].ToString();
            company.Note = Request["note"] == null ? "" : Request["note"].ToString();
            CompanyContext cc = new CompanyContext();
            cc.AppendCompanyInfo(company);
        }

        public void UploadPic()
        {
            string cid = Session["newCid"].ToString();
            string picbase64;
            try { 
                if (Request["picdata"] == null)
                {
                    return;
                }
            }
            catch
            {
                return;
            }
            picbase64 = Request["picdata"].ToString();
            if (picbase64 == "")
                return;
            picbase64 = picbase64.Substring(picbase64.IndexOf(',') + 1);
            string picName = Request["name"].ToString();
            string cpic = Server.MapPath("/CompanyPics/Company" + cid);
            if (!Directory.Exists(cpic))
            {
                Directory.CreateDirectory(cpic);
            }
            string fullpath = cpic + "/" + picName + ".jpeg";
            ImageBase64.Base64ToImage(picbase64, fullpath);
            string sql = "";
            switch (picName)
            {
                case "refpic":
                    sql = "update Company set ReferreIDPic='"+fullpath+"' where id="+cid;
                    break;
                case "blpic":
                    sql = "update Company set BusinessLicensePic='" + fullpath + "' where id=" + cid;
                    break;
                case "scPic":
                    sql = "update Company set SecurityCertificatePic='" + fullpath + "' where id=" + cid;
                    break;
                case "rsfz":
                    sql = "update Company set RepIDPic='" + fullpath + "' where id=" + cid;
                    break;
                case "ctsfz":
                    sql = "update Company set ContactIDPic='" + fullpath + "' where id=" + cid;
                    break;

            }
            DBHelper.ExecuteNonQuery(sql);
        }
        public void UploadZiZhi()
        {
            string picData = Request["zz"].ToString();
            string[] zzs = picData.Split('|');
            string sql = "update CompanyZiZhiPic set CompanyId=0 where CompanyId=" + Session["newCid"].ToString()+"; ";
            for(int i=0;i< zzs.Length-1;i++)
            {
                sql += "update CompanyZiZhiPic set CompanyId = "+ Session["newCid"].ToString() + " where PicPath like '%"+zzs[i]+"%'; ";
            }
            DBHelper.ExecuteNonQuery(sql);
        }
        public string UploadWorkHistory()
        {
            string data = Request["workhistory"].ToString();
            char[] separators = { '^' };
            string[] items = data.Split(separators,StringSplitOptions.None);
            string sql = "delete WorkHistory where CompanyId="+ Session["newCid"].ToString()+"; ";
            string path = Server.MapPath("/projectFiles/WorkHistory");
            for (int i=0;i<items.Length-1;i++)
            {
                string[] o = items[i].Split('|');
                sql += "insert into WorkHistory values(" + Session["newCid"] + ",N'"+o[0]+ "',N'" + o[1] + "',N'" + o[2] + "',N'" + o[3] + "',N'" + o[4] + "',N'" + o[5] + "',N'" + o[6] + "',N'" + path+"/"+o[7] + "'); ";
            }
            if(sql!="")
                DBHelper.ExecuteNonQuery(sql);
            return Session["newCid"].ToString();
        }

        // GET: Companys/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Companys/Edit/5
        [HttpPost]
        public string EditCompany(int id)
        {
            try
            {
                CompanyContext cc = new CompanyContext();
                // TODO: Add insert logic here
                Company company = new Company();
                company.Id = id;
                company.Name = Request["companyName"].ToString();
                company.BusinessType = Request["businessType"].ToString();
                company.Referrer = Request["referre"].ToString();
                company.CreditNo = Request["creditNo"].ToString();
                company.QualificationLevel = Request["qualificationLevel"].ToString();
                company.SecurityCertificateNo = Request["securityNo"].ToString();
                company.BusinessScope = Request["businessScope"].ToString();
                company.CorporateRepresentive = Request["representive"].ToString();
                company.RepPhone = Request["repPhone"].ToString();
                company.Contact = Request["contact"].ToString();
                company.ContactPhone = Request["contactPhone"].ToString();
                company.ContactAddress = Request["contactAddress"].ToString();
                company.ConstructionContent = Request["construction"].ToString();
                company.Note = Request["note"].ToString();
                //cc.CreateCompany(company);
                return "{\"result\":\"success\"}";
            }
            catch
            {
                return "{\"result\":\"fail\"}";
            }
        }

        // GET: Companys/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Companys/Delete/5
        [HttpPost]
        public string DeleteCompany(string ids)
        {
            try
            {
                CompanyContext cc = new CompanyContext();
                cc.DeleteCompany(ids);
                return "0";
            }
            catch
            {
                return "1";
            }
        }

        public ActionResult UpdateImage()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ImgUpload()
        {
            //const int maxSize = 205000;
            string cId = Request["companyId"].ToString();
            string picName = Request["picName"].ToString();
            string picVName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            string picPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"Pics\\";
            if (!Directory.Exists(picPath))
                Directory.CreateDirectory(picPath);
            string fileTypes = "gif,jpg,jpeg,png,bmp";
            
            var curFile = Request.Files[0];
            string src = @"\Pics\";
            var fileExt = Path.GetExtension(curFile.FileName);
            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return Json(new { success = false, msg = "上传文件格式不正确" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                picVName += fileExt;
                string fullFileName = picPath + "\\" + picVName;
                src += "\\" + picVName;
                try
                {
                    curFile.SaveAs(fullFileName);
                    addCompanyPicMapping(cId, picName, picVName);
                }
                catch(Exception ex)
                {
                    return Json(new { success = false, msg = "上传失败!"+ex.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = true, msg = "上传成功!", src }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public void DeleteImg()
        {
            // picName是时间，精确到毫秒，所以picName为唯一的，可以确定唯一图。
            string picVName = Request["picVName"].ToString();
            string picPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Pics\\" + picVName;
            if (System.IO.File.Exists(picPath))
                System.IO.File.Delete(picPath);
            removeCompanyPicMapping(picVName);
        }

        private bool addCompanyPicMapping(string cId, string picName, string picVName, int status=1)
        {
            string sql = "insert into CompanyPictures(CompanyId, PicName, PicPath, Status) values(" + cId + ", '" + picName + "', '" + picVName + "', " + status + ");";
            int i = DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            if (i > 0)
                return true;
            return false;
        }

        private bool removeCompanyPicMapping(string picVName)
        {
            string sql = "delete CompanyPictures where PicPath = '" + picVName + "'";
            int i = DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            if (i > 0)
                return true;
            return false;
        }

        public bool AddWorkHistory(WorkHistory wh)
        {
            WorkHistoryContext context = new WorkHistoryContext();
            return context.AddWorkHistory(wh);
        }
        
        public void ToggleCompany(string id, string status)
        {
            CompanyContext cc = new CompanyContext();
            cc.ToggleCompany(id, status);
        }

        public string CheckCompanyNameUsed(string cName, string cyear)
        {
            CompanyContext cc = new CompanyContext();
            string i = cc.CheckCompanyNameUsed(cName, cyear);
            if (i != "0")
                //返回“0”该公司名不可用
                return "0";
            else
                //返回“1”该公司名可用
                return "1";
        }

        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult ReApply()
        {
            FillCompanyInfo();
            return View();
        }

        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult EditCompany()
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("Companys", Session["RoleId"].ToString());
            FillCompanyInfo();
            return View();
        }

        private void FillCompanyInfo()
        {
            ViewBag.cid = Request["cid"].ToString();
            string id = Request["cid"].ToString();
            CompanyContext cc = new CompanyContext();
            DataTable dt = cc.GetCompany(int.Parse(id));
            DataRow dr = dt.Rows[0];
            ViewBag.Name = dr["Name"].ToString();
            ViewBag.AuditStatus = dr["AuditStatus"].ToString();
            ViewBag.CreditNo = dr["CreditNo"].ToString();
            ViewBag.CorporateRepresentative = dr["CorporateRepresentative"].ToString();
            ViewBag.RepPhone = dr["RepPhone"].ToString();
            ViewBag.RegisteredCapital = dr["RegisteredCapital"].ToString();
            ViewBag.BusinessScope = dr["BusinessScope"].ToString();
            ViewBag.Contact = dr["Contact"].ToString();
            ViewBag.ContactPhone = dr["ContactPhone"].ToString();
            ViewBag.BusinessType = dr["BusinessType"].ToString();
            ViewBag.ContactAddress = dr["ContactAddress"].ToString();
            ViewBag.QualificationLevel = dr["QualificationLevel"].ToString();
            ViewBag.ConstructionContent = dr["ConstructionContent"].ToString();
            ViewBag.SecurityCertificateNo = dr["SecurityCertificateNo"].ToString();

            ViewBag.Note = dr["Note"].ToString();
            ViewBag.Referre = dr["Referre"].ToString();
            ViewBag.apid = dr["Type"].ToString() == "1" ? "5" : "1";
            ViewBag.inout = dr["Type"].ToString();
            string projid = dr["OutCompanyProjId"].ToString();
            ViewBag.projId = projid == "" ? "0" : projid;
            Session["inout"]= dr["Type"].ToString();

            ViewBag.roleid = Session["RoleId"].ToString();
            ViewBag.AddYear = dr["AddYear"].ToString();
            // 上传图片和文件
            DataTable pics = cc.GetCompanyPics(id);
            string rootPath = Server.MapPath("../");
            string picHtml = "";
            string ReferreIDPic = dr["ReferreIDPic"].ToString();
            ReferreIDPic = ReferreIDPic.Substring(ReferreIDPic.LastIndexOf('/') + 1);
            string BusinessLicensePic = dr["BusinessLicensePic"].ToString();
            BusinessLicensePic = BusinessLicensePic.Substring(BusinessLicensePic.LastIndexOf('/') + 1);
            string SecurityCertificatePic = dr["SecurityCertificatePic"].ToString();
            SecurityCertificatePic = SecurityCertificatePic.Substring(SecurityCertificatePic.LastIndexOf('/') + 1);
            string RepIDPic = dr["RepIDPic"].ToString();
            RepIDPic = RepIDPic.Substring(RepIDPic.LastIndexOf('/') + 1);
            string ContactIDPic = dr["ContactIDPic"].ToString();
            ContactIDPic = ContactIDPic.Substring(ContactIDPic.LastIndexOf('/') + 1);
            for (int i = 0; i < pics.Rows.Count; i++)
            {
                if (pics.Rows[i]["FilePath"].ToString().IndexOf(ReferreIDPic) > -1&& ReferreIDPic != "")
                {
                    ViewBag.ReferreIDPic = ReferreIDPic;
                    ViewBag.dw1 = pics.Rows[i]["FileName"].ToString();
                }
                else if(ReferreIDPic == "")
                {
                    ViewBag.ReferreIDPic = "";
                    ViewBag.dw1 = "";
                }
                if (pics.Rows[i]["FilePath"].ToString().IndexOf(BusinessLicensePic) > -1&& BusinessLicensePic!="")
                {
                    ViewBag.BusinessLicensePic = BusinessLicensePic;
                    ViewBag.dw2 = pics.Rows[i]["FileName"].ToString();
                }
                if (pics.Rows[i]["FilePath"].ToString().IndexOf(SecurityCertificatePic) > -1&& SecurityCertificatePic!="")
                {
                    ViewBag.SecurityCertificatePic = SecurityCertificatePic;
                    ViewBag.dw3 = pics.Rows[i]["FileName"].ToString();
                }
                else if(SecurityCertificatePic == "")
                {
                    ViewBag.SecurityCertificatePic = "";
                    ViewBag.dw3 = "";
                }
                if (pics.Rows[i]["FilePath"].ToString().IndexOf(RepIDPic) > -1&& RepIDPic!="")
                {
                    ViewBag.RepIDPic = RepIDPic;
                    ViewBag.dw4 = pics.Rows[i]["FileName"].ToString();
                }
                if (pics.Rows[i]["FilePath"].ToString().IndexOf(ContactIDPic) > -1&& ContactIDPic!="")
                {
                    ViewBag.ContactIDPic = ContactIDPic;
                    ViewBag.dw5 = pics.Rows[i]["FileName"].ToString();
                }
            }

            // 资质图片
            string pic = "";
            dt = cc.GetZiZhiPics(int.Parse(id));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pic = dt.Rows[i]["PicPath"].ToString();
                pic = pic.Replace(rootPath, "/");
                string guid = pic.Substring(pic.LastIndexOf('/') + 1);
                picHtml += "<li style='text-align:center'><div><span class='aui-up-span'></span>" +
                           "<img class='aui-close-up-img' src='../img/close.png'><img src='" + pic + "'></div>" +
                           "<label id='zzmc'>" + dt.Rows[i]["ZZName"].ToString() + "</label><br><label id='zsbh'>" + dt.Rows[i]["ZZCode"].ToString() + "</label><label style='display: none'>" + guid + "</label></li>";
            }
            ViewBag.ZiZhiPics = picHtml;

            dt = cc.GetWorkHistory(int.Parse(id));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr class='white-bg' id='row" + i + "'>");
                sb.Append("<td>" + dt.Rows[i]["ProjectName"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ContractAmount"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["StartDate"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["EndDate"].ToString() + "</td>");
                sb.Append("<td class=’gray-time>" + dt.Rows[i]["DelayStatus"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["SettlementAmount"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["TestifyFile"].ToString() + "</td>");
                sb.Append("<td class='gray-time'><div class='color01' style='cursor: pointer;' onclick='removeRowObj(row" + i + ")'>删除</div></td>");
                sb.Append("<td class='gray-time'><a href='" + dt.Rows[i]["FilePath"].ToString().Replace(rootPath, "/") + "' target='_blank'>" + dt.Rows[i]["FilePath"].ToString().Substring(dt.Rows[i]["FilePath"].ToString().LastIndexOf('/') + 1) + "</a></td>");

                sb.Append("</tr>");
            }
            ViewBag.WorkHistory = sb.ToString();
            ViewBag.WorkHistoryCount = dt.Rows.Count;
            dt = cc.GetCompanyReferee(int.Parse(id));
            if (dt.Rows.Count > 0)
                ViewBag.RefereFile = "<a href='" + dt.Rows[0]["FilePath"].ToString().Replace(rootPath, "/") + "', target='_blank'>" + dt.Rows[0]["FileName"].ToString() + "</a>";
            else
                ViewBag.RefereFile = "";
        }
        
        public string SaveCompany()
        {
            CompanyContext cc = new CompanyContext();
            try
            {
                // TODO: Add insert logic here
                Company company = new Company();
                company.Id = int.Parse(Request["id"].ToString());
                company.Name = Request["cname"].ToString();
                company.BusinessType = Request["btype"].ToString();
                company.Referrer = Request["rname"].ToString();
                company.CreditNo = Request["scno"].ToString();
                company.QualificationLevel = Request["qlevel"].ToString();
                company.SecurityCertificateNo = Request["lno"].ToString();
                company.BusinessScope = Request["bscope"].ToString();
                company.RegisteredCapital = Request["rmoney"].ToString() == "" ? "0" : Request["rmoney"].ToString();
                company.Type = int.Parse(Request["inout"] == null ? "1" : Request["inout"].ToString());
                company.AuditStatus = int.Parse(Request["auditstatus"].ToString());
                company.Status = int.Parse(Request["status"].ToString());
                company.OutCompanyProjId = int.Parse(Request["pi"].ToString()==""?"0": Request["pi"].ToString());
                company.AddYear = int.Parse(Request["cyear"].ToString());

                cc.UpdateCompany(company, Session["UserId"].ToString());
                Session["newCid"] = company.Id;
                return company.Id.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public void SaveCompanyPics()
        {
            Company company = new Company();
            company.Id = int.Parse(Request["id"].ToString());
            company.BusinessLicensePic = Request["blpic"].ToString();
            company.ContactIDPic = Request["ctsfz"].ToString();
            company.ReferreIDPic = Request["refd"].ToString();
            company.RepIDPic = Request["rsfz"].ToString();
            company.SecurityCertificatePic = Request["scPic"].ToString();
            CompanyContext cc = new CompanyContext();
            cc.UpdateCompanyPics(company);
        }

        public string CheckUpdateCompanyNameUsed(string cName, string cid, string cyear)
        {

            CompanyContext cc = new CompanyContext();
            string i = cc.CheckUpdateCompanyNameUsed(cName, cid, cyear);
            if (i != "0")
                //返回“0”该公司名不可用
                return "0";
            else
                //返回“1”该公司名可用
                return "1";
        }

        public void SubmitCompany(string cid, string ctype)
        {
            CompanyContext cc = new CompanyContext();
            cc.SubmitCompany(cid, ctype, Session["UserId"].ToString());
        }

        public string ExportCompanysExcel(string cids)
        {
            string where = "";
            if (cids == "0")
            {
                where = " where c.Status=1 ";
            }
            else
            {
                where = " where c.ID in ("+cids+") ";
            }
            string sql = @"select c.Name, c.QualificationLevel, c.RegisteredCapital, bt.Name as BusinessType, c.CorporateRepresentative, c.Contact, c.RepPhone, c.Status 
                            from Company c
                            left join CompanyType bt on c.BusinessType=bt.ID"+where;
            DataTable dt = DBHelper.GetDataTable(sql);
            string tempPath = Server.MapPath("/");
            try
            {
                string file = ExcelOperator.ExportCompany(dt, tempPath);
                return file.Replace(tempPath, "/");
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        public void DeleteCompanyApplication(string cid)
        {
            string sql = "delete Company where ID="+cid+"; delete AppProcessing where ObjId="+cid+" and AppProcId in (1,5)";
            DBHelper.ExecuteNonQuery(sql);
        }

        public void AddProject(string pn, string pt)
        {
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@uid", Session["UserId"].ToString());
            string dpid = DBHelper.ExecuteSP("findProjectDepartment", paras).Tables[0].Rows[0][0].ToString();


            string sql = "insert into OutCompanyProject values(N'"+pn+"',"+pt+", "+Session["UserId"].ToString()+","+dpid+ ", GETDATE());";
            DBHelper.ExecuteNonQuery(sql);
        }

        public string GetProjs()
        {
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@uid", Session["UserId"].ToString());
            try
            {
                string dpid = DBHelper.ExecuteSP("findProjectDepartment", paras).Tables[0].Rows[0][0].ToString();

                string sql = "select ID, ProjName as name from OutCompanyProject where PublisherDept=" + dpid + " order by ID desc";
                DataTable dt = DBHelper.GetDataTable(sql);
                return JsonHelper.DataTableToJSON(dt);
            }
            catch
            {
                return "";
            }
        }

        public string GetPtojects(string pageSize, string pageIndex, string cname, string ptype)
        {
            CompanyContext cc = new CompanyContext();
            if (Session["UserId"] == null)
            {
                return "no session";
            }
            string userId = Session["UserId"].ToString();
            string roleId = Session["RoleId"].ToString();
            return cc.GetPtojects(pageSize, pageIndex, cname, ptype, userId, roleId);
        }

        public string GetProjCompanys(string pid)
        {
            string sql = @"select c.id,c.Type,c.Name, c.QualificationLevel, c.RegisteredCapital, bt.name as BusinessType, c.CorporateRepresentative,  
	                        c.Contact, convert(varchar(20), c.AuditDate,23) as AuditDate, c.AuditStatus, case when c.AuditStatus=2 then 2 else isnull(a.Approved, 5) end as Approved
                        from Company c 
                        left join(
                            select distinct a.ObjId, a.Approved 
                            from vw_AppPLevel a 
                            inner join (select MAX(level) as level,AppProcId, ObjId 
			                            from vw_AppPLevel where AppProcId=1 and Approved=1 group by ObjId, AppProcId
                            ) b on a.AppProcId=b.AppProcId and a.Level>=b.level and a.ObjId=b.ObjId
                            where a.UserId=" + Session["UserId"].ToString() + @"
                        ) a on c.ID=a.ObjId
                        left join CompanyType bt on bt.id=c.BusinessType
                        where c.AuditStatus<>0 and c.OutCompanyProjId=" + pid;
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.DataTableToJSON(dt).Replace("	", "");
        }
        public string GetMYears()
        {
            string sql = "select * from MYear order by addyear desc";
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.DataTableToJSON(dt);
        }
    }
}
