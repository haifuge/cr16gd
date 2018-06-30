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
        public void FileUpload()
        {
            string pid = Request["pid"].ToString();
            string ftype = Request["ftype"].ToString();
            string comment = Request["comment"] ?? "";
            string path = Server.MapPath("/projects/p"+pid);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var curFile = Request.Files[0];
            var fileExt = Path.GetExtension(curFile.FileName);
            string guid = Guid.NewGuid().ToString();
            string fullPath = path + "/" + guid+"."+fileExt;
            curFile.SaveAs(fullPath);
            ProjectContext pc = new ProjectContext();
            pc.AddProjectFile(pid, ftype, fullPath, curFile.FileName, comment);
        }
        /// <summary>
        /// 删除项目文件，文件全路径唯一，可直接删除
        /// </summary>
        public void RemoveFile()
        {
            string fullPath = Request["fullPath"].ToString();
            System.IO.File.Delete(fullPath);
            ProjectContext pc = new ProjectContext();
            pc.RemoveProjectFile(fullPath);
        }
    }
}