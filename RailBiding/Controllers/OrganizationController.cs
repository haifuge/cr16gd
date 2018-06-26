using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;

namespace RailBiding.Controllers
{
    public class OrganizationController : Controller
    {
        // GET: Organization
        public ActionResult Index()
        {
            return View();
        }

        public string GetOrganizations()
        {
            return "";
        }
    }
}