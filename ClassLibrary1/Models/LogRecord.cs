using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Tools;

namespace DAL.Models
{
    public class LogRecord
    {
        public int Id { set; get; }
        public string UserName { get; set; }
        public string OperateType { get; set; }
        public string OperateDate { get; set; }
        public string Description { get; set; }
    }

    public class LogRecordContext
    {
        public string GetLogRecords(string pageSize, string pageIndex)
        {

            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps + 1;
            int endIndex = pi * ps;
            string sql= @"select identity(int,1,1) as iid, lr.Id*1 as Id, ui.UserName, OperateType, CONVERT(varchar(20), lr.operateDate, 20) as OperateDate, Description 
                            into #temp1 
                            from LogRecord lr left join UserInfo ui on lr.UserAccount = ui.UserAccount order by lr.operateDate desc
                            select * from #temp1 where iid between " + startIndex + " and " + endIndex + @"
                            drop table #temp1";
            DataTable dt = DBHelper.GetDataTable(sql);
            sql = "select count(1) from LogRecord ";
            string total = DBHelper.ExecuteScalar(sql);
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + JsonHelper.DataTableToJSON(dt) + ", \"total\":" + total + ", \"pagecount\":" + pagecount + "}";
        }
        public bool AddLog(string userAccount, string opType, string description)
        {
            string sql = "insert into LogRecord values('"+ userAccount + "', '"+ opType + "', getdate(), '"+ description + "')";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
    }
}
