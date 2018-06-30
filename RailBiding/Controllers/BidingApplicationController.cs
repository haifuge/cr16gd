using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Tools;
using RailBiding.Common;

namespace RailBiding.Controllers
{
    public class BidingApplicationController : Controller
    {
        // GET: BidingApplication
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemB")]
        public ActionResult Index()
        {
            return View();
        }

        public string GetBidApplications()
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidApplications();
            return JsonHelper.DataTableToJSON(dt);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemB")]
        public ActionResult BidingApplicationDetail(string bid)
        {
            if (bid == null)
                return View("Login");
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidCompany(bid);
            DataRow dr = dt.Rows[0];
            ViewBag.Name = dr["Name"].ToString();
            ViewBag.Location = dr["Location"].ToString();
            ViewBag.Content = dr["Content"].ToString();
            ViewBag.Type = dr["Type"].ToString();
            ViewBag.Publisher = dr["Publisher"].ToString();
            ViewBag.BidingNum = dr["BidingNum"].ToString();
            ViewBag.ApplyDate = dr["ApplyDate"].ToString();
            ViewBag.OpenDate = dr["OpenDate"].ToString();
            ViewBag.PublishDate = dr["PublishDate"].ToString();
            ViewBag.ProjDescription = dr["ProjDescription"].ToString();

            dt = bc.GetBidingCompanys(bid);

            var joinC = (from c in dt.AsEnumerable()
                         where c.Field<int>("CompanyResponse") == 1
                         select new { name = c["CompanyName"].ToString() }).ToList();
            var noJoinC = (from c in dt.AsEnumerable()
                           where c.Field<int>("CompanyResponse") == 2
                           select new { name = c["CompanyName"].ToString() }).ToList();
            var noResponseC = (from c in dt.AsEnumerable()
                               where c.Field<int>("CompanyResponse") == 0
                               select new { name = c["CompanyName"].ToString() }).ToList();
            ViewBag.joinNum = joinC.Count;
            ViewBag.noJoinNum = noJoinC.Count;
            ViewBag.noResponseNum = noResponseC.Count;
            StringBuilder cHtml = new StringBuilder();
            foreach (var c in joinC)
            {
                cHtml.Append("<span>" + c.name + "</span>");
            }
            ViewBag.JoinCompanys = cHtml.ToString();
            cHtml.Clear();
            foreach (var c in noJoinC)
            {
                cHtml.Append("<span>" + c.name + "</span>");
            }
            ViewBag.NoJoinCompanys = cHtml.ToString();
            cHtml.Clear();
            foreach (var c in noResponseC)
            {
                cHtml.Append("<span>" + c.name + "</span>");
            }
            ViewBag.NoResponseCompanys = cHtml.ToString();
            return View();
        }

        public string GetBidApplicationDetail(string bid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidApplicationDetail(bid);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string GetBidApplicationAuditComment(string bid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidApplicationAuditComment(bid);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string GetBidApplicationTransferInfo(string bid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidApplicationTransferInfo(bid);
            return JsonHelper.DataTableToJSON(dt);
        }

    }
}