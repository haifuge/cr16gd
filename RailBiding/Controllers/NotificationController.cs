using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;

namespace RailBiding.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        public void AddNotification()
        {
            string name = Request["name"].ToString();
            string filename = Request["filename"].ToString();
            string publisherId = Session["UserId"].ToString();
            NotificationContext nc = new NotificationContext();
            nc.AddNotification(name, filename, publisherId);
        }

        public void DeleteNotification()
        {
            string id = Request["id"].ToString();
            NotificationContext nc = new NotificationContext();
            nc.DeleteNotification(id);
        }
    }
}