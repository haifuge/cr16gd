using DAL.Models;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
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
            string fullPath = path + "\\" + guid+fileExt;
            curFile.SaveAs(fullPath);
            ProjectContext pc = new ProjectContext();
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


    }
}