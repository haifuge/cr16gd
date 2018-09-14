using DAL.Models;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.API
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }

        public string GetBusinessType()
        {
            BusinessTypeContext bc = new BusinessTypeContext();
            DataTable dt = bc.GetBusinessTypes();
            return JsonHelper.DataTableToJSON(dt);
        }

        public string GetCompanyType()
        {
            SystemSetup ss = new SystemSetup();
            DataTable dt = ss.GetCompanyBusinessTypes();
            return JsonHelper.DataTableToJSON(dt);
        }

        [HttpPost]
        public string FileUpload()
        {
            string pid = Request["pid"].ToString();
            string ftype = Request["ftype"].ToString();
            string comment = Request["comment"] ?? "";
            string path = Server.MapPath("/projectFiles/p"+pid);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var curFile = Request.Files[0];
            var fileExt = Path.GetExtension(curFile.FileName);
            string guid = Guid.NewGuid().ToString();
            string fullPath = path + "/" + guid+fileExt;
            curFile.SaveAs(fullPath);
            ProjectContext pc = new ProjectContext();
            //fullPath = "../projectFiles/p" + pid + "/" + guid + fileExt;
            pc.AddProjectFile(pid, ftype, fullPath, curFile.FileName, comment);
            return guid + "|"+ curFile.FileName;
        }

        public void FileDelete()
        {
            string fname = Request["fname"].ToString();
            string sql = @"declare @fpath nvarchar(400)
                            select @fpath=FilePath from BidDocument where FilePath like '%"+fname+ @"%'
                            delete BidDocument where FilePath like '%" + fname + @"%'
                            select @fpath; ";
            string fpath = DBHelper.ExecuteScalar(sql);
            System.IO.File.Delete(fpath);
        }

        public string GetApproveProcessingInfo(string oid, string apid)
        {
            // 获取审批流程数据
            string sql = @"select ap.Approved, ap.Comment, CONVERT(varchar(20),ap.DealDatetime,20) as dd, d.Name, ui.UserName, dbo.GetRootName(d.id) as pName, ap.Level, ui.id as uid
                            from vw_AppPLevel ap 
                            left join Department d on ap.DepartmentId=d.ID 
                            left join UserInfo ui on ui.ID=ap.UserId
                            where ap.AppProcId=" + apid + " and ap.ObjId=" + oid + " order by case when ap.DealDatetime is null then 0 else 1 end desc, ap.Level desc, ap.DealDatetime asc";
            DataTable dt = DBHelper.GetDataTable(sql);
            // 获取提交人数据
            if(apid=="1" ||apid=="5")
            {
                sql = @"select top 1 0 as Approved,'' as Comment, CONVERT(varchar(20),c.CreateDate,20) as dd, d.Name, ui.UserName,dbo.GetRootName(d.id) as pName, 1000 as Level, ui.id as uid
                        from Company c 
                        inner join UserInfo ui on c.SubmitUserId=ui.ID 
                        inner join DepartmentUser du on du.UserId=ui.ID and du.MainDeparment=1 and du.Status=1
                        inner join Department d on d.ID=du.DepartmentId
                        where c.ID=" + oid;
            }
            else
            {
                sql = @"select top 1 0 as Approved,'' as Comment, CONVERT(varchar(20),c.PublishDate,20) as dd, d.Name, ui.UserName,dbo.GetRootName(d.id) as pName, 1000 as Level, ui.id as uid
                        from Project c 
                        inner join UserInfo ui on c.PublisherId=ui.ID 
                        inner join DepartmentUser du on du.UserId=ui.ID and du.MainDeparment=1 and du.Status=1
                        inner join Department d on d.ID=du.DepartmentId
                        where c.ID=" + oid;
            }
            DataTable dt2 = DBHelper.GetDataTable(sql);
            DataRow dr = dt.NewRow();
            for(int i=0;i<dt2.Columns.Count;i++)
            {
                dr[i] = dt2.Rows[0][i].ToString();
            }
            dt.Rows.InsertAt(dr, 0);
            return JsonHelper.DataTableToJSON(dt);
        }

        public void UpdateApproveProcess(string apid, string oid, string aStatus, string comment)
        {
            string uid = Session["UserId"].ToString();
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@apid", apid);
            parameters[1] = new SqlParameter("@userid", uid);
            parameters[2] = new SqlParameter("@oid", oid);
            parameters[3] = new SqlParameter("@status", aStatus);
            parameters[4] = new SqlParameter("@comment", comment);
            DBHelper.ExecuteSP("UpdateApproveProcess", parameters);
            ApproveProcessLog(apid, oid, aStatus, uid);
        }
        public void ProjectReapply(string apid, string oid)
        {
            string sql = "update AppProcessing set Approved=1 where AppProcId="+ apid + " and ObjId="+ oid + " and Approved=3";
            DBHelper.ExecuteNonQuery(sql);
        }

        private void ApproveProcessLog(string apid, string oid, string aStatus, string uid)
        {
            Log l = new Log();
            string ap = "";
            if (aStatus == "2") { 
                l.OperType = OperateType.Approve;
                ap = "审核通过";
            }
            else if (aStatus == "3") {
                l.OperType = OperateType.Decline;
                ap = "驳回";
            }
            l.UserId = uid;
            string sql = "";
            switch (apid)
            {
                case "1":
                    sql = "select Name from Company where Id = " + oid;
                    l.Description = "名录外单位 - " + DBHelper.ExecuteScalar(sql) + ap;
                    break;
                case "2":
                    sql = "select Name from Project where Id = " + oid;
                    l.Description = DBHelper.ExecuteScalar(sql) + " - 招标文件"+ ap;
                    break;
                case "3":
                    sql = "select Name from Project where Id = " + oid;
                    l.Description = DBHelper.ExecuteScalar(sql) + " - 邀标申请"+ ap;
                    break;
                case "4":
                    sql = "select Name from Project where Id = " + oid;
                    l.Description = DBHelper.ExecuteScalar(sql) + " - 定标文件"+ap;
                    break;
                case "5":
                    sql = "select Name from Company where Id = " + oid;
                    l.Description = "名录内单位 - " + DBHelper.ExecuteScalar(sql) + ap;
                    break;
                default:
                    l.Description = "";
                    break;
            }
            LogContext.WriteLog(l);
        }

        public string UploadReference()
        {
            //string foreRef = Request["foreRef"].ToString();
            ProjectContext pc = new ProjectContext();
            string fullPath;
            //if (foreRef != "") {
            //    string g = foreRef;
            //    fullPath = pc.GetForeReference(g);
            //    System.IO.File.Delete(fullPath);
            //}
            string path = Server.MapPath("/CompanyFiles/Reference");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var curFile = Request.Files[0];
            string guid = Guid.NewGuid().ToString();
            var fileExt = Path.GetExtension(curFile.FileName);
            fullPath = path + "/" + guid + fileExt;
            curFile.SaveAs(fullPath);
            //临时存推荐书
            pc.AddProjectFile("0", "1", fullPath, curFile.FileName, "");
            return guid+"|"+curFile.FileName;
        }
        public string UploadZiZhiPic()
        {
            string input_zzmc = Request["input_zzmc"].ToString();
            string input_zsbh = Request["input_zsbh"].ToString();
            ProjectContext pc = new ProjectContext();
            string fullPath;
            string path = Server.MapPath("/CompanyFiles/ZiZhiPics");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var curFile = Request.Files[0];
            string guid = Guid.NewGuid().ToString();
            var fileExt = Path.GetExtension(curFile.FileName);
            fullPath = path + "/" + guid + fileExt;
            curFile.SaveAs(fullPath);
            //临时存推荐书
            string sql = "insert into CompanyZiZhiPic values(0, N'" + input_zzmc + "', N'" + input_zsbh + "', N'" + fullPath + "');";
            DBHelper.ExecuteNonQuery(sql);
            return guid + "|" + fullPath.Replace(Server.MapPath("../"), "../");
        }
        public string UploadTestifyFile()
        {
            string foreRef = Request["fname"].ToString();
            ProjectContext pc = new ProjectContext();
            string fullPath;
            string path = Server.MapPath("/projectFiles/WorkHistory");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var curFile = Request.Files[0];
            string guid = Guid.NewGuid().ToString();
            var fileExt = Path.GetExtension(curFile.FileName);
            fullPath = path + "/" + guid + fileExt;
            curFile.SaveAs(fullPath);
            return guid + fileExt+"|"+curFile.FileName;
        }

        public string GetCompanyCandidate(string page, string pagesize)
        {
            string cname = "";
            if (Request["cname"] != null)
                cname = Request["cname"].ToString();
            string ctype = "";
            if (Request["ctype"] != null)
                ctype = Request["ctype"].ToString();
            string inout = "";
            if (Request["inout"] != null)
                inout = Request["inout"].ToString();

            string where = "";
            if (cname != "")
                where += " c.Name like '%"+cname+"%' and ";
            if (ctype != "")
                where += " c.BusinessType = "+ctype+" and ";
            if (inout != "")
                where += " c.Type = "+inout+" and ";


            int pi = int.Parse(page);
            int ps = int.Parse(pagesize);
            int startIndex = (pi - 1) * ps + 1;
            int endIndex = pi * ps;
            string sql = @"select identity(int,1,1) as iid, c.ID*1 as ID, c.Name, CorporateRepresentative,QualificationLevel, RepPhone,RegisteredCapital,ConstructionContent,Type, ct.Name as companyType 
                           into #temp1
                           from Company c left join CompanyType ct on c.BusinessType=ct.ID 
                            where " + where+@" Status=1 and AuditStatus=2 order by Id
                            select * from #temp1 where iid between " + startIndex + " and " + endIndex + @"
                            drop table #temp1";
            DataTable dt = DBHelper.GetDataTable(sql);
            sql = "select count(1) from Company  where Status=1 and AuditStatus=2 ";
            string total = DBHelper.ExecuteScalar(sql);
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + JsonHelper.DataTableToJSON(dt) + ", \"total\":" + total + ", \"pagecount\":" + pagecount + "}";
        }

        public string GetBidingCompanys()
        {
            string pid = Request["pid"].ToString();
            string sql = @" select c.ID, c.Name, CorporateRepresentative, RepPhone,RegisteredCapital, ConstructionContent, QualificationLevel, ct.Name as companyType, [dbo].[GetProjectDepartmentByUserId](c.SubmitUserId) as refDepartment
                            from Company c left join CompanyType ct on c.BusinessType=ct.ID
                            where c.id in(select CompanyId from BidingCompany where Biding = 1 and ProjId = " + pid+")";
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.DataTableToJSON(dt);
        }
        /// <summary>
        /// 收到邀标，用户反馈更新数据库
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="pid"></param>
        /// <param name="token">验证码</param>
        /// <param name="res"></param>
        public void ResponseInvite(string cid, string pid, string token, string res)
        {
            string biding = "0";
            if (res == "1")
                biding = "1";
            string sql = @"update BidingCompany set CompanyResponse=" + res + ", Biding=" + biding + " where ProjId=" + pid + " and CompanyId=" + cid + " and VerifyCode='" + token + "'; ";
            DBHelper.ExecuteNonQuery(sql);
        }
        public string GetUserRoles()
        {
            string sql = "select ID, Name from UserRole order by ID";
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string CheckUserAccountExist(string uact)
        {
            string sql = "select count(1) from UserInfo where UserAccount='" + uact + "' and Status=1";
            return DBHelper.ExecuteScalar(sql).ToString();
        }
    }
}