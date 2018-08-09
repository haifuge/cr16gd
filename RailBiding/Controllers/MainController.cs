using DAL.Tools;
using RailBiding.Common;
using System.Data;
using System.Web.Mvc;

namespace RailBiding.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        [VerifyLoginFilter]
        public ActionResult Index()
        {
            ViewBag.UserName = Session["UserName"];
            ViewBag.UserDepartment = Session["UserDepartment"];
            ViewBag.RoleId = Session["RoleId"];
            return View();
        }

        public string GetToBeDone()
        {
            string sql = @"select a.AppProcId, sum(case when c.ID is null then 0 else 1 end) as company, sum(case when bf.ProjId is null then 0 else 1 end) as bidfile, 
			                        sum(case when b.ProjId is null then 0 else 1 end) as b, sum(case when mb.ProjId is null then 0 else 1 end) as mb 
	                        from (select distinct a.ObjId, a.Approved, a.AppProcId 
		                          from vw_AppPLevel a 
			                        inner join (select MAX(level) as level,AppProcId, ObjId 
						                        from vw_AppPLevel where Approved=1 group by ObjId, AppProcId
		                        ) b on a.AppProcId=b.AppProcId and a.Level>=b.level and a.ObjId=b.ObjId
		                        where a.UserId="+Session["UserId"].ToString()+ @" and a.Approved=1) a 
	                        left join Company c on a.ObjId=c.ID and a.AppProcId=1 and c.AuditStatus=1
	                        left join BidingFile bf on a.ObjId=bf.ProjId and a.AppProcId=2 and bf.Status=1
	                        left join Bid b on a.ObjId=b.ProjId and a.AppProcId=3 and b.Status=1
	                        left join MakeBidingFile mb on a.ObjId=mb.ProjId and a.AppProcId=4 and mb.Status=1
	                        group by a.AppProcId";
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.DataTableToJSON(dt);
        }
    }
}