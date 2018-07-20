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
            string sql = @"select ap.Approved, ap.Comment, CONVERT(varchar(20),ap.DealDatetime,20) as dd, d.Name, ui.UserName, dbo.GetRootName(d.id) as pName, ap.Level
                            from vw_AppPLevel ap 
                            left join Department d on ap.DepartmentId=d.ID 
                            left join UserInfo ui on ui.ID=ap.UserId
                            where ap.AppProcId=" + apid + " and ap.ObjId=" + oid + " order by case when ap.DealDatetime is null then 0 else 1 end desc, ap.Level desc, ap.DealDatetime asc";
            DataTable dt = DBHelper.GetDataTable(sql);
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
        }

        public string UploadReference()
        {
            string foreRef = Request["foreRef"].ToString();
            ProjectContext pc = new ProjectContext();
            string fullPath;
            if (foreRef != "") {
                string g = foreRef;
                fullPath = pc.GetForeReference(g);
                System.IO.File.Delete(fullPath);
            }
            string path = Server.MapPath("/projectFiles/Reference");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var curFile = Request.Files[0];
            string guid = Guid.NewGuid().ToString();
            var fileExt = Path.GetExtension(curFile.FileName);
            fullPath = path + "/" + guid + fileExt;
            curFile.SaveAs(fullPath);
            //临时存推荐书
            pc.AddProjectFile("0", "1", fullPath, curFile.FileName, "");
            return guid;
        }
        public string UploadTestifyFile()
        {
            string foreRef = Request["fname"].ToString();
            ProjectContext pc = new ProjectContext();
            string fullPath;
            string path = Server.MapPath("/projectFiles/WorkHistory");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (foreRef != "")
            {
                System.IO.File.Delete(path+"/"+foreRef);
            }
            var curFile = Request.Files[0];
            string guid = Guid.NewGuid().ToString();
            var fileExt = Path.GetExtension(curFile.FileName);
            fullPath = path + "/" + guid + fileExt;
            curFile.SaveAs(fullPath);
            return guid + fileExt+"|"+curFile.FileName;
        }

        public string GetCompanyCandidate(string page, string pagesize)
        {
            int pi = int.Parse(page);
            int ps = int.Parse(pagesize);
            int startIndex = (pi - 1) * ps + 1;
            int endIndex = pi * ps;
            string sql = @"select identity(int,1,1) as iid, ID*1 as ID, Name, CorporateRepresentative,QualificationLevel, RepPhone,RegisteredCapital,ConstructionContent 
                           into #temp1
                           from Company where Status=1 order by Id
                            select * from #temp1 where iid between " + startIndex + " and " + endIndex + @"
                            drop table #temp1";
            DataTable dt = DBHelper.GetDataTable(sql);
            sql = "select count(1) from Company ";
            string total = DBHelper.ExecuteScalar(sql);
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + JsonHelper.DataTableToJSON(dt) + ", \"total\":" + total + ", \"pagecount\":" + pagecount + "}";
        }
    }
}