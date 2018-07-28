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
        public string GetBidingFiles(string pageSize, string pageIndex)
        {
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps + 1;
            int endIndex = pi * ps;
            string sql = @"select identity(int,1,1) as iid, p.Id*1 as Id, p.Name, SUBSTRING(bf.Content, 0, 120) as Content, d.Name+' '+ui.UserName as Publisher, CONVERT(varchar(20), bf.PublishDate, 23) as PublishDate, bf.Status
                            into #temp1
                            from BidingFile bf 
                            inner join Project p on bf.ProjId=p.Id
                            left join UserInfo ui on ui.ID=bf.PublisherId
                            inner join DepartmentUser du on du.UserId=ui.ID
                            left join Department d on du.DepartmentId=d.ID
                            order by bf.ProjId desc
                            select * from #temp1 where iid between " + startIndex + " and " + endIndex + @"
                            select count(1) from #temp1
                            drop table #temp1";
            DataSet ds = DBHelper.GetDataSet(sql);
            string data = JsonHelper.DataTableToJSON(ds.Tables[0]);
            string total = ds.Tables[1].Rows[0][0].ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            DataTable dt = DBHelper.GetDataTable(CommandType.Text, sql);
            return "{\"List\":" + data + ", \"total\":" + total + ", \"PageCount\":" + pagecount + ",\"CurrentPage\":" + pageIndex + "}";
        }

        public string GetMyFileApprove(string userid, string pageSize, string pageIndex)
        {
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps + 1;
            int endIndex = pi * ps;
            string sql = @" select identity(int,1,1) as iid, a.* into #temp1 from (
                                select top 100 percent p.Id, p.Name, SUBSTRING(bf.Content, 0, 120) as Content, 
                                        d.Name+' '+ui.UserName as Publisher, CONVERT(varchar(20), 
                                        bf.PublishDate, 23) as PublishDate, a.Approved as Status
                                from BidingFile bf 
                                inner join Project p on bf.ProjId=p.Id
                                left join UserInfo ui on ui.ID=bf.PublisherId
                                inner join DepartmentUser du on du.UserId=ui.ID
                                left join Department d on du.DepartmentId=d.ID
                                inner join(
                                    select distinct a.ObjId, a.Approved 
                                    from vw_AppPLevel a 
                                    inner join (select MAX(level) as level,AppProcId, ObjId 
			                                    from vw_AppPLevel where AppProcId=2 and Approved=1 group by ObjId, AppProcId
                                ) b on a.AppProcId=b.AppProcId and a.Level>=b.level and a.ObjId=b.ObjId
                                where a.UserId=" + userid+ @") a on p.ID=a.ObjId order by p.Id desc) a 
                            select * from #temp1 where iid between " + startIndex + " and " + endIndex + @"
                            select count(1) from #temp1
                            drop table #temp1";
            DataSet ds = DBHelper.GetDataSet(sql);
            string data = JsonHelper.DataTableToJSON(ds.Tables[0]);
            string total = ds.Tables[1].Rows[0][0].ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            DataTable dt = DBHelper.GetDataTable(CommandType.Text, sql);
            return "{\"List\":" + data + ", \"total\":" + total + ", \"PageCount\":" + pagecount + ",\"CurrentPage\":" + pageIndex + "}";
        }

        public DataTable getBidingFileDetail(string pid)
        {
            string sql = @"select p.Id, p.Name, bf.Content, d.Name+' '+ui.UserName as Publisher, CONVERT(varchar(20), bf.PublishDate, 23) as PublishDate, bf.Status,  p.ProDescription
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
            string sql = @"if exists(select * from BidingFile where ProjId=" + pid + @")
                                update BidingFile set Content=N'" + content + "', Status=" + status + " where ProjId=" + pid + @"
							else
                                insert into BidingFile values(" + pid+",N'"+content+"', "+ publisherId + ", getdate(), "+status+")";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool UpdateBidingFile(string pid, string content)
        {
            string sql = @"update BidingFile set Content = N'" + content + "' where ProjId =" + pid;
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
            string sql = "select FileName, FilePath from BidDocument where ProjId="+pid+" and FileType=2";
            return DBHelper.GetDataTable(sql);
        }
    }
}
