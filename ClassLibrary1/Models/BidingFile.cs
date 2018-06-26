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
            string sql = @"select bf.Id, bf.Name, bf.Content, isnull(d.Name,'')+' '+ui.UserName as UserName, CONVERT(varchar(20),bf.PublishDate, 23) as PublishDate, bf.Status
                            from BidingFile bf 
                            left join UserInfo ui on bf.PublisherId=ui.id
                            left join Department d on ui.DepartmentId=d.ID and d.Status=1 
                            order by bf.Id desc";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }

        public DataTable getBidingFileDetail(string fid)
        {
            string sql = @"select bf.Name, bf.Content, isnull(d.Name,'')+' '+ui.UserName as UserName, CONVERT(varchar(20),bf.PublishDate, 23) as PublishDate
                            from BidingFile bf 
                            left join UserInfo ui on bf.PublisherId=ui.id
                            left join Department d on ui.DepartmentId=d.ID and d.Status=1
                            where bf.Id="+fid;
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }

        public bool AddBidingFile(BidingFile bf)
        {
            string sql = @"insert into BidingFile values('"+bf.Name+"', '"+bf.Content+"', "+bf.Publisher+", getdate(), 0)";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool UpdateBidingFile(BidingFile bf)
        {
            string sql = @"update BidingFile set Name ='" + bf.Name + "', Content = '" + bf.Content + "' where Id =" + bf.Id;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool UpdateBidingFileStatus(string id, string status)
        {
            string sql = @"update BidingFile set Status =" + status + " where Id =" + id;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
    }
}
