﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Mobile
{
    public class MobileMakeBidFileController : Controller
    {
        // GET: MobileMakeBidFile
        public ActionResult Index(string pid)
        {
            ViewBag.UserId = Session["UserId"].ToString();
            ViewBag.pid = pid;
            MakeBidFileContext mc = new MakeBidFileContext();
            DataTable dt = mc.GetMakeBidFileDetail(pid);

            DataRow dr = dt.Rows[0];
            ViewBag.PName = dr["Name"].ToString();
            ViewBag.Publisher = dr["Publisher"].ToString();
            ViewBag.PublishDate = dr["PublishDate"].ToString();
            ViewBag.ProDescription = dr["ProDescription"].ToString().Replace("\n", "<br/>");
            ViewBag.Abstract = dr["Abstract"].ToString().Replace("\r", "    ").Replace("\n", "</br>");
            ViewBag.FileExplain = dr["FileExplain"].ToString().Replace("\r", "    ").Replace("\n", "</br>");

            dt = mc.GetBidingCompany(pid);
            string joinCompanys = "";
            string winCompanys = "";
            foreach (DataRow row in dt.Rows)
            {
                if (row["Biding"].ToString() == "1")
                {
                    string ConstructionContent = row["ConstructionContent"].ToString();
                    ConstructionContent = ConstructionContent.Substring(0, ConstructionContent.Length > 40 ? 40 : ConstructionContent.Length);
                    string QualificationLevel = row["QualificationLevel"].ToString();
                    QualificationLevel = QualificationLevel.Substring(0, QualificationLevel.Length > 16 ? 16 : QualificationLevel.Length);
                    joinCompanys += string.Format(@"<li class='onling'>
                                        <a href = '#' class='item-link item-content'>
                                            <div class='item-inner'>
                                                <div class='item-title-row'>
                                                    <div class='item-title t2'>{0}</div>
                                                </div>

                                                <div class='item-text'>
                                                    资质等级：<span>{3}</span>
                                                </div>
                                                <div class='item-text'>
                                                    注册资金：<span>{4}万元</span>
                                                </div>
                                                <div class='item-text'>
                                                    <span>{5}</span>
                                                </div>
                                                <div class='item-text'>
                                                    投标报价：<span class='col02'>{1}万元</span>
                                                    二次报价：<span class='col02'>{2}万元</span>
                                                </div>
                                            </div>
                                        </a>
                                    </li>", row["Name"].ToString(), row["FirstPrice"].ToString(), row["SecondPrice"].ToString(), QualificationLevel, row["RegisteredCapital"].ToString(), ConstructionContent);
                }
                if (row["Win"].ToString() == "1")
                {
                    winCompanys += string.Format(@"<li class='onling'>
                                        <a href = '#' class='item-link item-content'>
                                            <div class='item-inner'>
                                                <div class='item-title-row'>
                                                    <div class='item-title t2'>{0}</div>
                                                </div>
                                                <div class='item-text'>
                                                    <span>{1}</span>
                                                </div>
                                            </div>
                                        </a>
                                    </li>", row["Name"].ToString(), row["Comment"].ToString());
                }
            }
            ViewBag.JoinCompany = joinCompanys;
            ViewBag.WinCompany = winCompanys;
            return View();
        }
        public ActionResult AuditAction(string pid)
        {
            ViewBag.UserId = Session["UserId"].ToString();
            ViewBag.pid = pid;
            return View();
        }
    }
}