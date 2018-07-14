using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    /// <summary>
    /// 招标文件
    /// </summary>
    public class BidingFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Publisher { get; set; }
        public string PublishDate { get; set; }
        public string Status { get; set; }
    }

    public class BidingFileContext
    {
        public DataTable GetBidingFiles()
        {
            string sql = @"select p.Id, p.Name, bf.Content, d.Name+' '+ui.UserName as Publisher, CONVERT(varchar(20), bf.PublishDate, 23) as PublishDate, bf.Status
                            from BidingFile bf 
                            inner join Project p on bf.ProjId=p.Id
                            left join UserInfo ui on ui.ID=bf.PublisherId
                            inner join DepartmentUser du on du.UserId=ui.ID
                            left join Department d on du.DepartmentId=d.ID
                            order by bf.ProjId desc";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }

        public DataTable getBidingFileDetail(string pid)
        {
            string sql = @"select p.Id, p.Name, bf.Content, d.Name+' '+ui.UserName as Publisher, CONVERT(varchar(20), bf.PublishDate, 23) as PublishDate, bf.Status
                            from BidingFile bf 
                            inner join Project p on bf.ProjId=p.Id
                            left join UserInfo ui on ui.ID=bf.PublisherId
                            inner join DepartmentUser du on du.UserId=ui.ID
                            left join Department d on du.DepartmentId=d.ID
                            where p.Id=" + pid;
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }

        public bool AddBidingFile(string pid, string content, string publisherId, string status)
        {
            string sql = @"insert into BidingFile values("+pid+",'"+content+"', "+ publisherId + ", getdate(), "+status+")";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool UpdateBidingFile(string pid, string content)
        {
            string sql = @"update BidingFile set Content = '" + content + "' where ProjId =" + pid;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool UpdateBidingFileStatus(string id, string status)
        {
            string sql = @"update BidingFile set Status =" + status + " where ProjId =" + id;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public DataTable GetFiles(string pid)
        {
            string sql = "select FileName from BidDocument where ProjId="+pid+" and FileType=1";
            return DBHelper.GetDataTable(sql);
        }
    }
}
