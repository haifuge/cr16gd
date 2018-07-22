using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Log
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public OperateType OperType { get; set; }
        public string OperateDate { get; set; }
        public string Description { get; set; }
    }

    public class LogContext
    {
        public DataTable GetLogsOfPage(int page, int pageSize)
        {
            int max = page * pageSize;
            int min = (page - 1) * pageSize;
            string sql = @"select top "+pageSize+@" ID, UserAccount, UserName, OperateType, 
                                  CONVERT(varchar(20), OperateDate, 20) as OperateDate, Description 
                           from LogRecord where ID not in (select top "+min+" ID from LogRecord order by ID desc) order by ID desc";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
        public int GetTotalLogNum()
        {
            string sql = "select count(1) from LogRecord";
            int n = int.Parse(DBHelper.ExecuteScalar(sql));
            return n;
        }

        public static void WriteLog(Log log)
        {
            string sql = "insert into LogRecord values('" + log.UserId + "', '" + log.OperType + "', getdate(), N'" + log.Description + "')";
            DBHelper.ExecuteNonQuery(sql);
        }
    }

    public enum OperateType
    {
        Login=0,
        Create=1,
        Update=2,
        Delete=3,
        Insert=4,
        Approve=5,
        Decline=6
    }
}
