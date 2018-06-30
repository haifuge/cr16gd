﻿using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemS")]
        public ActionResult Index()
        {
            ViewBag.ActiveMenu = Request["activeMenu"].ToString();
            ViewBag.UserName = Session["UserName"];
            ViewBag.UserDepartment = Session["UserDepartment"];
            return View();
        }

        public string GetCompaniesStatistic()
        {
            return "";
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemS")]
        public ActionResult StatisticDetail(string cid)
        {
            return View();
        }
    }
}