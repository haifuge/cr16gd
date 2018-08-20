using RailBiding.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using System.Data;
using DAL.Tools;
using OperateExcel;

namespace RailBiding.Controllers
{

    [AdministratorPagesFilter]
    public class StatisticController : Controller
    {
        // GET: Statistic
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemS")]
        public ActionResult Index()
        {
            return View();
        }

        public string GetCompaniesStatistic(string pageSize, string pageIndex, string pname)
        {
            BidContext bc = new BidContext();
            return bc.GetCompanyStats(pageSize, pageIndex, pname);
        }
        [VerifyLoginFilter]
        [ActiveMenuFilter(MenuName = "itemS")]
        public ActionResult StatisticDetail(string cid)
        {
            ViewBag.cid = cid;
            BidContext bc = new BidContext();
            DataTable dt = bc.GetCompanyStatById(cid);
            if (dt.Rows.Count > 0)
            {
                ViewBag.Name = dt.Rows[0]["Name"].ToString();
                ViewBag.Total = dt.Rows[0]["Total"].ToString();
                ViewBag.JoinBiding = dt.Rows[0]["JoinBiding"].ToString();
                ViewBag.NoJoin = dt.Rows[0]["NoJoin"].ToString();
                ViewBag.NoResponse = dt.Rows[0]["NoResponse"].ToString();
                ViewBag.NoWin = dt.Rows[0]["NoWin"].ToString();
                ViewBag.Win = dt.Rows[0]["Win"].ToString();
                ViewBag.TotalAmount = dt.Rows[0]["TotalAmount"].ToString();
            }
            return View();
        }
        public string GetCompanyStatById(string cid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetCompanyStatById(cid);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string GetCompanyBidDetail(string cid)
        {
            BidContext bc = new BidContext();
            DataTable dt = bc.GetCompanyBidDetail(cid);
            return JsonHelper.DataTableToJSON(dt);
        }
        public string ExportCompanysStat(string cids)
        {
            string where = "";
            if (cids == "0")
                where = " where c.Status = 1 ";
            else
                where = " where c.ID in (" + cids + ") ";
            string sql = @"select c.Name, SUM(isnull(bc.Biding,0)) as JoinTimes, sum(case when bc.CompanyResponse=0 then 1 else 0 end) as NoJoinTimes,
                                SUM(isnull(bc.Win, 0)) as WinBids, sum(case when bc.Win = 0 then 1 else 0 end) as NoWinBids, convert(varchar(20), max(p.PublishDate), 23) as LastJoinDate
                        from Company c
                        left join BidingCompany bc on c.ID = bc.CompanyId
                        left join Project p on bc.ProjId = p.Id
                        "+where+@"
                        group by c.Name
                        order by c.Name";
            DataTable dt = DBHelper.GetDataTable(sql);
            string tempPath = Server.MapPath("/");
            string file = ExcelOperator.ExportCompanysStat(dt, tempPath);
            return file.Replace(tempPath, "/");
        }
    }
}