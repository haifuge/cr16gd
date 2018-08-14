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
            ViewBag.cid = cid;
            BidContext bc = new BidContext();
            DataTable dt = bc.GetCompanyStatById(cid);
            if (dt.Rows.Count > 0)
            {
                ViewBag.Name = dt.Rows[0]["Name"].ToString();
                ViewBag.Total = dt.Rows[0]["Total"].ToString();
                ViewBag.JoinBiding = int.Parse(dt.Rows[0]["JoinBiding"].ToString())+int.Parse(dt.Rows[0]["Win"].ToString());
                ViewBag.NoJoin = dt.Rows[0]["NoJoin"].ToString();
                ViewBag.NoResponse = dt.Rows[0]["NoResponse"].ToString();
                ViewBag.NoWin = dt.Rows[0]["NoWin"].ToString();
                ViewBag.Win = dt.Rows[0]["Win"].ToString();
                ViewBag.TotalAmount = dt.Rows[0]["TotalAmount"].ToString();
            }
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