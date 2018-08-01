using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;

namespace RailBiding.Mobile
{
    public class MobileInviteBidingController : Controller
    {
        // GET: MobileInviteBiding
        public ActionResult Index()
        {
            ViewBag.Token = Request["token"].ToString();
            ViewBag.CompanyId = Request["cid"].ToString();
            ViewBag.ProjId = Request["pid"].ToString();
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidDetail(ViewBag.ProjId);
            ViewBag.PName = dt.Rows[0]["Name"].ToString();
            ViewBag.PLocation = dt.Rows[0]["Location"].ToString();
            ViewBag.ProjType = dt.Rows[0]["ProjType"].ToString();
            ViewBag.ApplyDate = dt.Rows[0]["ApplyDate"].ToString();
            ViewBag.OpenDate = dt.Rows[0]["OpenDate"].ToString();
            ViewBag.PDescription = dt.Rows[0]["ProDescription"].ToString();
            return View();
        }
    }
}