using DAL.Models;
using DAL.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RailBiding.Common;
using System.Data;
using System.Text;
using System.Web.Mvc;

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
        public string GetOrganizationUser(string appPid)
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetOrganizationUser(appPid);
            return JsonHelper.DataTableToJSON(dt);
        }
        
        public string AddDepartment(string pid, string dname, string dl, string projectdp)
        {
            SystemSetup ss = new SystemSetup();
            return ss.AddDepartment(pid, dname, dl,projectdp);
        }
        public void UpdateDepartment(string id, string name, string level, string projectdp)
        {
            SystemSetup ss = new SystemSetup();
            ss.UpdateDepartment(id, name, level, projectdp);
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
        public string SaveApproveProcess(string appP, string apid)
        {
            JArray ja = (JArray)JsonConvert.DeserializeObject(appP);
            StringBuilder sb = new StringBuilder();
            sb.Append("delete APDetail where APID="+apid+"; ");
            foreach(var o in ja)
            {
                sb.Append("insert into APDetail values(" + apid + ", '"+o["duguid"].ToString()+"', "+ o["rootid"].ToString() + ", 1, "+o["pdid"].ToString()+"); ");
            }
            DBHelper.ExecuteNonQuery(sb.ToString());
            return "1";
        }
        public string GetPersenalInfo()
        {
            string uid = Session["UserId"].ToString();
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetPersenalInfo(uid);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string UpdateUserInfo()
        {
            string uname = Request["uname"].ToString();
            string tel = Request["tel"].ToString();
            string psd;
            if (Request["psd"] != null)
                psd = Request["psd"].ToString();
            else
                psd = "";
            string email = Request["em"].ToString();
            string account = Request["account"].ToString();
            SystemSetup ss = new SystemSetup();
            return ss.UpdateUserInfo(account, uname, psd, tel, email);
        }
        public string CreateUser()
        {
            string acc = Request["acc"].ToString();
            string pasd = Request["pasd"].ToString();
            string nam = Request["nam"].ToString();
            string telephone = Request["telephone"].ToString();
            string em = Request["em"].ToString();
            string did = Request["did"].ToString();
            SystemSetup ss = new SystemSetup();
            return ss.CreateUser(acc, pasd, nam, telephone, em, did);
        }

        public string AddExistUserToDepartment()
        {
            string uid = Request["uid"].ToString();
            string did = Request["did"].ToString();
            SystemSetup ss = new SystemSetup();
            return ss.AddExistUserToDepartment(uid, did);
        }

        public string SearchUsers()
        {
            string uname = "";
            if (Request["uname"] != null)
                uname = Request["uname"].ToString();
            SystemSetup ss = new SystemSetup();
            return ss.SearchUsers(uname);
        }

        public string GetDepartmentUsers()
        {
            string did = Request["did"].ToString();
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetDepartmentUsers(did);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string SearchUseraccount()
        {
            string saccount = Request["saccount"].ToString();
            return "1";
        }
        public string AddBusinessType(string bt)
        {
            SystemSetup ss = new SystemSetup();
            return ss.AddBusinessType(bt);
        }
        public void DeleteBusinessType(string id)
        {
            SystemSetup ss = new SystemSetup();
            ss.DeleteBusinessType(id);
        }
        public void UpdateBusinessType(string id, string name)
        {
            SystemSetup ss = new SystemSetup();
            ss.UpdateBusinessType(id, name);
        }
        public void DeleteUser(string uid)
        {
            SystemSetup ss = new SystemSetup();
            ss.DeleteUser(uid);
        }
        public void AddUserToDepartment(string did, string uid)
        {
            SystemSetup ss = new SystemSetup();
            ss.AddUserToDepartment(did, uid);
        }

        public string GetCompanyBusinessTypes()
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetCompanyBusinessTypes();
            return JsonHelper.DataTableToJSON(dt);
        }
        public void DeleteCompanyBusinessType(string id)
        {
            SystemSetup ss = new SystemSetup();
            ss.DeleteCompanyBusinessType(id);
        }
        public void AddCompanyBusinessType(string bt)
        {
            SystemSetup ss = new SystemSetup();
            ss.AddCompanyBusinessType(bt);
        }
    }
}