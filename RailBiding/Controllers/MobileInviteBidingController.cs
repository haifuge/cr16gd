using DAL.Models;
using System.Data;
using System.Web.Mvc;
using DAL.Tools;

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
            string responnse = bc.GetCompanyResponse(ViewBag.ProjId, ViewBag.CompanyId);
            if (responnse == "0")
            {
                ViewBag.btn = "<li><a href='#' class='bid-redbtn' onclick='response(2)'>不参加</a></li>" +
                             "<li><a href= '#' class='bid-greenbtn' onclick='response(1)'>参加</a></li>";
            }
            if (responnse == "2")
            {
                ViewBag.btn = "<li><a href= 'javascript:;' class='bid-clickafterbtn' style='color:red'>不参加</a></li>";
            }
            if (responnse == "1")
            {
                ViewBag.btn = "<li><a href= 'javascript:;' class='bid-clickafterbtn' style='color:red'>已参加</a></li>";
            }
            return View();
        }

        public string GetBidDetail(string pid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidDetail(pid);
            return JsonHelper.DataTableToJSON(dt);
        }
    }
}