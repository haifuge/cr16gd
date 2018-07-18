using DAL.Models;
using DAL.Tools;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RailBiding.Common;

namespace RailBiding.Controllers
{
    public class BidingsController : Controller
    {
        // GET: Bidings
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemB")]
        public ActionResult Index()
        {
            return View();
        }
        public string GetBidings()
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetAllBids();
            return JsonHelper.DataTableToJSON(dt);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemB")]
        public ActionResult BidDetail(string pid)
        {
            if (pid == null)
                return View("Login");
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidCompany(pid);
            DataRow dr = dt.Rows[0];
            ViewBag.Name = dr["Name"].ToString();
            ViewBag.Location = dr["Location"].ToString();
            ViewBag.Content = dr["Content"].ToString();
            ViewBag.Type = dr["ProjType"].ToString();
            ViewBag.Publisher = dr["Publisher"].ToString();
            ViewBag.BidingNum = dr["BidingNum"].ToString();
            ViewBag.ApplyDate = dr["ApplyDate"].ToString();
            ViewBag.OpenDate = dr["OpenDate"].ToString();
            ViewBag.ProjDescription = dr["Content"].ToString();

            dt = bc.GetBidingCompanys(pid);
            
            var joinC = (from c in dt.AsEnumerable()
                        where c.Field<int>("CompanyResponse")==1
                        select new { name = c["CompanyName"].ToString() }).ToList();
            var noJoinC= (from c in dt.AsEnumerable()
                         where c.Field<int>("CompanyResponse") == 2
                         select new { name = c["CompanyName"].ToString() }).ToList();
            var noResponseC = (from c in dt.AsEnumerable()
                          where c.Field<int>("CompanyResponse") == 0
                          select new { name = c["Name"].ToString() }).ToList();
            ViewBag.joinNum = joinC.Count;
            ViewBag.noJoinNum = noJoinC.Count;
            ViewBag.noResponseNum = noResponseC.Count;
            StringBuilder cHtml = new StringBuilder();
            foreach (var c in joinC)
            {
                cHtml.Append("<span>"+c.name+"</span>");
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
            return View();
        }

        public string GetBidCompanies(string bid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidCompany(bid);
            return JsonHelper.DataTableToJSON(dt);
        }

        // GET: Bidings/Details/5
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemB")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Bidings/Create
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemB")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bidings/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Bidings/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Bidings/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Bidings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Bidings/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public JsonResult DocUpload()
        {
            string docName = Request["docName"].ToString();
            string docVName = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            string docPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "BidDocs\\";
            if (!Directory.Exists(docPath))
                Directory.CreateDirectory(docPath);
            string fileTypes = "pdf, docx, doc";

            var curFile = Request.Files[0];
            string src = @"\Pics\";
            var fileExt = Path.GetExtension(curFile.FileName);
            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return Json(new { success = false, msg = "上传文件格式不正确" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                docVName += fileExt;
                string fullFileName = docPath + "\\" + docVName;
                src += "\\" + docVName;
                try
                {
                    curFile.SaveAs(fullFileName);
                    addCompanyDocMapping(docName, docVName);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, msg = "上传失败!" + ex.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = true, msg = "上传成功!", src }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteImg()
        {
            // picName是时间，精确到毫秒，所以picName为唯一的，可以确定唯一图。
            string docVName = Request["docVName"].ToString();
            string picPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Docs\\" + docVName;
            if (System.IO.File.Exists(picPath))
                System.IO.File.Delete(picPath);
            removeBidDocMapping(docVName);
        }

        private bool addCompanyDocMapping(string docName, string docVName)
        {
            string sql = "insert into BidDocument(BidId, DocName, DocVName) values(0, '" + docName + "', '" + docVName + "');";
            int i = DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            if (i > 0)
                return true;
            return false;
        }

        private bool removeBidDocMapping(string docVName)
        {
            string sql = "delete BidDocument where DocVName = '" + docVName + "'";
            int i = DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            if (i > 0)
                return true;
            return false;
        }
    }
}
