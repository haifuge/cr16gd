using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Tools;
using System.Data;

namespace RailBiding.Controllers
{
    public class MakeBidFileController : Controller
    {
        // GET: 定标文件
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult Index()
        {
            ViewBag.UserName = Session["UserName"];
            ViewBag.UserDepartment = Session["UserDepartment"];
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult FileDetail()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult AddFile()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult FileApplication()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult FileApprove()
        {
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult FileApproveDetail(string pid)
        {
            ViewBag.pid = pid;
            MakeBidFileContext mc = new MakeBidFileContext();
            DataTable dt = mc.GetMakeBidFileDetail(pid);
           
            DataRow dr = dt.Rows[0];
            ViewBag.PName = dr["Name"].ToString();
            ViewBag.Publisher= dr["Publisher"].ToString();
            ViewBag.PublishDate = dr["PublishDate"].ToString();
            ViewBag.Abstract = dr["Abstract"].ToString().Replace("\r", "    ").Replace("\n","</br>");
            ViewBag.FileExplain = dr["FileExplain"].ToString().Replace("\r", "    ").Replace("\n", "</br>");

            dt = mc.GetBidingCompany(pid);
            string joinCompanys = "";
            string winCompanys = "";
            foreach(DataRow row in dt.Rows)
            {
                if (row["Biding"].ToString() == "1")
                {
                    joinCompanys += string.Format(@"<li><p class='f16'>{0}</p>
                                <p>投标报价：<span class='colblue'>{1}元</span></p>
                                <p>二次报价：<span class='colblue'>{2}元</span></p>
                                <p>资质等级：{3}</p>
                                <p>注册资金：{4}万元</p>
                                <p>{5}</p>
                            </li>", row["Name"].ToString(), row["FirstPrice"].ToString(), row["SecondPrice"].ToString(), row["QualificationLevel"].ToString().Substring(0,16), row["RegisteredCapital"].ToString(), row["ConstructionContent"].ToString().Substring(0,40));
                }
                if(row["Win"].ToString()=="1")
                {
                    winCompanys += string.Format(@"<li><h3>{0}</h3><p>{1}</p></li>", row["Name"].ToString(), row["Comment"].ToString());
                }
            }
            ViewBag.JoinCompany = joinCompanys;
            ViewBag.WinCompany = winCompanys;
            return View();
        }
        
        public string GetAllMakeBidFile()
        {
            MakeBidFileContext mc = new MakeBidFileContext();
            DataTable dt = mc.GetMakeBidFiles();
            return JsonHelper.DataTableToJSON(dt).Replace("\r","    ").Replace("\n","</br>");
        }
        public string GetMyMakeBidFiles()
        {
            string uid = Session["UserId"].ToString();
            MakeBidFileContext mc = new MakeBidFileContext();
            DataTable dt = mc.GetMyMakeBidFiles(uid);
            return JsonHelper.DataTableToJSON(dt).Replace("\r", "    ").Replace("\n", "</br>");
        }
    }
}