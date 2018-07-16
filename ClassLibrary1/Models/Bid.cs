using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Bid
    {
        public int ProjId { get; set; }
        public string ApplyDate { get; set; }
        public string OpenDate { get; set; }
        public string BidingNum { get; set; }
        public string PublishDate { get; set; }
        public int PublisherId { get; set; }
        public string Status { get; set; }
    }

    public class BidContext
    {
        public DataTable GetAllBids()
        {
            string sql = @"select p.Id, p.Name, bf.Content, p.Location, d.name+' '+ui.UserName as Publisher, Convert(varchar(20),b.PublishDate, 23) as PublishDate,
	                            convert(varchar(20),b.ApplyDate,23) as ApplyDate, CONVERT(varchar(20), b.OpenDate ,23) as OpenDate, b.Status
                            from project p inner join Bid b on p.Id=b.ProjId
                            inner join BidingFile bf on bf.ProjId=p.Id
                            left join UserInfo ui on b.PublisherId=ui.ID
                            left join DepartmentUser du on du.Userid=ui.ID
                            left join Department d on du.DepartmentId=d.ID
                            order by p.Id desc";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
        public DataTable GetBid(string pid)
        {
            string sql = @"select CONVERT(varchar(20), ApplyDate, 23) as ApplyDate,BidingNum, Status,
		                            CONVERT(varchar(20), OpenDate, 23) as OpenDate, 
		                            CONVERT(varchar(20), PublishDate, 23) as PublishDate 
                            from Bid where ProjId="+pid;
            return DBHelper.GetDataTable(sql);
        }

        public bool AddBid(Bid bid)
        {
            string sql = @"insert into Bid(ProjId, ApplyDate, OpenDate, BidingNum, PublishDate, PublisherId, Status)
                           values(" + bid.ProjId+", '"+bid.ApplyDate+"', '"+bid.OpenDate+"', "+bid.BidingNum+", getdate(),"+bid.PublisherId+", N'"+bid.Status+"')";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
        public DataTable GetFiles(string pid)
        {
            string sql = "select FileName from BidDocument where ProjId=" + pid + " and FileType=2";
            return DBHelper.GetDataTable(sql);
        }

        public void RemoveBidingCompany(string pid, string cid)
        {
            string sql = "delete BidingCompany where ProjId="+pid+ " and CompanyId="+cid;
            DBHelper.ExecuteNonQuery(sql);
        }

        public bool AddCompanyToBid(string bid, string cid)
        {
            throw new NotImplementedException();
        }

        public bool ProcessBidApplication(string operation, string comment)
        {
            throw new NotImplementedException();
        }

        public DataTable GetBidingCompanys(string pid)
        {
            string sql = @"select c.Name, bc.CompanyResponse 
                            from BidingCompany bc 
                            inner join Company c on bc.CompanyId=c.ID 
                            where bc.ProjId=" + pid;
            return DBHelper.GetDataTable(sql);
        }

        public bool UpdateBid(Bid bid)
        {
            string sql = "update Bid set BidName='";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
        public bool DeleteBid(int bid)
        {
            string sql = "delete Bid where ID=" + bid;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool AddBidingCompany(string pid, string cid)
        {
            string sql= "insert into BidingCompany(ProjId, CompanyId, CompanyResponse, Biding, Win) values(" + pid+", "+cid+", 0, 0, 0)";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool RemoveBidingCompany(int pid, int cid)
        {
            string sql = "delete BidingCompany where ProjId = " + pid + " and CompanyId = " + cid;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool UpdateBidingCompany(int pid, int cid, int firstPrice, int secondPrice, string comment, int win)
        {
            string sql = "update BidingCompany set FirstPrice = "+firstPrice+ ", SecondPrice = "+ secondPrice + ", Comment=N'"+comment+"', Win="+win+" where ProjId = " + pid + " and CompanyId = " + cid;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool SetWinBidCompany(int pid, int cid)
        {
            string sql = "update BidingCompany set Win=1 where ProjId = " + pid + " and CompanyId = " + cid;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public List<BidCompany> GetBidCompanies(int bid)
        {
            string sql = @"select Id, Name, Contact, ContactPhone 
                           from Company 
                           where Id not in (select CompanyId from CompanyInBid where BidId = "+bid+") order by Name";
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.ConvertTableToObj<BidCompany>(dt);
        }

        public List<CompanyStat> GetCompanyStats()
        {
            string sql = @"select CompanyName, SUM(response) as JoinTimes, sum(case when Response=0 then 1 else 0 end) as NoJoinTimes, 
		                            SUM(WinBid) as WinBids, COUNT(1) - SUM(WinBid) as NoWinBids, Convert(varchar(20),MAX(JoinDate),20) as LastJoinDate
                            from CompanyInBid group by CompanyName";
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.ConvertTableToObj<CompanyStat>(dt);
        }
        public List<CompanyBidDetail> GetCompanyBidDetail(string cId)
        {
            string sql = @"select IDENTITY(int, 1,1) as Id, b.Name as ProjName, b.ApplyDate as ProjDate, b.Content, b.Location, b.WinnerAmount, 
                            case when cb.WinBid = 1 and cb.Response=1 then '中标' 
                                 when cb.WinBid = 0 and cb.Response=1 then '未中标' 
                                 when cb.Response=2 then '未参加' 
                                 when cb.Response=0 then '未响应'
                                 when cb.Response=1 then '参加' end as Status
                            from CompanyInBid cb inner join Bid b on cb.BidId=b.ID 
                            where cb.CompanyId=" + cId+" order by b.ApplyDate desc";
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.ConvertTableToObj<CompanyBidDetail>(dt);
        }
        public DataTable GetCompanyStat(string cId)
        {
            string sql = @"select COUNT(1) as Total, 
		                        sum(case when cb.WinBid = 1 and cb.Response=1 then 1 else 0 end) as win,
                                sum(case when cb.WinBid = 0 and cb.Response=1 then 1 else 0 end) as nowin,
                                sum(case when cb.Response=2 then 1 else 0 end) as nojoin,
                                sum(case when cb.Response=0 then 1 else 0 end) as noresoponse,
                                sum(case when cb.Response=1 then 1 else 0 end) as attend
                        from CompanyInBid cb inner join Bid b on cb.BidId=b.ID 
                        where cb.CompanyId=" + cId + " order by b.ApplyDate desc";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }

        public DataTable GetBidCompany(string pid)
        {
            string sql = @"select p.Name, d.Name+' '+ui.UserName as Publisher, p.Location, p.ProjType, Convert(varchar(20),b.PublishDate, 23) as PublishDate,
	                            convert(varchar(20),b.ApplyDate,23) as ApplyDate, CONVERT(varchar(20), b.OpenDate ,23) as OpenDate,b.BidingNum, bf.Content
                            from project p inner join bid b on p.Id=b.ProjId
                            inner join BidingFile bf on bf.ProjId=p.Id
                            left join UserInfo ui on ui.id=b.publisherId
							left join DepartmentUser du on du.UserId=ui.ID
                            left join Department d on d.ID=du.DepartmentId
                            where p.id=" + pid;
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }

        public DataTable GetBidApplicationDetail(string bid)
        {
            throw new NotImplementedException();
        }
        public DataTable GetBidApplicationAuditComment(string bid)
        {
            throw new NotImplementedException();
        }
        public DataTable GetBidApplicationTransferInfo(string bid)
        {
            throw new NotImplementedException();
        }

        public DataTable GetBidingApproveDetail(string bid)
        {
            throw new NotImplementedException();
        }

        public DataTable GetBidApplications()
        {
            string sql = @"select p.Id, p.Name, p.Location, bf.Content, d.name+' '+ui.UserName as Publisher, Convert(varchar(20),b.PublishDate, 23) as PublishDate,
	                            convert(varchar(20),b.ApplyDate,23) as ApplyDate, CONVERT(varchar(20), b.OpenDate ,23) as OpenDate, b.Status
                            from project p inner join Bid b on p.Id=b.ProjId
                            inner join BidingFile bf on bf.ProjId=p.Id
                            left join UserInfo ui on b.PublisherId=ui.ID
							left join DepartmentUser du on du.UserId=ui.ID
                            left join Department d on du.DepartmentId=d.ID
                            order by p.Id desc";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
        public DataTable GetBidingApproves(string userid)
        {
            string sql = @"select p.Id, p.Name, p.Location, bf.Content, d.name+' '+ui.UserName as Publisher, Convert(varchar(20),b.PublishDate, 23) as PublishDate,
	                            convert(varchar(20),b.ApplyDate,23) as ApplyDate, CONVERT(varchar(20), b.OpenDate ,23) as OpenDate, b.Status
                            from project p inner join Bid b on p.Id=b.ProjId
                            inner join BidingFile bf on bf.ProjId=p.Id
                            left join UserInfo ui on b.PublisherId=ui.ID
                            left join DepartmentUser du on du.UserId=ui.ID
                            left join Department d on du.DepartmentId=d.ID
                            inner join(
                                select distinct a.ObjId, a.Approved 
                                from vw_AppPLevel a 
                                inner join (select MAX(level) as level,AppProcId, ObjId 
			                                from vw_AppPLevel where AppProcId=3 and Approved=1 group by ObjId, AppProcId
                            ) b on a.AppProcId=b.AppProcId and a.Level>=b.level and a.ObjId=b.ObjId
                            where a.UserId="+ userid + @") a on p.ID=a.ObjId
                            order by p.Id desc;";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
    }
    public class BidCompany
    {
        public int CompanyId;
        public string CompanyName;
        public string Contact;
        public string ContactPhone;
    }
    public class CompanyStat
    {
        public string CompanyName;
        public int JoinTimes;
        public int NoJoinTimes;
        public int WinBids;
        public int NoWinBids;
        public string LastJoinDate;
    }
    public class CompanyBidDetail
    {
        public int Id;
        public string ProjName;
        public string ProjDate;
        public string Content;
        public string Location;
        public string WinnerAmount;
        public string Status;
    }
}
