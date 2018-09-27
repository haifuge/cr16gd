using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using RailBiding.Common;
using System.Data;
using DAL.Tools;

namespace RailBiding.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemN")]
        public ActionResult Index()
        {
            if (Session["RoleId"].ToString() == "2")
            {
                ViewBag.delebtn = "<a class='meet-btn red-btn small-size sm-btn' onclick='deleteNotice()'>" +
                                   "<i class='xs-meet-icon icon-dele'></i>删除</a>";
                ViewBag.createbtn = @"<a class='meet-btn green-btn small-size sm-btn' href='#' onclick='addNotice()'>
                <i class='xs-meet-icon icon-add'></i>添加</a>";
            }
            else
            {
                ViewBag.delebtn = "";
                ViewBag.createbtn = "";
            }
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

        public string GetNotifications()
        {
            NotificationContext nc = new NotificationContext();
            string pagesize = Request["pageSize"].ToString();
            string pageIndex = Request["pageIndex"].ToString();
            string pname = Request["pname"].ToString();
            string res= nc.GetNotifications(pagesize, pageIndex, pname);
            return res;
        }
    }
}