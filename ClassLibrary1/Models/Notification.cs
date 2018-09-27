using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Notification
    {
    }
    public class NotificationContext
    {
        public void AddNotification(string Name, string FileName, string publisherId)
        {
            string sql= "insert into Notification values('"+Name+"','"+FileName+"',getdate(),"+publisherId+")";
            DBHelper.ExecuteNonQuery(sql);
        }
        public void DeleteNotification(string id)
        {
            string sql = "delete Notification where Id in (" + id+")";
            DBHelper.ExecuteNonQuery(sql);
        }
        public string GetNotifications(string pageSize, string pageIndex, string pname)
        {
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps + 1;
            int endIndex = pi * ps;
            string sql = @"select identity(int,1,1) as iid, 1*Id as Id, Name, FileName, convert(varchar(20),PublishDate,23) as PublishDate, PublisherId 
                            into #temp1 
                            from Notification where Name like '%" + pname+@"%' order by Id desc
                            select * from #temp1 where iid between " + startIndex + " and " + endIndex + @"
                           select count(1) from #temp1
                           drop table #temp1";
            DataSet ds = DBHelper.GetDataSet(sql);
            string data = JsonHelper.DataTableToJSON(ds.Tables[0]).Replace("\r", "").Replace("\n", "").Replace("	", "");
            string total = ds.Tables[1].Rows[0][0].ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + data + ", \"total\":" + total + ", \"PageCount\":" + pagecount + ",\"CurrentPage\":" + pageIndex + "}";
        }
    }
}
