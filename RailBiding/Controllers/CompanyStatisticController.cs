﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;

namespace RailBiding.Controllers
{
    public class CompanyStatisticController : Controller
    {
        // GET: CompanyStatistic
        public ActionResult Index()
        {
            //BidContext bc = new BidContext();
            //List<CompanyStat> companyStats = bc.GetCompanyStats();
            //return View(companyStats);
            return View();
        }

        public ActionResult GetCompanyBidDetails()
        {
            string cId = Request["cId"].ToString();
            BidContext bc = new BidContext();
            
            return View();
        }
        
        public JsonResult GetCompanyStat()
        {
            string cId = Request["cId"].ToString();
            BidContext bc = new BidContext();
            DataTable dt = bc.GetCompanyBidDetail(cId);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
    }
}