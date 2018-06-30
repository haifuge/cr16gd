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

            dt = cc.GetWorkHistory(id);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr class='white-bg'>");
                sb.Append("<td>" + dt.Rows[i]["ProjectName"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ContractAmount"].ToString() + "万</td>");
                sb.Append("<td>" + dt.Rows[i]["StartDate"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["EndDate"].ToString() + "</td>");
                sb.Append("<td class=’gray-time>" + dt.Rows[i]["DelayStatus"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["SettlementAmount"].ToString() + "万</td>");
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

            dt = cc.GetWorkHistory(id);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr class='white-bg'>");
                sb.Append("<td>" + dt.Rows[i]["ProjectName"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ContractAmount"].ToString() + "万</td>");
                sb.Append("<td>" + dt.Rows[i]["StartDate"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["EndDate"].ToString() + "</td>");
                sb.Append("<td class=’gray-time>" + dt.Rows[i]["DelayStatus"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["SettlementAmount"].ToString() + "万</td>");
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

            dt = cc.GetWorkHistory(id);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr class='white-bg'><td>" + dt.Rows[i]["ProjectId"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ProjectName"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ContractAmount"].ToString() + "万</td>");
                sb.Append("<td>" + dt.Rows[i]["StartDate"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["EndDate"].ToString() + "</td>");
                sb.Append("<td class=’gray-time>" + dt.Rows[i]["DelayStatus"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["SettlementAmount"].ToString() + "万</td>");
                sb.Append("</tr>");
            }
            ViewBag.WorkHistory = sb.ToString();
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

            dt = cc.GetWorkHistory(id);
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr class='white-bg'><td>"+dt.Rows[i]["ProjectId"].ToString()+"</td>");
                sb.Append("<td>" + dt.Rows[i]["ProjectName"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["ContractAmount"].ToString() + "万</td>");
                sb.Append("<td>" + dt.Rows[i]["StartDate"].ToString() + "</td>");
                sb.Append("<td>" + dt.Rows[i]["EndDate"].ToString() + "</td>");
                sb.Append("<td class=’gray-time>" + dt.Rows[i]["DelayStatus"].ToString() + "</td>");
                sb.Append("<td class='gray-time'>" + dt.Rows[i]["SettlementAmount"].ToString() + "万</td>");
                sb.Append("</tr>");
            }
            ViewBag.WorkHistory = sb.ToString();
            return View();
        }

        public string GetMyRecommend()
        {
            CompanyContext cc = new CompanyContext();
            if (Session["UserName"] == null) { 
                return "no session";
            }
            string userName = Session["UserName"].ToString();
            DataTable companies = cc.GetMyRecommend(userName);
            string json = JsonHelper.DataTableToJSON(companies);
            return json;
        }

        public string GetMyAudit()
        {
            CompanyContext cc = new CompanyContext();
            if (Session["UserId"] == null) { 
                return "no session";
            }
            string userId = Session["UserId"].ToString();
            DataTable companies = cc.GetMyAudit(userId);
            string json = JsonHelper.DataTableToJSON(companies);
            return json;
        }

        /// <summary>
        /// 获取所有公司
        /// </summary>
        /// <param name="ctype">0:名录外；1:名录内</param>
        /// <returns></returns>
        public string GetCompanies(string ctype)
        {
            CompanyContext cc = new CompanyContext();
            DataTable companies = cc.GetAllCompanies(ctype);
            string json = JsonHelper.DataTableToJSON(companies);
            return json;
        }

        // GET: Companys/Create
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemC")]
        public ActionResult Create()
        {
            Session["outin"] = Request["outin"].ToString();
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
                //string referPic = Request["rpic"].ToString();
                //string blpic = Request["blpic"].ToString();
                company.CreditNo = Request["scno"].ToString();
                company.QualificationLevel = Request["qlevel"].ToString();
                //string scpic = Request["scpic"].ToString();
                company.SecurityCertificateNo = Request["lno"].ToString();
                company.BusinessScope = Request["bscope"].ToString();
                company.RegisteredCapital = Request["rmoney"].ToString();
                company.CorporateRepresentive = Request["rep"].ToString();
                company.RepPhone = Request["rtel"].ToString();
                //string rsfz = Request["rsfz"].ToString();
                company.Contact = Request["contact"].ToString();
                company.ContactPhone = Request["cphone"].ToString();
                //string csfz = Request["csfz"].ToString();
                company.ContactAddress = Request["caddress"].ToString();
                company.ConstructionContent = Request["construction"].ToString();
                company.Note = Request["note"].ToString();
                company.Type = int.Parse(Session["outin"].ToString());
                Session["newCid"] = cc.CreateCompany(company);
                
                return "1";
            }
            catch(Exception ex)
            {
                return "0";
            }
        }
        public void UploadPic()
        {
            string cid = Session["newCid"].ToString();
            string picbase64 = Request["picdata"].ToString();
            picbase64 = picbase64.Substring(picbase64.IndexOf(',') + 1);
            string picName = Request["name"].ToString();
            string cpic = Server.MapPath("/CompanyPics/Company" + Session["newCid"].ToString());
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

        public void UploadWorkHistory()
        {
            string data = Request["workhistory"].ToString();
            string[] items = data.Split('^');
            string sql = "";
            for(int i=0;i<items.Length-1;i++)
            {
                string[] o = items[i].Split('|');
                sql += "insert into WorkHistory values(" + Session["newCid"] + ",N'"+o[0]+ "',N'" + o[1] + "',N'" + o[2] + "',N'" + o[3] + "',N'" + o[4] + "',N'" + o[5] + "'); ";
            }
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
                cc.CreateCompany(company);
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

        [HttpPost]
        public void FileUpload()
        {
            string path = Server.MapPath("/projects/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var curFile = Request.Files[0];
            var fileExt = Path.GetExtension(curFile.FileName);
            string fullPath = path + "/" + curFile.FileName;
            curFile.SaveAs(fullPath);
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
    }
}
