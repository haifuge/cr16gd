using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;

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
    }
}