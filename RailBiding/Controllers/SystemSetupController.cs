using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Tools;
using RailBiding.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RailBiding.Controllers
{
    public class SystemSetupController : Controller
    {
        // GET: SystemSetup
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult Index()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult PersonalCenter()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult SystemLog()
        {
            return View();
        }
        public string GetLogs()
        {
            LogRecordContext lr = new LogRecordContext();
            DataTable dt = lr.GetLogRecords();
            return JsonHelper.DataTableToJSON(dt);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult CategoryManagement()
        {
            return View();
        }
        public string GetSystemTypes()
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetSystemTypes();
            return JsonHelper.DataTableToJSON(dt);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult SystemUser()
        {
            return View();
        }
        public string GetAdmins()
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetAdmins();
            return JsonHelper.DataTableToJSON(dt);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult ApproveProcessManagement()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult ApproveProcessDetail(string apid)
        {
            ViewBag.apppid = apid;
            switch (apid)
            {
                case "1":
                    ViewBag.ApproveProcessTitle = "公司审批流程配置";
                    break;
                case "2":
                    ViewBag.ApproveProcessTitle = "竞标文件审批流程配置";
                    break;
                case "3":
                    ViewBag.ApproveProcessTitle = "发标审批流程配置";
                    break;
                case "4":
                    ViewBag.ApproveProcessTitle = "定标文件审批流程配置";
                    break;
            }
            return View();
        }

        public string GetApproveProcesses()
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetApproveProcesses();
            return JsonHelper.DataTableToJSON(dt);
        }

        public string GetOrganizations()
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetOrganizations();
            return JsonHelper.DataTableToJSON(dt);
        }
        public string GetOrganizationUser()
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetOrganizationUser();
            return JsonHelper.DataTableToJSON(dt);
        }
        
        public string AddDepartment(string pid, string level)
        {
            SystemSetup ss = new SystemSetup();
            return ss.AddDepartment(pid, level);
        }
        public void UpdateDepartment(string id, string name)
        {
            SystemSetup ss = new SystemSetup();
            ss.UpdateDepartment(id, name);
        }
        public void RemoveDepartment(string id)
        {
            SystemSetup ss = new SystemSetup();
            ss.RemoveDepartment(id);
        }
        public string GetApprovePrcess(string appPid)
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetApprovePrcess(appPid);
            return JsonHelper.DataTableToJSON(dt);
        }
        [HttpPost]
        public string SaveApproveProcess(string appP)
        {
            //if (appP.Length < 2)
            //    return "0";

            //string jsonText = appP.Substring(1,appP.Length-2);
            //string[] objs = jsonText.Split('},{');

            //foreach(var o in objs)
            //{
            //    JObject json1 = (JObject)JsonConvert.DeserializeObject(o);
            //}
            
            ////JArray array = (JArray)json1["Rows"];
            ////int i = array.Count;
            ////string aa = "";
            ////foreach (var jObject in array)
            ////{
            ////    //赋值属性
            ////    aa = jObject["id"].ToString();//获取字符串中id值
            ////}
            return "1";
        }
    }
}