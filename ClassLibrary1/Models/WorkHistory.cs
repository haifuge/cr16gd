using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Tools;

namespace DAL.Models
{
    public class WorkHistory
    {
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ContractAmount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DelayStatus { get; set; }
        public int SettlementAmount { get; set; }
    }

    public class WorkHistoryContext
    {
        public List<WorkHistory> GetCompanyWorkHistory(int cId)
        {
            string sql = "select * from WorkHistory where CompanyId = " + cId+" order by ProjectId";
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.ConvertTableToObj<WorkHistory>(dt);
        }

        public bool AddWorkHistory(WorkHistory workHistory)
        {
            string sql = @"declare @mid int
                           select @mid = max(projectId)+1 from CompanyId where CompanyId = "+workHistory.CompanyId+";";
            sql += " insert into WorkHistory values(" + workHistory.CompanyId + ", @mid, '" + workHistory.ProjectName + "', " + workHistory.ContractAmount + ", " +
                  " '" + workHistory.StartDate + "', '" + workHistory.EndDate + "', '" + workHistory.DelayStatus + "'," + workHistory.SettlementAmount + ");";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
        public bool DeleteWorkHistory(int cId, int pId)
        {
            string sql = "delete WorkHistory where CompanyId = " + cId + " and ProjectId = " + pId;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
        public bool UpdateWorkHistory(WorkHistory workHistory)
        {
            string sql = "update WorkHistory set ProjectName= '" + workHistory.ProjectName + "', ContractAmount = " + workHistory.ContractAmount + ", StartDate = '" + workHistory.StartDate + "', " +
                        "EndDate = '" + workHistory.EndDate + "', DelayStatus = '" + workHistory.DelayStatus + "', SettlementAmount = " + workHistory.SettlementAmount +
                        " where CompanyId = " + workHistory.CompanyId + " and ProjectId = " + workHistory.ProjectId;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
    }
}
