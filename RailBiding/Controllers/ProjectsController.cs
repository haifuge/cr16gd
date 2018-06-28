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
            p.Name = Request["Name"].ToString();
            p.ProjType = Request["ProjType"].ToString();
            p.Location = Request["Location"].ToString();
            p.PublisherId = int.Parse(Request["PublisherId"].ToString());
            p.ProDescription = Request["ProDescription"].ToString();
            ProjectContext pc = new ProjectContext();
            if (pc.AddProject(p))
                return "1";
            return "0";
        }
    }
}