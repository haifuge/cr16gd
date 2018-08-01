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
            
            return View();
        }

        public string GetBidDetail(string pid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetBidDetail(ViewBag.ProjId);
            return JsonHelper.DataTableToJSON(dt);
        }
    }
}