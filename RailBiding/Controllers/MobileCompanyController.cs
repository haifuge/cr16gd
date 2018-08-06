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
    public class MobileCompanyController : Controller
    {
        // GET: MobileCompany
        [VerifyMobileLoginFilter]
        public ActionResult Index(int id)
        {
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

            string rootPath = Server.MapPath("/");
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
            if (dt.Rows.Count > 0)
                ViewBag.RefereFile = "<a href='" + dt.Rows[0]["FilePath"].ToString().Replace(rootPath, "/") + "', target='_blank'>" + dt.Rows[0]["FileName"].ToString() + "</a>";

            dt = cc.GetWorkHistory(id);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr class='white-bg'>");
                sb.Append("<td>" + dt.Rows[i]["ProjectName"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ContractAmount"].ToString() + "</td>");
                sb.Append("<td class=’gray-time>" + dt.Rows[i]["DelayStatus"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["SettlementAmount"].ToString() + "</td>");
                sb.Append("<td class='gray-time'><a href='" + dt.Rows[i]["FilePath"].ToString().Replace(rootPath, "/") + "' target='_blank'>" + dt.Rows[i]["TestifyFile"].ToString() + "</a></td>");
                sb.Append("</tr>");
            }
            ViewBag.WorkHistory = sb.ToString();
            return View();
        }
        [VerifyMobileLoginFilter]
        public ActionResult AuditAction(int id)
        {
            ViewBag.UserId = Session["UserId"].ToString();
            CompanyContext cc = new CompanyContext();
            DataTable dt = cc.GetCompany(id);
            DataRow dr = dt.Rows[0];
            ViewBag.cid = id;
            ViewBag.apid = dr["Type"].ToString() == "1" ? "5" : "1";
            return View();
        }
    }
}