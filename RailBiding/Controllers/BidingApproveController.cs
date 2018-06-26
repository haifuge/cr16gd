using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Tools;

namespace RailBiding.Controllers
{
    public class BidingApproveController : Controller
    {
        // GET: BidingApprove
        public ActionResult Index()
        {
            return View();
        }

        public string GetBidingApproves()
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidingApproves();
            return JsonHelper.DataTableToJSON(dt);
        }

        public ActionResult BidingApproveDetail(string bid)
        {
            if (bid == null)
                return View("\\Login");
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
            StringBuilder cHtml = new StringBuilder();
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                cHtml.Append("<span>"+dt.Rows[i]["CompanyName"].ToString()+"<i><img src='/img/icon-del.png' alt=''></i></span>");
            }
            ViewBag.JoinCompanys = cHtml.ToString();
            return View();
        }

        public string GetBidingApproveDetail(string bid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidingApproveDetail(bid);
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
        public string ProcessBidApplication()
        {
            string operation = Request["operation"].ToString();
            string comment = Request["comment"] ?? "";
            BidContext bc = new BidContext();
            bool suc = bc.ProcessBidApplication(operation, comment);
            return suc.ToString();
        }
        public void AddBidingCompany(string bid, string cid)
        {
            BidContext bc = new BidContext();
            bc.AddCompanyToBid(bid, cid);
        }
        public void RemoveBidingCompany(string bid, string cid)
        {
            BidContext bc = new BidContext();
            bc.RemoveBidingCompany(bid, cid);
        }
    }
}