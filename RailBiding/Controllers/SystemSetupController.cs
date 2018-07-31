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
            if(Session["RoleId"].ToString()!="2")
            {
                ViewBag.SecondMenu = @"<li><a href='SystemSetup/PersonalCenter' class='active'><i class='icon icon-hand2'></i>个人中心</a></li>";
                return View("PersonalCenter");
            }
            else
            {
                return View();
            }
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult PersonalCenter()
        {
            if (Session["RoleId"].ToString() != "2")
            {
                ViewBag.SecondMenu = @"<li><a href='/SystemSetup/PersonalCenter' class='active'><i class='icon icon-hand2'></i>个人中心</a></li>";
            }
            else
            {
                ViewBag.SecondMenu = @"<li><a href='/SystemSetup'><i class='icon icon-eye1'></i>组织结构</a></li>
                                    <li><a href='/SystemSetup/PersonalCenter' class='active'><i class='icon icon-hand2'></i>个人中心</a></li>
                                    <li><a href='/SystemSetup/SystemLog'><i class='icon icon-hand3'></i>系统日志</a></li>
                                    <li><a href='/SystemSetup/CategoryManagement'><i class='icon icon-hand4'></i>类别管理</a></li>
                                    <li><a href='/SystemSetup/SystemUser'><i class='icon icon-hand5'></i>系统用户</a></li>
                                    <li><a href='/SystemSetup/ApproveProcessManagement'><i class='icon icon-hand4'></i>审核流程</a></li>";
            }
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult SystemLog()
        {
            if (Session["RoleId"].ToString() != "2")
            {
                ViewBag.SecondMenu = @"<li><a href='SystemSetup/PersonalCenter' class='active'><i class='icon icon-hand2'></i>个人中心</a></li>";
                return View("PersonalCenter");
            }
            else
            {
                return View();
            }
        }
        public string GetLogs(string pageSize, string pageIndex)
        {
            LogRecordContext lr = new LogRecordContext();
            return lr.GetLogRecords(pageSize, pageIndex);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult CategoryManagement()
        {
            if (Session["RoleId"].ToString() != "2")
            {
                ViewBag.SecondMenu = @"<li><a href='SystemSetup/PersonalCenter' class='active'><i class='icon icon-hand2'></i>个人中心</a></li>";
                return View("PersonalCenter");
            }
            else
            {
                return View();
            }
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
            if (Session["RoleId"].ToString() != "2")
            {
                ViewBag.SecondMenu = @"<li><a href='SystemSetup/PersonalCenter' class='active'><i class='icon icon-hand2'></i>个人中心</a></li>";
                return View("PersonalCenter");
            }
            else
            {
                return View();
            }
        }
        public string GetAdmins()
        {
            SystemSetup ss = new SystemSetup();
            string page = Request["PageIndex"].ToString();
            string pagesize= Request["PageSize"].ToString();
            string data = ss.GetAdmins(page,pagesize);
            return data;
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemSS")]
        public ActionResult ApproveProcessManagement()
        {
            if (Session["RoleId"].ToString() != "2")
            {
                ViewBag.SecondMenu = @"<li><a href='SystemSetup/PersonalCenter' class='active'><i class='icon icon-hand2'></i>个人中心</a></li>";
                return View("PersonalCenter");
            }
            else
            {
                return View();
            }
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
            if (Session["RoleId"].ToString() != "2")
            {
                ViewBag.SecondMenu = @"<li><a href='SystemSetup/PersonalCenter' class='active'><i class='icon icon-hand2'></i>个人中心</a></li>";
                return View("/SystemSetup/PersonalCenter");
            }
            else
            {
                return View();
            }
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
            string roleid = Request["roleid"].ToString();
            SystemSetup ss = new SystemSetup();
            return ss.UpdateUserInfo(account, uname, psd, tel, email, roleid);
        }
        public string CreateUser()
        {
            string acc = Request["acc"].ToString();
            string pasd = Request["pasd"].ToString();
            string nam = Request["nam"].ToString();
            string telephone = Request["telephone"].ToString();
            string em = Request["em"].ToString();
            string did = Request["did"].ToString();
            string roleid = Request["roleid"].ToString();
            SystemSetup ss = new SystemSetup();
            return ss.CreateUser(acc, pasd, nam, telephone, em, did, roleid);
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

        public string GetDepartmentUsers(string pageSize, string pageIndex)
        {
            string did = Request["did"].ToString();
            SystemSetup ss = new SystemSetup();
            return ss.GetDepartmentUsers(did, pageSize, pageIndex);
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
        public string AddCompanyBusinessType(string bt)
        {
            SystemSetup ss = new SystemSetup();
            return ss.AddCompanyBusinessType(bt);
        }
        public void UpdateCompanyBusinessType(string id, string name)
        {
            SystemSetup ss = new SystemSetup();
            ss.UpdateCompanyBusinessType(id, name);
        }
    }
}