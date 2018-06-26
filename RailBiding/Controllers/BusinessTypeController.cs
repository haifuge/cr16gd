using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Tools;

namespace RailBiding.Controllers
{
    public class BusinessTypeController : Controller
    {
        // GET: BusinessType
        public ActionResult Index()
        {
            return View();
        }

        public string GetBusinessTypes()
        {
            BusinessTypeContext btc = new BusinessTypeContext();
            DataTable bts = btc.GetBusinessTypes();
            return JsonHelper.DataTableToJSON(bts);
        }

        public bool AddBusinessType()
        {
            string name = Request["name"].ToString();
            BusinessTypeContext btc = new BusinessTypeContext();
            return btc.AddBusinessType(name);
        }

        public bool RemoveBusinessType()
        {
            string ids = Request["ids"].ToString();
            BusinessTypeContext btc = new BusinessTypeContext();
            return btc.RemoveBusinessType(ids);
        }
    }
}