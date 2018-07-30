using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using System.Data;
using DAL.Tools;
using System.Text;
using Newtonsoft.Json;

namespace RailBiding.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Projects
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemP")]
        public ActionResult Index()
        {
            if (Session["RoleId"].ToString() == "4")
          {
                ViewBag.Create = @"<a class='meet-btn red-btn small-size sm-btn' href='/Projects/Add'><i class='xs-meet-icon icon-add'></i>添加</a>";
           }
           else
              ViewBag.Create = "";
            return View();
        }
        public string GetAllProjects(string pageSize, string pageIndex, string ptype, string status, string pname)
        {
            ProjectContext pc = new ProjectContext();
            return pc.GetAllProjects(pageSize, pageIndex, Session["UserId"].ToString(), ptype, status, pname);
        }

        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemP")]
        public ActionResult Detail(string pid)
        {
            ProjectContext pc = new ProjectContext();
            DataTable dt = pc.GetProject(pid);
            DataRow dr = dt.Rows[0];
            ViewBag.ProjId = pid;
            ViewBag.pName = dr["Name"].ToString();
            ViewBag.pType = dr["ProjType"].ToString();
            ViewBag.pLocation = dr["Location"].ToString();
            ViewBag.pProDescription = dr["ProDescription"].ToString().Replace("\n","<br/>");
            // 未发布；定标文件审核中，定标文件审核通过，招标文件审核中；招标文件审核通过；定标文件审核中；已通过
            string pStatus = dr["Status"].ToString();
            switch (pStatus)
            {
                case "未发布":
                    ViewBag.Button = @"<a href='#' class='js-cancle-meet' title='招标文件申请'>
                                            <i class='meet-icon icon-cancel icon-bh2' onclick='bidFileApply()'>招标文件申请</i>
                                        </a>";
                    break;
                case "招标文件审核中":
                    ViewBag.Button = "";
                    ViewBag.BFitem=getBidFileItem(pid);
                    break;
                case "招标文件被驳回":
                    ViewBag.Button = "";
                    ViewBag.BFitem = getBidFileItem(pid);
                    break;
                case "招标文件审核通过":
                    ViewBag.Button = @"<a href='#' class='js-cancle-meet' title='招标申请'>
                                            <i class='meet-icon icon-cancel icon-bh2' onclick='bidApply()'>招标申请</i>
                                       </a>";
                    ViewBag.BFitem = getBidFileItem(pid);
                    break;
                case "招标审核中":
                    ViewBag.Button = "";
                    ViewBag.BFitem = getBidFileItem(pid);
                    ViewBag.Bitem = getBidItem(pid);
                    break;
                case "招标被驳回":
                    ViewBag.Button = "";
                    ViewBag.BFitem = getBidFileItem(pid);
                    ViewBag.Bitem = getBidItem(pid);
                    break;
                case "招标审核通过":
                    ViewBag.Button = @"<a href='/Projects/MakeBidFile?pid="+pid+ @"&status=1' class='js-cancle-meet' title='定标文件申请'>
                                            <i class='meet-icon icon-cancel icon-bh2'>定标文件申请</i>
                                        </a>";
                    ViewBag.BFitem = getBidFileItem(pid);
                    ViewBag.Bitem = getBidItem(pid);
                    break;
                case "定标文件审核中":
                    ViewBag.Button = "";
                    ViewBag.BFitem = getBidFileItem(pid);
                    ViewBag.Bitem = getBidItem(pid);
                    ViewBag.MBFitem = getMakeBidItem(pid);
                    break;
                case "定标文件被驳回":
                    ViewBag.Button = "";
                    ViewBag.BFitem = getBidFileItem(pid);
                    ViewBag.Bitem = getBidItem(pid);
                    ViewBag.MBFitem = getMakeBidItem(pid);
                    break;
                case "已结束":
                    ViewBag.Button = "";
                    ViewBag.BFitem = getBidFileItem(pid);
                    ViewBag.Bitem = getBidItem(pid);
                    ViewBag.MBFitem = getMakeBidItem(pid);
                    break;
            }
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemP")]
        public ActionResult MakeBidFile(string pid)
        {
            ViewBag.pid = pid;
            string sql = @"select mb.ProjId, mb.Abstract, mb.FileExplain, d.Name+' '+ui.UserName as pName
                            from MakeBidingFile mb 
                            inner join DepartmentUser du on mb.PublisherId=du.UserId and du.MainDeparment=1
                            inner join Department d on d.ID=du.DepartmentId
                            inner join UserInfo ui on ui.ID=du.UserId and 
                            where mb.ProjId=" + pid;
            DataTable dt = DBHelper.GetDataTable(sql);
            if(dt.Rows.Count>0)
            {
                ViewBag.Abstract = dt.Rows[0]["Abstract"].ToString();
                ViewBag.FileExplain = dt.Rows[0]["FileExplain"].ToString();
                ViewBag.pName = dt.Rows[0]["pName"].ToString();
            }
            return View();
        }
        private string getBidFileItem(string pid)
        {
            BidingFileContext bf = new BidingFileContext();
            DataTable dt = bf.getBidingFileDetail(pid);
            if (dt.Rows.Count == 0)
                return "";
            DataRow dr = dt.Rows[0];
            string pdate = dr["PublishDate"].ToString();
            string content = dr["Content"].ToString();
            string status= dr["Status"].ToString();
            string statusClass = "";
            string statusnum = "";
            switch (status)
            {
                case "0":
                    status = "未发布";
                    statusnum = "0";
                    statusClass = "ytg001 ytg002";
                    break;
                case "1":
                    status = "审核中";
                    statusnum = "1";
                    statusClass = "ytg001 ytg004";
                    break;
                case "2":
                    status = "已通过";
                    statusnum = "2";
                    statusClass = "ytg001";
                    break;
                case "3":
                    status = "被驳回";
                    statusnum = "3";
                    statusClass = "ytg001 ytg003";
                    break;
            }
            string files = "";
            dt = bf.GetFiles(pid);
            for (int i = 0; i < dt.Rows.Count; i++) {
                files += "<li><b><img src='../img/icon-file.png'></b>"+dt.Rows[i][0].ToString()+"</li>";
            }
            string result = @"<h3>招标文件 <span>" + pdate + "</span></h3>"+
                                "<div class='a-zbwj' onclick=\"location.href='/Projects/BidFileDetail?pid=" + pid+ "&status="+ statusnum + "'\" sytle='cursor: pointer;'>" +
                                    "<div class='con-01'><p>" + content + @"</p></div>
                                    <div class='con-02'>
                                        <ul>"+files+@"</ul>
                                    </div>
                                    <div class='con-03'><div class='"+ statusClass + "'>"+ status + @"</div></div>
                                </div>";

            return result;
        }
        private string getBidItem(string pid) {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBid(pid);
            if (dt.Rows.Count == 0)
                return "";
            string pdate = dt.Rows[0]["PublishDate"].ToString();
            string odate = dt.Rows[0]["OpenDate"].ToString();
            string adate = dt.Rows[0]["ApplyDate"].ToString();
            string bnum = dt.Rows[0]["BidingNum"].ToString();
            string status = dt.Rows[0]["Status"].ToString();
            string statusClass = "";
            string statusnum = "";
            switch (status)
            {
                case "0":
                    status = "未发布";
                    statusnum = "0";
                    statusClass = "ytg001 ytg002";
                    break;
                case "1":
                    status = "审核中";
                    statusnum = "1";
                    statusClass = "ytg001 ytg004";
                    break;
                case "2":
                    status = "已通过";
                    statusnum = "2";
                    statusClass = "ytg001";
                    break;
                case "3":
                    status = "被驳回";
                    statusnum = "3";
                    statusClass = "ytg001 ytg003";
                    break;
            }
            string files = "";
            dt = bc.GetFiles(pid);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                files += "<li><b><img src='../img/icon-file.png'></b>" + dt.Rows[i][0].ToString() + "</li>";
            }
            string result = "<h3>招标申请 <span>"+ pdate + "</span></h3>"+
                            "<div class='a-zbwj' onclick=\"location.href='/Projects/BidDetail?pid=" + pid + "&status=" + statusnum + "'\" sytle='cursor: pointer;'>" +
                                @"<div class='con-01 con-01-1'>
                                    <p><span class='t-time'>报名时间：</span><span class='time'>" + adate + @"</span>
                                    <span class='t-time'>预计开标时间：</span><span class='time'>" + odate + @"</span>
                                    <span class='t-time'>拟中标单位数量：</span><span class='time'>" + bnum + @"</span></p>
                                </div>
                              
                                <div class='con-03'><div class='" + statusClass + "'>" + status + @"</div></div>
                            </div>";
            return result;
        }
        private string getMakeBidItem(string pid)
        {
            MakeBidFileContext mc = new MakeBidFileContext();
            DataTable dt = mc.GetMakeBidFileDetail(pid);
            if (dt.Rows.Count == 0)
                return "";
            string pdate = dt.Rows[0]["PublishDate"].ToString();
            string abst = dt.Rows[0]["Abstract"].ToString();
            string status = dt.Rows[0]["Status"].ToString();
            string statusClass = "";
            string statusnum = "";
            switch (status)
            {
                case "0":
                    status = "未发布";
                    statusnum = "0";
                    statusClass = "ytg001 ytg002";
                    break;
                case "1":
                    status = "审核中";
                    statusnum = "1";
                    statusClass = "ytg001 ytg004";
                    break;
                case "2":
                    status = "已通过";
                    statusnum = "2";
                    statusClass = "ytg001";
                    break;
                case "3":
                    status = "被驳回";
                    statusnum = "3";
                    statusClass = "ytg001 ytg003";
                    break;
            }
            string files = "";
            dt = mc.GetFiles(pid);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                files += "<li><b><img src='../img/icon-file.png'></b>" + dt.Rows[i][0].ToString() + "</li>";
            }
            string result = @"<h3>定标文件 <span>"+pdate+ "</span></h3>"+
                                "<div class='a-zbwj' onclick=\"location.href='/Projects/MakeBidFileDetail?pid="+pid+ "&status=" + statusnum + "'\" sytle='cursor: pointer;'>" +
                                    "<div class='con-01'><p>" + abst + @"</p></div>
                                    <div class='con-02'><ul>"+ files + @"</ul></div>
                                    <div class='con-03'><div class='"+ statusClass + @"'>"+ status + @"</div></div>
                                </div>";
            return result;
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemP")]
        public ActionResult Add()
        {
            return View();
        }
        public string AddProject()
        {
            Project p = new Project();
            p.Name = Request["name"].ToString();
            p.ProjType = Request["type"].ToString();
            p.Location = Request["location"].ToString();
            p.PublisherId = int.Parse(Session["UserId"].ToString());
            p.ProDescription = Request["description"].ToString();
            ProjectContext pc = new ProjectContext();
            if (pc.AddProject(p))
            {
                Log l = new Log();
                l.OperType = OperateType.Create;
                l.UserId = p.PublisherId.ToString();
                l.Description = "创建项目 - " + p.Name;
                LogContext.WriteLog(l);
                return "1";
            }
            return "0";
        }

        [HttpPost]
        public bool AddBidFile()
        {
            string userid= Session["UserId"].ToString();
            string pid = Request["pid"].ToString();
            string content = Request["content"].ToString();
            string publisherId = userid;
            string status = Request["status"].ToString();
            BidingFileContext bfc = new BidingFileContext();
            if(bfc.AddBidingFile(pid, content, publisherId,status))
                if (status == "1")
                {
                    ProjectContext pc = new ProjectContext();
                    pc.UpdateProjectStatus(pid, "招标文件审核中");
                    pc.CreateApproveProcess(userid, pid, 2);

                    Log l = new Log();
                    l.OperType = OperateType.Create;
                    l.UserId =userid;
                    l.Description = "创建"+DBHelper.ExecuteScalar("select Name from Project where Id = " + pid) +" - 招标文件";
                    LogContext.WriteLog(l);
                }
            return true;
        }
        public bool ReapplyBidFile()
        {
            string userid = Session["UserId"].ToString();
            string pid = Request["pid"].ToString();
            string content = Request["content"].ToString();
            string publisherId = userid;
            string status = Request["status"].ToString();
            BidingFileContext bfc = new BidingFileContext();
            if (bfc.UpdateBidingFile(pid, content))
            {
                ProjectContext pc = new ProjectContext();
                pc.UpdateProjectStatus(pid, "招标文件审核中");
                string sql = "update AppProcessing set Approved=1 where AppProcId=2 and ObjId=" + pid + " and Approved=3; ";
                sql += " update BidingFile set Status = 1 where ProjId = "+pid;
                DBHelper.ExecuteNonQuery(sql);

                Log l = new Log();
                l.OperType = OperateType.Create;
                l.UserId = userid;
                l.Description = "重新申请" + DBHelper.ExecuteScalar("select Name from Project where Id = " + pid) + " - 招标文件";
                LogContext.WriteLog(l);
            }
            return true;
        }
        [HttpPost]
        public bool AddBid()
        {
            string pid = Request["pid"].ToString();
            string userid = Session["UserId"].ToString();
            Bid bid = new Bid();
            bid.ApplyDate = Request["adate"].ToString();
            bid.OpenDate = Request["odate"].ToString();
            bid.BidingNum = Request["bnum"].ToString();
            bid.ProjId = int.Parse(pid);
            bid.Status = Request["status"].ToString();
            bid.PublisherId = int.Parse(userid);
            BidContext bc = new BidContext();
            if(bc.AddBid(bid))
                if (bid.Status == "1")
                {
                    ProjectContext pc = new ProjectContext();
                    pc.UpdateProjectStatus(bid.ProjId.ToString(), "招标审核中");
                    pc.CreateApproveProcess(userid, pid, 3);

                    Log l = new Log();
                    l.OperType = OperateType.Create;
                    l.UserId = userid;
                    l.Description = "创建" + DBHelper.ExecuteScalar("select Name from Project where Id = " + pid) + " - 邀标申请";
                    LogContext.WriteLog(l);
                }
            return true;
        }
        public void ReapplyBid()
        {
            string pid = Request["pid"].ToString();
            string userid = Session["UserId"].ToString();
            Bid bid = new Bid();
            bid.ApplyDate = Request["adate"].ToString();
            bid.OpenDate = Request["odate"].ToString();
            bid.BidingNum = Request["bnum"].ToString();
            bid.ProjId = int.Parse(pid);
            bid.Status = Request["status"].ToString();
            bid.PublisherId = int.Parse(userid);
            BidContext bc = new BidContext();
            if (bc.UpdateBid(bid))
            {
                ProjectContext pc = new ProjectContext();
                pc.UpdateProjectStatus(bid.ProjId.ToString(), "招标审核中");
                string sql = "update AppProcessing set Approved=1 where AppProcId=3 and ObjId=" + pid + " and Approved=3; ";
                sql += " update Bid set Status = 1 where ProjId = "+pid;
                DBHelper.ExecuteNonQuery(sql);

                Log l = new Log();
                l.OperType = OperateType.Create;
                l.UserId = userid;
                l.Description = "从新申请" + DBHelper.ExecuteScalar("select Name from Project where Id = " + pid) + " - 邀标申请";
                LogContext.WriteLog(l);
            }
        }

        public string GetBidInfo(string pid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBid(pid);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string GetBidFileInfo(string pid)
        {
            BidingFileContext bfc = new BidingFileContext();
            DataTable dt = bfc.getBidingFileDetail(pid);
            if (dt.Rows.Count == 0)
                return "";
            return dt.Rows[0]["Content"].ToString();
        }
        public string GetFiles(string pid, string ftype)
        {
            ProjectContext pc = new ProjectContext();
            DataTable dt = pc.GetProjectFiles(pid, ftype);
            string spath = Server.MapPath("../");
            var ndt = from row in dt.AsEnumerable()
                      select new { FilePath = row[0].ToString().Replace(spath, "../"), FileName = row[1].ToString(), fid= row[0].ToString().Substring(row[0].ToString().LastIndexOf('/')+1,36) };
            string json = JsonConvert.SerializeObject(ndt);
            return json;
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemP")]
        public ActionResult BidDetail(string pid)
        {
            if (pid == null)
                return View("Login");
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
            ViewBag.ProjDescription = dr["Content"].ToString();
            ViewBag.PublishDate = dr["PublishDate"].ToString();
            ViewBag.Content = dr["Content"].ToString();
            ViewBag.ProDescription = dr["ProDescription"].ToString();
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
            StringBuilder cHtml = new StringBuilder();
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
            ViewBag.moretime = "";
            if (Request["status"].ToString() == "3")
            {
                ViewBag.moretime = "<a href = 'javascript:;' class='js-cancle-meet' title='再次申请' onclick='bidApply()'><i class='meet-icon icon-cancel icon-daooutbtn'>再次申请</i></a>";
            }
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemP")]
        public ActionResult BidFileDetail(string pid)
        {
            if (pid == null)
                return View("/Login");
            ViewBag.pid = pid;
            BidingFileContext bc = new BidingFileContext();
            DataTable dt = bc.getBidingFileDetail(pid);
            DataRow dr = dt.Rows[0];
            ViewBag.BidFileName = dr["Name"].ToString();
            ViewBag.BidFileContent = dr["Content"].ToString().Replace("\r\n", "<br/>");
            ViewBag.BidFileUserName = dr["Publisher"].ToString();
            ViewBag.BidFilePublishDate = dr["PublishDate"].ToString();
            ViewBag.ProDescription= dr["ProDescription"].ToString();

            string spath = Server.MapPath("../");
            dt = bc.GetFiles(pid);
            string fujian = "";
            for(int i=0;i<dt.Rows.Count;i++)
            {
                if (i == 0)
                    fujian += "<div class='fujian'><i class='meet-icon icon-file'></i></div><div class='cc'><ul><li>"+dt.Rows[i]["FileName"].ToString()+ "<div><a href = '" + dt.Rows[i]["FilePath"].ToString().Replace(spath, "/") + "' target='_blank'> &nbsp;&nbsp;下载 </a></li>";
                else
                    fujian+= "<li>" + dt.Rows[i]["FileName"].ToString() + "<div><a href = '" + dt.Rows[i]["FilePath"].ToString().Replace(spath,"/") + "' target='_blank'> &nbsp;&nbsp;下载 </a></li>";
            }
            fujian += "</ul></div>";
            ViewBag.Fujian = fujian;
            ViewBag.moretime = "";
            if (Request["status"].ToString() == "3")
            {
                ViewBag.moretime = "<a href = 'javascript:;' class='js-cancle-meet' title='再次申请' onclick='bidFileApply()'><i class='meet-icon icon-cancel icon-daooutbtn'>再次申请</i></a>";
            }
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemP")]
        public ActionResult MakeBidFileDetail(string pid)
        {
            ViewBag.pid = pid;
            MakeBidFileContext mc = new MakeBidFileContext();
            DataTable dt = mc.GetMakeBidFileDetail(pid);

            DataRow dr = dt.Rows[0];
            ViewBag.PName = dr["Name"].ToString();
            ViewBag.Publisher = dr["Publisher"].ToString();
            ViewBag.PublishDate = dr["PublishDate"].ToString();
            ViewBag.Abstract = dr["Abstract"].ToString().Replace("\r", "    ").Replace("\n", "</br>");
            ViewBag.FileExplain = dr["FileExplain"].ToString().Replace("\r", "    ").Replace("\n", "</br>");

            dt = mc.GetBidingCompany(pid);
            string joinCompanys = "";
            string winCompanys = "";
            foreach (DataRow row in dt.Rows)
            {
                if (row["Biding"].ToString() == "1")
                {
                    string ConstructionContent = row["ConstructionContent"].ToString();
                    ConstructionContent = ConstructionContent.Substring(0, ConstructionContent.Length > 40 ? 40 : ConstructionContent.Length);
                    string QualificationLevel = row["QualificationLevel"].ToString();
                    QualificationLevel = QualificationLevel.Substring(0, QualificationLevel.Length > 16 ? 16 : QualificationLevel.Length);
                    joinCompanys += string.Format(@"<li><p class='f16'>{0}</p>
                                <p>投标报价：<span class='colblue'>{1}元</span></p>
                                <p>二次报价：<span class='colblue'>{2}元</span></p>
                                <p>资质等级：{3}</p>
                                <p>注册资金：{4}万元</p>
                                <p>{5}</p>
                            </li>", row["Name"].ToString(), row["FirstPrice"].ToString(), row["SecondPrice"].ToString(), QualificationLevel, row["RegisteredCapital"].ToString(), ConstructionContent);
                }
                if (row["Win"].ToString() == "1")
                {
                    winCompanys += string.Format(@"<li><h3>{0}</h3><p>{1}</p></li>", row["Name"].ToString(), row["Comment"].ToString());
                }
            }
            ViewBag.JoinCompany = joinCompanys;
            ViewBag.WinCompany = winCompanys;
            ViewBag.moretime = "";
            if (Request["status"].ToString() =="3")
            {
                ViewBag.moretime = "<a href='/Projects/MakeBidFile?pid=" + pid + @"&status=3' class='js-cancle-meet' title='再次申请' ><i class='meet-icon icon-cancel icon-daooutbtn'>再次申请</i></a>";
            }
            return View();
        }
        
        public string SaveMakeBidFile()
        {
            MakeBidFileContext mc = new MakeBidFileContext();
            string pid = Request["pid"].ToString();
            string abst = Request["abst"].ToString();
            mc.AddMakeBidFile(pid, abst, Session["UserId"].ToString());
            string joinCompany = Request["joincompany"].ToString();
            string[] companys = joinCompany.Split('|');
            string sql = "";
            foreach(string c in companys)
            {
                string[] cc = c.Split('-');
                sql += "update BidingCompany set biding=1, FirstPrice=" + cc[1] + ", SecondPrice=" + cc[2] + " where ProjId=" + pid + " and CompanyId=" + cc[0]+"; ";
            }
            DBHelper.ExecuteNonQuery(sql);
            string winCompany = Request["wincompany"].ToString();
            sql = "";
            companys = winCompany.Split('|');
            foreach(string c in companys)
            {
                string[] cc = c.Split('-');
                sql += "update BidingCompany set Win=1,biding=1, Comment=N'" + cc[1] + "' where ProjId=" + pid + " and CompanyId=" + cc[0] + "; ";
            }
            DBHelper.ExecuteNonQuery(sql);
            ProjectContext pc = new ProjectContext();
            pc.UpdateProjectStatus(pid, "定标文件审核中");
            pc.CreateApproveProcess(Session["UserId"].ToString(), pid, 4);

            Log l = new Log();
            l.OperType = OperateType.Create;
            l.UserId = Session["UserId"].ToString();
            l.Description = "创建" + DBHelper.ExecuteScalar("select Name from Project where Id = " + pid) + " - 定标文件";
            LogContext.WriteLog(l);

            return "1";
        }

        public void ReapplyMakeBidFile()
        {
            MakeBidFileContext mc = new MakeBidFileContext();
            string pid = Request["pid"].ToString();
            string abst = Request["abst"].ToString();
            mc.UpdateMakeBidFile(pid, abst);
            string joinCompany = Request["joincompany"].ToString();
            string[] companys = joinCompany.Split('|');
            string sql = "";
            foreach (string c in companys)
            {
                string[] cc = c.Split('-');
                sql += "update BidingCompany set biding=1, FirstPrice=" + cc[1] + ", SecondPrice=" + cc[2] + " where ProjId=" + pid + " and CompanyId=" + cc[0] + "; ";
            }
            DBHelper.ExecuteNonQuery(sql);
            string winCompany = Request["wincompany"].ToString();
            sql = "";
            companys = winCompany.Split('|');
            foreach (string c in companys)
            {
                string[] cc = c.Split('-');
                sql += "update BidingCompany set Win=1,biding=1, Comment=N'" + cc[1] + "' where ProjId=" + pid + " and CompanyId=" + cc[0] + "; ";
            }
            DBHelper.ExecuteNonQuery(sql);
            ProjectContext pc = new ProjectContext();
            pc.UpdateProjectStatus(pid, "定标文件审核中");
            sql = "update AppProcessing set Approved=1 where AppProcId=4 and ObjId=" + pid + " and Approved=3";
            sql += " update MakeBidingFile set Status = 1 where ProjId = "+pid;
            DBHelper.ExecuteNonQuery(sql);

            Log l = new Log();
            l.OperType = OperateType.Create;
            l.UserId = Session["UserId"].ToString();
            l.Description = "重新申请" + DBHelper.ExecuteScalar("select Name from Project where Id = " + pid) + " - 定标文件";
            LogContext.WriteLog(l);
        }

        public string GetMakeBidFiles(string pageSize, string pageIndex, string pname="")
        {
            MakeBidFileContext mc = new MakeBidFileContext();
            return mc.GetMakeBidFiles(pageSize, pageIndex, pname);
        }

        public void SaveRichText(string pid, string rtext)
        {
            MakeBidFileContext mc = new MakeBidFileContext();
            mc.SaveRichText(pid, rtext);
        }
    }
}