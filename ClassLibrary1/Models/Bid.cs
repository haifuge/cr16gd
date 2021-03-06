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
        public string GetAllBids(string pageSize, string pageIndex, string pname, string uid)
        {
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            SqlParameter[] paras = new SqlParameter[4];
            paras[0] = new SqlParameter("@uid", uid);
            paras[1] = new SqlParameter("@pageSize", pageSize);
            paras[2] = new SqlParameter("@pageIndex", pageIndex);
            paras[3] = new SqlParameter("@pname", pname);
            DataSet ds = DBHelper.ExecuteDataset(DBHelper.GetConnection(), "GetBidingByUserId", paras);
            DataTable dt = ds.Tables[0];
            string data = JsonHelper.DataTableToJSON(dt);
            string total = ds.Tables[1].Rows[0][0].ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + data + ", \"total\":" + total + ", \"PageCount\":" + pagecount + ",\"CurrentPage\":" + pageIndex + "}";
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
            string sql = @"select c.id, c.Name, bc.CompanyResponse 
                            from BidingCompany bc 
                            inner join Company c on bc.CompanyId=c.ID 
                            where bc.ProjId=" + pid;
            return DBHelper.GetDataTable(sql);
        }

        public bool UpdateBid(Bid bid)
        {
            string sql = "update Bid set ApplyDate='"+bid.ApplyDate+"', OpenDate='"+bid.OpenDate+"', BidingNum="+bid.BidingNum+",Status=1 where ProjId="+bid.ProjId;
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

        public string GetCompanyStats(string pageSize, string pageIndex, string pname, string bdate, string edate)
        {
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps + 1;
            int endIndex = pi * ps;
            string date_where = "";
            if (!string.IsNullOrEmpty(bdate))
            {
                date_where += " and b.PublishDate>'"+bdate+"' ";
            }

            if (!string.IsNullOrEmpty(edate))
            {
                date_where += " and b.PublishDate<'"+edate+"' ";
            }

            string sql = @"select identity(int,1,1) as iid, c.id, c.Name, SUM(isnull(bc.Biding,0)) as JoinTimes, sum(case when bc.CompanyResponse=0 then 1 else 0 end) as NoJoinTimes,
		                            SUM(isnull(bc.Win,0)) as WinBids, sum(case when bc.Win=0 then 1 else 0 end) as NoWinBids, convert(varchar(20),max(b.PublishDate),23) as LastJoinDate 
                            into #temp1
                            from Company c
                            inner join BidingCompany bc on c.ID=bc.CompanyId
                            inner join Bid b on bc.ProjId=b.ProjId
                            where c.Name like '%" + pname+ @"%' and c.AuditStatus=2 "+date_where+@"
                            group by c.Name, c.id
                            order by c.Name
                            select * from #temp1 where iid between " + startIndex + " and " + endIndex + @"
                            select count(1) from #temp1 
                            drop table #temp1";
            DataSet ds = DBHelper.GetDataSet(sql);
            DataTable dt = ds.Tables[0];
            string total = ds.Tables[1].Rows[0][0].ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + JsonHelper.DataTableToJSON(dt) + ", \"total\":" + total + ", \"pagecount\":" + pagecount + "}";
        }
        public DataTable GetCompanyBidDetail(string cId)
        {
            string sql = @"select c.Name, p.Name, convert(varchar(20),b.OpenDate, 23) as OpenDate, p.ProDescription, p.Location, case when bc.win=1 and bc.SecondPrice is NULL then bc.FirstPrice when bc.win=1 then bc.SecondPrice else 0 end as Amount, 
	                            case when bc.Win=1 then '中标' when bc.CompanyResponse=2 then '不参加' when bc.CompanyResponse=0 then '未响应' when bc.Win=0 and bc.CompanyResponse=1 then '参加' end as Status
                            from Company c
                            inner join BidingCompany bc  on c.ID=bc.CompanyId
                            inner join Bid b on bc.ProjId=b.ProjId 
                            inner join Project p on bc.ProjId=p.Id
                            where c.Id = " + cId+" order by b.OpenDate desc";
            return DBHelper.GetDataTable(sql);
        }
        public DataTable GetCompanyStatById(string cId, string bd, string ed)
        {
            string date_where = "";
            if (!string.IsNullOrEmpty(bd))
            {
                date_where += " and b.PublishDate>'" + bd + "' ";
            }

            if (!string.IsNullOrEmpty(ed))
            {
                date_where += " and b.PublishDate<'" + ed + "' ";
            }
            string sql = @"select c.Name, count(1) as Total, SUM(case when bc.CompanyResponse=1 then 1 else 0 end) as JoinBiding, SUM(case when bc.CompanyResponse=2 then 1 else 0 end) as NoJoin, 
	                            SUM(case when bc.CompanyResponse=0 then 1 else 0 end) as NoResponse, SUM(case when bc.biding=1 and bc.win=0 then 1 else 0 end) as NoWin,SUM(isnull(bc.win,0)) as Win, 
	                            SUM(case when bc.win=1 and bc.SecondPrice is NULL then bc.FirstPrice when bc.win=1 then bc.SecondPrice else 0 end) as TotalAmount
                            from Company c
                            inner join BidingCompany bc  on c.ID=bc.CompanyId
                            inner join Bid b on bc.ProjId=b.ProjId 
                            inner join Project p on bc.ProjId=p.Id
                            where c.Id = " + cId + date_where + " group by c.Name";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }

        public DataTable GetBidDetail(string pid)
        {
            string sql = @"select p.Name, d.Name+' '+ui.UserName as Publisher,ui.id as uid, p.Location,bt.Name as ProjType, Convert(varchar(20),b.PublishDate, 23) as PublishDate,
	                            convert(varchar(20),b.ApplyDate,23) as ApplyDate, CONVERT(varchar(20), b.OpenDate ,23) as OpenDate,b.BidingNum, 
                                bf.Content, p.ProDescription, b.Status
                            from project p inner join bid b on p.Id=b.ProjId
                            inner join BidingFile bf on bf.ProjId=p.Id
                            left join CompanyType bt on bt.id=p.ProjType
                            left join UserInfo ui on ui.id=b.publisherId
							left join DepartmentUser du on du.UserId=ui.ID and du.Status=1
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

        public string GetBidApplications(string pageSize, string pageIndex)
        {
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps + 1;
            int endIndex = pi * ps;
            string sql = @" select identity(int,1,1) as iid, a.* into #temp1 from (
                            select distinct p.Id as Id, p.Name, p.Location, bf.Content, d.name+' '+ui.UserName as Publisher, Convert(varchar(20),b.PublishDate, 23) as PublishDate,
	                            convert(varchar(20),b.ApplyDate,23) as ApplyDate, CONVERT(varchar(20), b.OpenDate ,23) as OpenDate, b.Status
                            from project p inner join Bid b on p.Id=b.ProjId
                            inner join BidingFile bf on bf.ProjId=p.Id
                            left join UserInfo ui on b.PublisherId=ui.ID
							left join DepartmentUser du on du.UserId=ui.ID and du.Status=1
                            left join Department d on du.DepartmentId=d.ID
                            ) a order by a.Id desc
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
        public string GetBidingApproves(string userid, string pageSize, string pageIndex, string pname, string status)
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
            string sql = @"select identity(int,1,1) as iid, a.* 
                            into #temp1 
                            from( select distinct p.Id*1 as Id, p.Name, p.Location, bf.Content, d.name+' '+ui.UserName as Publisher, Convert(varchar(20),b.PublishDate, 23) as PublishDate,
	                                convert(varchar(20),b.ApplyDate,23) as ApplyDate, CONVERT(varchar(20), b.OpenDate ,23) as OpenDate, a.Approved as Status
                                from project p 
                                inner join Bid b on p.Id=b.ProjId and b.Status<>0
                                inner join BidingFile bf on bf.ProjId=p.Id
                                left join UserInfo ui on b.PublisherId=ui.ID
                                left join DepartmentUser du on du.UserId=ui.ID and du.Status=1
                                left join Department d on du.DepartmentId=d.ID
                                inner join(
                                    select distinct a.ObjId, a.Approved 
                                    from vw_AppPLevel a 
                                    inner join (select MAX(level) as level,AppProcId, ObjId 
			                                    from vw_AppPLevel where AppProcId=3 and Approved=1 group by ObjId, AppProcId
                                ) b on a.AppProcId=b.AppProcId and a.Level>=b.level and a.ObjId=b.ObjId
                                where a.UserId=" + userid + @") a on p.ID=a.ObjId "+where+@" 
                            ) a
                            order by a.Id desc
                            select * from #temp1 where iid between " + startIndex + " and " + endIndex + @"
                            select count(1) from #temp1
                            drop table #temp1";
            DataSet ds = DBHelper.GetDataSet(sql);
            string data = JsonHelper.DataTableToJSON(ds.Tables[0]);
            data = data.Replace(@"\n", "<br/>").Replace(@"\t", "    ").Replace(@"\r", "    ").Replace("	","    ");
            string total = ds.Tables[1].Rows[0][0].ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            DataTable dt = DBHelper.GetDataTable(CommandType.Text, sql);
            return "{\"List\":" + data + ", \"total\":" + total + ", \"PageCount\":" + pagecount + ",\"CurrentPage\":" + pageIndex + "}";
        }
        public string GetCompanyResponse(string pid, string cid)
        {
            string sql = "select CompanyResponse from BidingCompany where ProjId="+pid+" and CompanyId="+cid;
            return DBHelper.ExecuteScalar(sql);
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
