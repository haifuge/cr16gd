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
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string ApplyDate { get; set; }
        public string OpenDate { get; set; }
        public int BidingNum { get; set; }
        public string ProjDescription { get; set; }
        public string DocName { get; set; }
        public string DocPath { get; set; }
        public int WinnerCompanyId { get; set; }
        public string WinnerCompanyName { get; set; }
        public int WinnerAmount { get; set; }
        public int PublisherId { get; set; }
        public string Publisher { get; set; }
        public string PublishDate { get; set; }
        public string Status { get; set; }
    }

    public class BidContext
    {
        public DataTable GetAllBids()
        {
            string sql = @"select ID, Name, Location, Content, Publisher, convert(varchar(20), PublishDate, 23) as PublishDate,
                            CONVERT(varchar(20), applydate, 23) as ApplyDate, CONVERT(varchar(20), OpenDate, 23) as OpenDate 
                            from Bid order by ID desc;";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }

        public bool AddBid(Bid bid)
        {
            string sql = @"insert into Bid(Name, Type, Content, ApplyDate, OpenDate, BidingNum, Location, ProjDescription, 
                                           DocName, DocPath, WinnerAmount, PublisherId, Publisher, PublishDate)
                            values('" + bid.Name+"', '"+bid.Type+"', '"+bid.Content+"', '"+bid.ApplyDate+"', '"+bid.OpenDate+"', "+bid.BidingNum+
                            ", '"+bid.Location+"', '"+bid.ProjDescription+"', '"+bid.DocName+"', '"+bid.DocPath+"', "+bid.WinnerAmount+
                            ", "+bid.PublisherId+", '"+bid.Publisher+"', '"+bid.PublishDate+"')";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public void RemoveBidingCompany(string bid, string cid)
        {
            throw new NotImplementedException();
        }

        public bool AddCompanyToBid(string bid, string cid)
        {
            throw new NotImplementedException();
        }

        public bool ProcessBidApplication(string operation, string comment)
        {
            throw new NotImplementedException();
        }

        public DataTable GetBidingCompanys(string bid)
        {
            string sql = "select CompanyName,CompanyResponse from BidingCompany where BidId="+bid;
            return DBHelper.GetDataTable(sql);
        }

        public bool UpdateBid(Bid bid)
        {
            string sql = "update Bid set BidName='"+bid.Name+"', Type='"+bid.Type+"', Content='"+bid.Content+"', ApplyDate='"+bid.ApplyDate+
                "',OpenDate='"+bid.OpenDate+ "', BidingNum=" + bid.BidingNum+",Location='"+bid.Location+"', PorjDescription='"+bid.ProjDescription+
                "', DocName='"+bid.DocName+"', DocPath='"+bid.DocPath+"' where ID="+bid.ID;
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

        public bool AddBidingCompany(int bid, int cid, string cName)
        {
            string sql= "insert into CompanyInBid values("+bid+", "+cid+",'"+cName+"', 0, 0)";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool RemoveBidingCompany(int bid, int cid)
        {
            string sql = "delete CompanyInBid where BidId = " + bid + " and CompanyId = " + cid;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool SetWinBidCompany(int bid, int cid)
        {
            string sql = "update CompanyInBid set WinBid=1 where BidId = " + bid + " and CompanyId = " + cid;
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

        public DataTable GetBidCompany(string bid)
        {
            string sql = @"select ID, Name, Location,Content, Publisher, convert(varchar(20),PublishDate, 23) as PublishDate,ProjDescription,
                                CONVERT(varchar(20), applydate, 23) as ApplyDate, CONVERT(varchar(20), OpenDate, 23) as OpenDate, Type,BidingNum
                            from Bid where ID=" + bid;
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
            string sql = @"select ID, Name, Location, Content, Publisher, convert(varchar(20), PublishDate, 23) as PublishDate,
                            CONVERT(varchar(20), applydate, 23) as ApplyDate, CONVERT(varchar(20), OpenDate, 23) as OpenDate, Status
                            from Bid order by ID desc;";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
        public DataTable GetBidingApproves()
        {
            string sql = @"select ID, Name, Location, Content, Publisher, convert(varchar(20), PublishDate, 23) as PublishDate,
                            CONVERT(varchar(20), applydate, 23) as ApplyDate, CONVERT(varchar(20), OpenDate, 23) as OpenDate, Status
                            from Bid order by ID desc;";
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
