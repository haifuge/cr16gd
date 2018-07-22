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
    public class StatisticController : Controller
    {
        // GET: Statistic
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemS")]
        public ActionResult Index()
        {
            return View();
        }

        public string GetCompaniesStatistic(string pageSize, string pageIndex)
        {
            BidContext bc = new BidContext();
            return bc.GetCompanyStats(pageSize, pageIndex);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemS")]
        public ActionResult StatisticDetail(string cid)
        {
            return View();
        }
        public string GetCompanyStatById(string cid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetCompanyStatById(cid);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string GetCompanyBidDetail(string cid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetCompanyBidDetail(cid);
            return JsonHelper.DataTableToJSON(dt);
        }
    }
}