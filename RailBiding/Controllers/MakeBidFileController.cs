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
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("MakeBidFile", Session["RoleId"].ToString());
            return View();
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemM")]
        public ActionResult FileDetail(string pid)
        {
            ViewBag.SecondMenu = MenuHelper.GetSecondMenu("MakeBidFile", Session["RoleId"].ToString());
            ViewBag.pid = pid;
            MakeBidFileContext mc = new MakeBidFileContext();
            DataTable dt = mc.GetMakeBidFileDetail(pid);
            DataRow dr = dt.Rows[0];
            ViewBag.PName = dr["Name"].ToString();
            ViewBag.Publisher = dr["Publisher"].ToString();
            ViewBag.ProDescription = dr["ProDescription"].ToString().Replace("\n", "<br/>");
            ViewBag.PublishDate = dr["PublishDate"].ToString();
            ViewBag.Abstract = dr["Abstract"].ToString().Replace("\r", "    ").Replace("\n", "<br/>");
            ViewBag.FileExplain = dr["FileExplain"].ToString().Replace("\r", "    ").Replace("\n", "<br/>");

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
                    string secondprice = "";
                    if (row["SecondPrice"].ToString() == "")
                    {
                        secondprice = string.Format(@"<p> 二次报价：<span class='colblue'></span></p>");
                    }else
                    {
                        secondprice = string.Format(@"<p> 二次报价：<span class='colblue'>" + row["SecondPrice"].ToString()+"万元</span></p>");
                    }
                    joinCompanys += string.Format(@"<li><p class='f16'>{0}</p>
                                <p>投标报价：<span class='colblue'>{1}万元</span></p>
                                {2}
                                <p>资质等级：{3}</p>
                                <p>注册资金：{4}万元</p>
                                <p>{5}</p>
                            </li>", row["Name"].ToString(), row["FirstPrice"].ToString(), secondprice, QualificationLevel, row["RegisteredCapital"].ToString(), ConstructionContent);
                }
                if (row["Win"].ToString() == "1")
                {
                    winCompanys += string.Format(@"<li><h3>{0}</h3><p>{1}</p></li>", row["Name"].ToString(), row["Comment"].ToString());
                }
            }
            ViewBag.JoinCompany = joinCompanys;
            ViewBag.WinCompany = winCompanys;
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
            ViewBag.UserId = Session["UserId"].ToString();
            ViewBag.pid = pid;
            MakeBidFileContext mc = new MakeBidFileContext();
            DataTable dt = mc.GetMakeBidFileDetail(pid);
           
            DataRow dr = dt.Rows[0];
            ViewBag.PName = dr["Name"].ToString();
            ViewBag.Publisher= dr["Publisher"].ToString();
            ViewBag.PublishDate = dr["PublishDate"].ToString();
            ViewBag.ProDescription = dr["ProDescription"].ToString().Replace("\n", "<br/>");
            ViewBag.Abstract = dr["Abstract"].ToString().Replace("\r", "    ").Replace("\n","<br/>");
            ViewBag.FileExplain = dr["FileExplain"].ToString().Replace("\r", "    ").Replace("\n", "<br/>");

            dt = mc.GetBidingCompany(pid);
            string joinCompanys = "";
            string winCompanys = "";
            foreach(DataRow row in dt.Rows)
            {
                if (row["Biding"].ToString() == "1")
                {
                    string ConstructionContent = row["ConstructionContent"].ToString();
                    ConstructionContent = ConstructionContent.Substring(0, ConstructionContent.Length > 40 ? 40 : ConstructionContent.Length);
                    string QualificationLevel = row["QualificationLevel"].ToString();
                    QualificationLevel = QualificationLevel.Substring(0, QualificationLevel.Length > 16 ? 16 : QualificationLevel.Length);
                    string secondprice = "";
                    if (row["SecondPrice"].ToString() == "")
                    {
                        secondprice = string.Format(@"<p> 二次报价：<span class='colblue'></span></p>");
                    }
                    else
                    {
                        secondprice = string.Format(@"<p> 二次报价：<span class='colblue'>" + row["SecondPrice"].ToString() + "万元</span></p>");
                    }
                    joinCompanys += string.Format(@"<li><p class='f16'>{0}</p>
                                <p>投标报价：<span class='colblue'>{1}万元</span></p>
                                {2}
                                <p>资质等级：{3}</p>
                                <p>注册资金：{4}万元</p>
                                <p>{5}</p>
                            </li>", row["Name"].ToString(), row["FirstPrice"].ToString(), secondprice, QualificationLevel, row["RegisteredCapital"].ToString(), ConstructionContent);
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
        
        public string GetAllMakeBidFile(string pageSize, string pageIndex, string pname)
        {
            MakeBidFileContext mc = new MakeBidFileContext();
            return mc.GetMakeBidFiles(pageSize, pageIndex, pname, Session["UserId"].ToString()).Replace("\r","    ").Replace("\n","<br/>");
        }
        public string GetMyMakeBidFiles(string pageSize, string pageIndex, string pname, string fstatus)
        {
            string uid = Session["UserId"].ToString();
            MakeBidFileContext mc = new MakeBidFileContext();
            return mc.GetMyMakeBidFiles(uid, pageSize, pageIndex, pname, fstatus).Replace("\r", "    ").Replace("\n", "<br/>");
        }
    }
}