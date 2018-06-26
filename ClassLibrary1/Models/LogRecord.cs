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
        public DataTable GetLogRecords()
        {
            string sql= @"select lr.Id, ui.UserName, OperateType, CONVERT(varchar(20), lr.operateDate, 20) as OperateDate, Description 
                            from LogRecord lr left join UserInfo ui on lr.UserAccount = ui.UserAccount order by lr.operateDate desc";
            return DBHelper.GetDataTable(sql);
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
