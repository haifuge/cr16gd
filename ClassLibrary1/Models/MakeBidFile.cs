using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class MakeBidFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abstract { get; set; }
        public string ProjAbstract { get; set; }
        public string Publisher { get; set; }
        public string PublishDate { get; set; }
        public string FileExplain { get; set; }
        public int Status { get; set; }
        public List<MakeBidFileCompany> BidCompanies { get; set; }
    }
    public class MakeBidFileCompany
    {
        public int MBFId;
        public int CompanyId;
        public int BidAmount;
        public int Status;
    }

    public class MakeBidFileContext
    {
        public string GetMakeBidFiles(string pageSize, string pageIndex, string pname, string uid)
        {
            int ps = int.Parse(pageSize);
            SqlParameter[] paras = new SqlParameter[4];
            paras[0] = new SqlParameter("@uid", uid);
            paras[1] = new SqlParameter("@pageSize", pageSize);
            paras[2] = new SqlParameter("@pageIndex", pageIndex);
            paras[3] = new SqlParameter("@pname", pname);
            DataSet ds = DBHelper.ExecuteDataset(DBHelper.GetConnection(), "GetMakeBidFileByUserId", paras);
            DataTable dt = ds.Tables[0];
            string data = JsonHelper.DataTableToJSON(dt);
            string total = ds.Tables[1].Rows[0][0].ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + data + ", \"total\":" + total + ", \"PageCount\":" + pagecount + ",\"CurrentPage\":" + pageIndex + "}";
        }

        public string GetMyMakeBidFiles(string uid, string pageSize, string pageIndex, string pname, string status)
        {
            string where = "";
            if (status != "")
                where += " and a.Approved = " + status;
            if (pname != "")
                where += " and p.Name like '%" + pname + "%'";
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps + 1;
            int endIndex = pi * ps;
            string sql = @" select identity(int,1,1) as iid, a.* into #temp1 from (
                            select distinct p.Id*1 as Id, p.Name, dbo.GetProjectDepartmentByUserId(p.PublisherId) as PubDepartment, mb.Abstract, 
                            convert(varchar(20),mb.PublishDate,23) as PublishDate, a.Approved as Status
                            from project p inner join Bid b on p.Id=b.ProjId
                            inner join MakeBidingFile mb on mb.ProjId=p.Id and mb.Status<>0
                            left join UserInfo ui on b.PublisherId=ui.ID
                            left join DepartmentUser du on du.UserId=ui.ID and du.Status=1
                            left join Department d on du.DepartmentId=d.ID
                            inner join(
                                select distinct a.ObjId, a.Approved 
                                from vw_AppPLevel a 
                                inner join (select MAX(level) as level,AppProcId, ObjId 
			                                from vw_AppPLevel where AppProcId=4 and Approved=1 group by ObjId, AppProcId
                                ) b on a.AppProcId=b.AppProcId and a.Level>=b.level and a.ObjId=b.ObjId
                            where a.UserId=" + uid + @") a on p.ID=a.ObjId " + where + @" where mb.status<>3
                            ) a order by a.Id desc;
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

        public DataTable GetMakeBidFileDetail(string id)
        {
            string sql = @"select p.Name, dbo.GetProjectDepartmentByUserId(p.PublisherId) as Publisher,p.ProDescription,p.PublisherId as uid, CONVERT(varchar(20), p.PublishDate, 23) as PublishDate, mf.Abstract,mf.FileExplain,mf.Status 
                            from MakeBidingFile mf inner join Project p on mf.ProjId=p.Id
                            where p.Id=" + id;
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
        public DataTable GetFiles(string pid)
        {
            string sql = "select FileName from BidDocument where ProjId=" + pid + " and FileType=4";
            return DBHelper.GetDataTable(sql);
        }
        /// <summary>
        /// 新建定标文件
        /// </summary>
        /// <param name="makeBidFile"></param>
        /// <param name="ss">定标文件状态，0保存，1提交</param>
        /// <returns></returns>
        public void AddMakeBidFile(string pid, string abst, string puserid, string ss)
        {
            string sql = "select count(1) from MakeBidingFile where ProjId = " + pid;
            string n = DBHelper.ExecuteScalar(sql);
            if (n != "0")
            {
                sql = "update MakeBidingFile set Abstract = N'" + abst + "', PublisherId=" + puserid + ", PublishDate=getdate(), Status=" + ss + " where ProjId = " + pid;
            }
            else { 
                sql = "insert into MakeBidingFile(ProjId, Abstract, PublisherId, PublishDate, Status) values("+pid+",N'"+abst+"',"+puserid+",getdate(),"+ss+")";
            }
            DBHelper.ExecuteNonQuery(sql);
        }

        public bool AddMakeBidFileCompany(string mbfId, string cId, string amount)
        {
            string sql = "insert into MakeBidingFileCompany values("+mbfId+", "+cId+", "+amount+", 0)";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
        public bool RemoveMakeBidFileCompany(string mbfId, string cId)
        {
            string sql = "delete MakeBidingFileCompany where MBFId=" + mbfId + " and CompanyId = " + cId;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 更新定标文件，参加投标单位状态。0：参加；1：拟中标
        /// </summary>
        /// <param name="mbfId"></param>
        /// <param name="cId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateMakeBidFileCompanyStatus(string mbfId, string cId, string status)
        {
            string sql = "update MakeBidingFileCompany set Status = "+status+" where MBFId=" + mbfId + " and CompanyId = " + cId;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public DataTable GetBidingCompany(string pid)
        {
            string sql = @"select bc.ProjId, bc.CompanyId, c.Name, bc.Comment, bc.Biding, bc.Win,bc.FirstPrice, bc.SecondPrice, c.QualificationLevel,c.RegisteredCapital, c.ConstructionContent
                            from BidingCompany bc inner
                            join Company c on bc.CompanyId = c.ID
                            where bc.ProjId = " + pid;
            return DBHelper.GetDataTable(sql);
        }

        public void SaveRichText(string pid, string rtext)
        {
            string sql = "update MakeBidingFile set FileExplain = N'"+rtext.Replace(" ","")+"' where ProjId="+pid;
            DBHelper.ExecuteNonQuery(sql);
        }

        public void UpdateMakeBidFile(string pid, string abst, string ss="1")
        {
            if(ss=="1")
            {
                ss = ", Status=1 ";
            }
            else
            {
                ss = "";
            }
            string sql = "update MakeBidingFile set Abstract=N'"+abst+"'"+ss+" where ProjId=" + pid;
            DBHelper.ExecuteNonQuery(sql);
        }
    }
}
