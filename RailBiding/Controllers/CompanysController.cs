using System;
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

namespace RailBiding.Controllers
{
    public class CompanysController : Controller
    {
        // GET: Companys
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName= "itemC")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Companys/Details/5
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult Details(int id)
        {
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

            string rootPath = Server.MapPath("../");
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
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='营业执照'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>营业执照</p><p style='text-align: center;'></p></div>";
            }
            pic = dr["SecurityCertificatePic"].ToString();
            if (pic != "")
            {
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                             <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='安全证书'><img src = '" + pic + @"'></a>
                             </div><p style='text-align: center; margin-bottom: 2px;'>安全证书</p><p style='text-align: center;'>" + dr["SecurityCertificateNo"].ToString() + @"<p></div>";
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
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                pic = dt.Rows[i]["PicPath"].ToString();
                pic = pic.Replace(rootPath, "/");
                picHtml += @"<div class='ab_tab2_img'><div>
                                    <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='" + dt.Rows[i]["ZZName"].ToString() + @"'><img src = '" + pic + @"'></a>
                                 </div><p style='text-align:center; margin-bottom: 2px;'>" + dt.Rows[i]["ZZName"].ToString() + @"</p><p style='text-align: center;'>" + dt.Rows[i]["ZZCode"].ToString() + @"</p></div>";
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
                sb.Append("<td class='gray-time'><a href='"+ dt.Rows[i]["FilePath"].ToString().Replace(rootPath, "/") + "' target='_blank'>" + dt.Rows[i]["TestifyFile"].ToString() + "</a></td>");
                sb.Append("</tr>");
            }
            ViewBag.WorkHistory = sb.ToString();
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult OutCompanyDetail(int id)
        {
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
            return View();
        }

        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult CompanysOut()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult MyRecommend()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult RecommendDetail(int id)
        {
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
            Session["outin"] = dr["Type"].ToString();
            string rootPath = Server.MapPath("../");
            string picHtml = "";
            string pic = dr["ReferreIDPic"].ToString();
            ViewBag.moretime = "";
            if (Request["auditstatus"].ToString() == "3") {
                ViewBag.moretime = "<a href = 'javascript:;' class='js-cancle-meet' title='再次申请' onclick='moretime()'><i class='meet-icon icon-cancel'>再次申请</i></a>";
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
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult AuditDetail(int id)
        {
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
                                    <a href = '" + pic + @"' rel='group' class='pirobox_gall' title='" + dt.Rows[i]["ZZName"].ToString() + @"'><img src = '" + pic + @"'></a>
                                 </div><p style='text-align: center; margin-bottom: 2px;'>" + dt.Rows[i]["ZZName"].ToString() + @"</p>
                                 <p style='text-align: center;'>" + dt.Rows[i]["ZZCode"].ToString() + @"</p>
                            </div>";
            }
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

            return cc.GetMyAudit(userId, pageSize, pageIndex,cname, ctype);
        }

        /// <summary>
        /// 获取所有公司
        /// </summary>
        /// <param name="ctype">0:名录外；1:名录内</param>
        /// <returns></returns>
        public string GetCompanies(string ctype, string pageSize, string pageIndex, string cname, string cctype)
        {
            CompanyContext cc = new CompanyContext();

            return cc.GetAllCompanies(ctype, pageSize, pageIndex, cname, cctype, Session["RoleId"].ToString());
        }

        // GET: Companys/Create
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult Create()
        {
            Session["outin"] = Request["outin"].ToString();
            
            if (Request["outin"].ToString() == "1")
            {
                ViewBag.outin = "1";
                ViewBag.InActive = " class=active";
                ViewBag.OutActive = "";
            }
            else
            {
                ViewBag.outin = "0";
                ViewBag.InActive = "";
                ViewBag.OutActive = " class=active";
            }
            return View();
        }
        // POST: Companys/Create
        [HttpPost]
        public string Create(FormCollection collection)
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
                company.RegisteredCapital = Request["rmoney"].ToString()==""?"0": Request["rmoney"].ToString();
                company.CorporateRepresentive = Request["rep"].ToString();
                company.RepPhone = Request["rtel"].ToString();
                company.Contact = Request["contact"].ToString();
                company.ContactPhone = Request["cphone"].ToString();
                company.Status = 1;
                company.ContactAddress = Request["caddress"].ToString();
                company.ConstructionContent = Request["construction"].ToString();
                company.Note = Request["note"].ToString();
                company.Type = int.Parse(Session["outin"].ToString());
                company.AuditStatus= int.Parse(Request["auditstatus"].ToString());

                company.BusinessLicensePic = Request["blpic"].ToString();
                company.ContactIDPic = Request["ctsfz"].ToString();
                company.ReferreIDPic = Request["refd"].ToString();
                company.RepIDPic = Request["rsfz"].ToString();
                company.SecurityCertificatePic = Request["scPic"].ToString();

                Session["newCid"] = cc.CreateCompany(company, Session["UserId"].ToString());
                
                return "1";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
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
            string sql = "delete CompanyZiZhiPic where CompanyId="+ Session["newCid"].ToString()+"; ";
            for(int i=0;i< zzs.Length-1;i++)
            {
                sql += "update CompanyZiZhiPic set CompanyId = "+ Session["newCid"].ToString() + " where PicPath like '%"+zzs[i]+"%'; ";
            }
            DBHelper.ExecuteNonQuery(sql);
        }
        public void UploadWorkHistory()
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
        public string DeleteCompany(int id, FormCollection collection)
        {
            try
            {
                CompanyContext cc = new CompanyContext();
                // TODO: Add insert logic here
                Company company = new Company();
                cc.DeleteCompany(company);
                return "{\"result\":\"success\"}";
            }
            catch
            {
                return "{\"result\":\"fail\"}";
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

        //[HttpPost]
        //public void FileUpload()
        //{
        //    string path = Server.MapPath("/projectPics/");
        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);
        //    var curFile = Request.Files[0];
        //    var fileExt = Path.GetExtension(curFile.FileName);
        //    string fullPath = path + "/" + curFile.FileName;
        //    curFile.SaveAs(fullPath);
        //}
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

        public string CheckCompanyNameUsed(string cName)
        {
            CompanyContext cc = new CompanyContext();
            string i = cc.CheckCompanyNameUsed(cName);
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
                    ViewBag.ReferreIDPic = @"<input type='text' id='referPic' name='foreRef' width='20' value='" + ReferreIDPic + "' hidden />";
                    ViewBag.dw1 = "<div class='dw1' style='padding-left:50px;text-align:left'>" + pics.Rows[i]["FileName"].ToString() + "</div>";
                }
                else if(ReferreIDPic == "")
                {
                    ViewBag.ReferreIDPic = @"<input type='text' id='referPic' name='foreRef' width='20' value='' hidden />";
                    ViewBag.dw1 = "<div class='dw1' style='padding-left:50px;text-align:left'></div>";
                }
                if (pics.Rows[i]["FilePath"].ToString().IndexOf(BusinessLicensePic) > -1&& BusinessLicensePic!="")
                {
                    ViewBag.BusinessLicensePic = @"<input id='busiLicPic' type='text' name='foreRef' value='" + BusinessLicensePic + "' hidden>";
                    ViewBag.dw2 = "<div class='dw2' style='padding-left:50px;text-align:left'>" + pics.Rows[i]["FileName"].ToString() + "</div>";
                }
                if (pics.Rows[i]["FilePath"].ToString().IndexOf(SecurityCertificatePic) > -1&& SecurityCertificatePic!="")
                {
                    ViewBag.SecurityCertificatePic = @"<input id='scPic' type='text' name='foreRef' value='" + SecurityCertificatePic + "' hidden>";
                    ViewBag.dw3 = "<div class='dw3' style='padding-left:50px;text-align:left'>" + pics.Rows[i]["FileName"].ToString() + "</div>";
                }
                else if(SecurityCertificatePic == "")
                {
                    ViewBag.SecurityCertificatePic = @"<input id='scPic' type='text' name='foreRef' value='' hidden>";
                    ViewBag.dw3 = "<div class='dw3' style='padding-left:50px;text-align:left'></div>";
                }
                if (pics.Rows[i]["FilePath"].ToString().IndexOf(RepIDPic) > -1&& RepIDPic!="")
                {
                    ViewBag.RepIDPic = @"<input id='repSFZ' type='text' name='foreRef' value='" + RepIDPic + "' hidden>";
                    ViewBag.dw4 = "<div class='dw4' style='padding-left:50px;text-align:left'>" + pics.Rows[i]["FileName"].ToString() + "</div>";
                }
                if (pics.Rows[i]["FilePath"].ToString().IndexOf(ContactIDPic) > -1&& ContactIDPic!="")
                {
                    ViewBag.ContactIDPic = @"<input id='conSFZ' type='text' name='foreRef' value='" + ContactIDPic + "' hidden>";
                    ViewBag.dw5 = "<div class='dw5' style='padding-left:50px;text-align:left'>" + pics.Rows[i]["FileName"].ToString() + "</div>";
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
                sb.Append("<td class='gray-time'><div class='color01' style='cursor: pointer;' onclick='removeRow(row" + i + ")'>删除</div></td>");
                sb.Append("<td class='gray-time'><a href='" + dt.Rows[i]["FilePath"].ToString().Replace(rootPath, "/") + "' target='_blank'>" + dt.Rows[i]["FilePath"].ToString().Substring(dt.Rows[i]["FilePath"].ToString().LastIndexOf('/') + 1) + "</a></td>");

                sb.Append("</tr>");
            }
            ViewBag.WorkHistory = sb.ToString();

            dt = cc.GetCompanyReferee(int.Parse(id));
            if (dt.Rows.Count > 0)
                ViewBag.RefereFile = "<a href='" + dt.Rows[0]["FilePath"].ToString().Replace(rootPath, "/") + "', target='_blank'>" + dt.Rows[0]["FileName"].ToString() + "</a>";
            else
                ViewBag.RefereFile = "";
        }

        public string UpdateCompany()
        {
            CompanyContext cc = new CompanyContext();
            try
            {
                // TODO: Add insert logic here
                Company company = new Company();
                company.Name = Request["cname"].ToString();
                company.BusinessType = Request["btype"].ToString();
                company.Referrer = Request["rname"].ToString();
                //string referPic = Request["rpic"].ToString();
                //string blpic = Request["blpic"].ToString();
                company.CreditNo = Request["scno"].ToString();
                company.QualificationLevel = Request["qlevel"].ToString();
                //string scpic = Request["scpic"].ToString();
                company.SecurityCertificateNo = Request["lno"].ToString();
                company.BusinessScope = Request["bscope"].ToString();
                company.RegisteredCapital = Request["rmoney"].ToString() == "" ? "0" : Request["rmoney"].ToString();
                company.CorporateRepresentive = Request["rep"].ToString();
                company.RepPhone = Request["rtel"].ToString();
                //string rsfz = Request["rsfz"].ToString();
                company.Contact = Request["contact"].ToString();
                company.ContactPhone = Request["cphone"].ToString();
                //string csfz = Request["csfz"].ToString();
                company.Status = 1;
                company.ContactAddress = Request["caddress"].ToString();
                company.ConstructionContent = Request["construction"].ToString();
                company.Note = Request["note"].ToString();
                company.Type = int.Parse(Session["outin"].ToString());
                company.AuditStatus = int.Parse(Request["auditstatus"].ToString());
                string refguid = Request["refd"].ToString();
                cc.UpdateCompany(company);

                return "1";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
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
                company.CorporateRepresentive = Request["rep"].ToString();
                company.RepPhone = Request["rtel"].ToString();
                company.Contact = Request["contact"].ToString();
                company.ContactPhone = Request["cphone"].ToString();
                company.Status = 1;
                company.ContactAddress = Request["caddress"].ToString();
                company.ConstructionContent = Request["construction"].ToString();
                company.Note = Request["note"].ToString();
                company.Type = int.Parse(Session["outin"].ToString());
                company.AuditStatus = int.Parse(Request["auditstatus"].ToString());

                company.BusinessLicensePic = Request["blpic"].ToString();
                company.ContactIDPic = Request["ctsfz"].ToString();
                company.ReferreIDPic = Request["refd"].ToString();
                company.RepIDPic = Request["rsfz"].ToString();
                company.SecurityCertificatePic = Request["scPic"].ToString();

                cc.UpdateCompany(company);
                Session["newCid"] = company.Id;
                return "1";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string CheckUpdateCompanyNameUsed(string cName, string cid)
        {

            CompanyContext cc = new CompanyContext();
            string i = cc.CheckUpdateCompanyNameUsed(cName, cid);
            if (i != "0")
                //返回“0”该公司名不可用
                return "0";
            else
                //返回“1”该公司名可用
                return "1";
        }
    }
}
