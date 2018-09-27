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
            string sql = "delete Notification where Id = " + id;
            DBHelper.ExecuteNonQuery(sql);
        }
        public DataTable GetNotifications()
        {
            string sql = "select * from Notification order by Id desc";
            return DBHelper.GetDataTable(sql);
        }
    }
}
