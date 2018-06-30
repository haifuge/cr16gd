using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using System.Data;
using DAL.Tools;

namespace RailBiding.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Projects
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemP")]
        public ActionResult Index()
        {
            return View();
        }
        public string GetAllProjects()
        {
            ProjectContext pc = new ProjectContext();
            DataTable dt = pc.GetAllProjects();
            return JsonHelper.DataTableToJSON(dt);
        }

        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemP")]
        public ActionResult Detail(string pid)
        {
            ProjectContext pc = new ProjectContext();
            DataTable dt = pc.GetProject(pid);
            DataRow dr = dt.Rows[0];
            ViewBag.pName = dr["Name"].ToString();
            ViewBag.pType = dr["ProjType"].ToString();
            ViewBag.pLocation = dr["Location"].ToString();
            ViewBag.pProDescription = dr["ProDescription"].ToString().Replace("\n","<br/>");
            return View();
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
                return "1";
            return "0";
        }


        [HttpPost]
        public bool AddBidFile()
        {
            string pid = Request["pid"].ToString();
            string content = Request["content"].ToString();
            string publisherId = Session["UserId"].ToString();
            BidingFileContext bfc = new BidingFileContext();
            return bfc.AddBidingFile(pid, content, publisherId);
        }
    }
}