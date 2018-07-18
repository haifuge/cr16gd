using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
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
        public DataTable GetMakeBidFiles()
        {
            string sql = @"select p.Id, p.Name, dbo.GetProjectDepartmentByUserId(p.PublisherId) as PubDepartment, mb.Abstract, 
                            convert(varchar(20),mb.PublishDate,23) as PublishDate, mb.Status
                            from MakeBidingFile mb inner join Project p on mb.ProjId = p.Id order by p.Id desc; ";
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetMyMakeBidFiles(string uid)
        {
            string sql = @"select p.Id, p.Name, dbo.GetProjectDepartmentByUserId(p.PublisherId) as PubDepartment, mb.Abstract, 
                            convert(varchar(20),mb.PublishDate,23) as PublishDate, mb.Status
                            from project p inner join Bid b on p.Id=b.ProjId
                            inner join MakeBidingFile mb on bf.ProjId=p.Id
                            left join UserInfo ui on b.PublisherId=ui.ID
                            left join DepartmentUser du on du.UserId=ui.ID
                            left join Department d on du.DepartmentId=d.ID
                            inner join(
                                select distinct a.ObjId, a.Approved 
                                from vw_AppPLevel a 
                                inner join (select MAX(level) as level,AppProcId, ObjId 
			                                from vw_AppPLevel where AppProcId=4 and Approved=1 group by ObjId, AppProcId
                                ) b on a.AppProcId=b.AppProcId and a.Level>=b.level and a.ObjId=b.ObjId
                            where a.UserId=" + uid + @") a on p.ID=a.ObjId
                            order by p.Id desc;";
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetMakeBidFileDetail(string id)
        {
            string sql = @"select p.Name, dbo.GetProjectDepartmentByUserId(p.PublisherId) as Publisher, CONVERT(varchar(20), p.PublishDate, 23) as PublishDate, mf.Abstract,mf.FileExplain,mf.Status 
                            from MakeBidingFile mf inner join Project p on mf.ProjId=p.Id
                            where p.Id=" + id;
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
        public DataTable GetFiles(string pid)
        {
            string sql = "select FileName from BidDocument where ProjId=" + pid + " and FileType=3";
            return DBHelper.GetDataTable(sql);
        }
        /// <summary>
        /// 新建定标文件
        /// </summary>
        /// <param name="makeBidFile"></param>
        /// <returns></returns>
        public void AddMakeBidFile(string pid, string abst, string puserid)
        {
            string sql = "insert into MakeBidingFile(ProjId, Abstract, PublisherId, PublishDate, Status) values("+pid+",N'"+abst+"',"+puserid+",getdate(),1)";
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
    }
}
