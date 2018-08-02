using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Mobile
{
    public class MobileBidController : Controller
    {
        // GET: MobileBid
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuditAction()
        {
            return View();
        }
    }
}