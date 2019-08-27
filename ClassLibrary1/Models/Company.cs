using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL.Tools;
using System.Data.SqlClient;

namespace DAL.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BusinessType { get; set; }
        public string Referrer { get; set; }
        public string ReferreIDPic { get; set; }
        public string BusinessLicensePic { get; set; }
        public string CreditNo { get; set; }
        public string QualificationLevel { get; set; }
        public string SecurityCertificatePic { get; set; }
        public string SecurityCertificateNo { get; set; }
        public string BusinessScope { get; set; }
        public string RegisteredCapital { get; set; }
        public string CorporateRepresentive { get; set; }
        public string RepPhone { get; set; }
        public string RepIDPic { get; set; }
        public string Contact { get; set; }
        public string ContactPhone { get; set; }
        public string ContactIDPic { get; set; }
        public string ContactAddress { get; set; }
        public string ConstructionContent { get; set; }
        public string Note { get; set; }
        private int status = 1;
        public int Status { get { return status; } set { status = value; } }
        public string AuditDate { get; set; }
        public int AuditStatus { get; set; }
        private int type = 1;
        public int Type {
            get { return type; }
            set { type = value; }
        }
        public int SubmitUserId { get; set; }
    }
    public class CompanyContext
    {
        public List<Company> companies = new List<Company>();

        public CompanyContext()
        {
            
        }

        public string GetMyAudit(string userId, string pageSize, string pageIndex, string cname, string ctype, string roleid)
        {
            if (ctype == "")
                ctype = "1";
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps;
            int endIndex = pi * ps;
            string where = "";
            if (ctype != "")
            {
                where = "a.Approved = " + ctype + @" and ";
            }
            string sql = "";
            if (roleid != "2") {
            sql = @"select top 100 percent * from (
                        select c.id,c.Type,c.Name, c.QualificationLevel, c.RegisteredCapital, bt.name as BusinessType, c.CorporateRepresentative,  
	                        c.Contact, convert(varchar(20), c.AuditDate,23) as AuditDate, c.AuditStatus, case when c.AuditStatus=2 then 4 else a.Approved end as Approved
                        from Company c inner join(
                            select distinct a.ObjId, a.Approved 
                            from vw_AppPLevel a 
                            inner join (select MAX(level) as level,AppProcId, ObjId 
			                            from vw_AppPLevel where AppProcId=1 and Approved=1 group by ObjId, AppProcId
                        ) b on a.AppProcId=b.AppProcId and a.Level>=b.level and a.ObjId=b.ObjId
                        where a.UserId=" + userId + @") a on c.ID=a.ObjId
                        left join CompanyType bt on bt.id=c.BusinessType
                        where " + where + @" c.AuditStatus=1
                        union
                        select c.id,c.Type, c.Name, c.QualificationLevel, c.RegisteredCapital, bt.name as BusinessType, c.CorporateRepresentative,  
	                        c.Contact, convert(varchar(20), c.AuditDate,23) as AuditDate, c.AuditStatus, case when c.AuditStatus=2 then 4 else a.Approved end as Approved
                        from Company c inner join vw_AppPLevel a on c.ID=a.ObjId
                        left join CompanyType bt on bt.id=c.BusinessType
                        where " + where + " a.UserId=" + userId + @" and (a.Approved=3 or a.AppProcId=5) and c.AuditStatus=1) a 
                        where a.Name like '%" + cname + "%'  and a.Approved=" + ctype + @"  order by a.id desc";
            }
            else
            {
                sql = @"select top 100 percent * from (
                        select c.id,c.Type,c.Name, c.QualificationLevel, c.RegisteredCapital, bt.name as BusinessType, c.CorporateRepresentative,  
	                        c.Contact, convert(varchar(20), c.AuditDate,23) as AuditDate, c.AuditStatus, case when c.AuditStatus=2 then 4 else a.Approved end as Approved
                        from Company c inner join(
                            select distinct a.ObjId, a.Approved 
                            from vw_AppPLevel a 
                            inner join (select MAX(level) as level,AppProcId, ObjId 
			                            from vw_AppPLevel where AppProcId=1 and Approved=1 group by ObjId, AppProcId
                        ) b on a.AppProcId=b.AppProcId and a.Level>=b.level and a.ObjId=b.ObjId
                        where a.UserId=" + userId + @") a on c.ID=a.ObjId
                        left join CompanyType bt on bt.id=c.BusinessType
                        where " + where + @" c.AuditStatus=1
                        union
                        select c.id,c.Type, c.Name, c.QualificationLevel, c.RegisteredCapital, bt.name as BusinessType, c.CorporateRepresentative,  
	                        c.Contact, convert(varchar(20), c.AuditDate,23) as AuditDate, c.AuditStatus, case when c.AuditStatus=2 then 4 else a.Approved end as Approved
                        from Company c inner join vw_AppPLevel a on c.ID=a.ObjId
                        left join CompanyType bt on bt.id=c.BusinessType
                        where " + where + " a.UserId=" + userId + @" and (a.Approved=3 or a.AppProcId=5) and c.AuditStatus=1
                        union
                        select c.ID, c.Type, c.Name, c.QualificationLevel, c.RegisteredCapital, ct.Name as BusinessType, c.CorporateRepresentative,
                                c.Contact, convert(varchar(20), c.AuditDate, 23) as AuditDate, c.AuditStatus, 3 as Approved
                        from Company c
                        left join CompanyType ct on ct.ID = c.BusinessType
                        where c.AuditStatus = 3
                        union
                        select c.ID, c.Type, c.Name, c.QualificationLevel, c.RegisteredCapital, ct.Name as BusinessType, c.CorporateRepresentative,
		                        c.Contact, convert(varchar(20), c.AuditDate, 23) as AuditDate, c.AuditStatus, 2 as Approved
                        from Company c
                        inner join vw_AppPLevel vap on c.ID = vap.ObjId and vap.AppProcId in (1, 5) and vap.Approved = 2 and vap.UserId=" + userId + @"
                        left join CompanyType ct on ct.ID = c.BusinessType
                        where c.AuditStatus = 1) a 
                        where " + where + " a.Name like '%" + cname + "%' order by a.id desc";
            }
            DataTable dataTable = DBHelper.GetDataTable(sql);
            DataTable dt = dataTable.Clone();
            int total = dataTable.Rows.Count;
            endIndex = endIndex < total ? endIndex : total;
            for(int i=startIndex;i<endIndex;i++)
            {
                dt.ImportRow(dataTable.Rows[i]);
            }
            int pagecount = (int)Math.Ceiling(decimal.Parse(total.ToString()) / ps);
            string data = JsonHelper.DataTableToJSON(dt).Replace("\r","").Replace("\n","").Replace("	","");
            return "{\"List\":" + data + ", \"total\":" + total + ", \"PageCount\":" + pagecount + ",\"CurrentPage\":" + pageIndex + "}";
        }

        public string GetAllCompanies(string cType, string pageSize, string pageIndex, string cname, string cctype, string roleid)
        {
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps+1;
            int endIndex = pi * ps;
            string where = "";
            if(cctype != "")
            {
                where = " c.BusinessType = " + cctype + " and ";
            }
            if(roleid!="2")
            {
                where += " c.Status=1 and ";
            }
            string sql = @"select identity(int,1,1) as iid,c.id, c.Name, c.QualificationLevel, c.RegisteredCapital, bt.name as BusinessType, c.CorporateRepresentative, 
                                  c.Contact,c.ContactPhone,c.ContactAddress, c.AuditStatus, c.Status, [dbo].[GetProjectDepartmentByUserId](c.SubmitUserId) as refDepartment
                           into #temp1
                           from Company c
                           left join CompanyType bt on bt.id=c.BusinessType
                           where c.AuditStatus=2 and c.Type = " + cType+" and "+where+" c.Name like '%"+cname+@"%' order by c.id desc;
                           select * from #temp1 where iid between " + startIndex + " and " + endIndex + @"
                           select count(1) from #temp1
                           drop table #temp1";
            DataSet ds = DBHelper.GetDataSet(sql);
            string data = JsonHelper.DataTableToJSON(ds.Tables[0]).Replace("\r", "").Replace("\n", "").Replace("	", "");
            string total = ds.Tables[1].Rows[0][0].ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + data + ", \"total\":" + total + ", \"PageCount\":" + pagecount + ",\"CurrentPage\":" + pageIndex + "}";
        }

        public DataTable GetCompany(int id)
        {
            string sql = @"select c.id, c.Name,c.CreditNo,c.CorporateRepresentative, c.RepPhone,c.RegisteredCapital, c.BusinessScope,c.SecurityCertificateNo,
                                  c.Contact,c.ContactPhone, bt.name as BusinessType, c.ContactAddress,c.QualificationLevel, c.ConstructionContent, c.Note,
								  c.ReferreIDPic, c.BusinessLicensePic,c.SecurityCertificatePic,c.RepIDPic,c.ContactIDPic,c.Referre,c.AuditStatus,c.Type
                           from Company c
                           left join CompanyType bt on bt.id=c.BusinessType 
                           where c.id = " + id;
            return DBHelper.GetDataTable(CommandType.Text, sql);
        }
        public DataTable GetCompanyReferee(int id)

        {
            string sql = @"select bd.FileName, bd.FilePath 
                            from BidDocument bd 
                            inner join Company c on bd.ProjId=c.ID and bd.FileType=1 and c.ReferreIDPic=bd.FilePath 
                            where bd.FileType=1 and bd.ProjId=" + id;
            return DBHelper.GetDataTable(sql);
        }
        public DataTable GetWorkHistory(int id)
        {
            string sql = @"select ProjectName,ContractAmount, CONVERT(varchar(20), StartDate,23) as StartDate,
			                    CONVERT(varchar(20),EndDate,23) as EndDate, SettlementAmount, DelayStatus, TestifyFile, FilePath
                          from WorkHistory where CompanyId= " + id;
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetZiZhiPics(int cid)
        {
            string sql = "select ZZName, PicPath, ZZCode from CompanyZiZhiPic where CompanyId="+ cid;
            return DBHelper.GetDataTable(sql);
        }

        public bool UpdateCompany(Company company, string uid)
        {
            string sql = "";
            sql = "select RoleId from UserInfo where id=" + uid;
            string roleid = DBHelper.ExecuteScalar(sql);
            // 
            if(roleid=="2")
            {
                company.AuditStatus = 2;
            }
            sql = "update Company set Name=N'" + company.Name + "', CreditNo=N'" + company.CreditNo + "', RegisteredCapital=" + company.RegisteredCapital + ", " +
                "BusinessType=N'" + company.BusinessType + "', BusinessScope=N'" + company.BusinessScope + "', QualificationLevel=N'" + company.QualificationLevel + "', " +
                "SecurityCertificateNo=N'" + company.SecurityCertificateNo + "', Status=" + company.Status + ", " +
                " Referre = N'" + company.Referrer + "',  AuditStatus=" +company.AuditStatus+ " where id=" + company.Id;
            int i = DBHelper.ExecuteNonQuery(CommandType.Text, sql);

            // 非管理员修改要重新走审批流程
            if (roleid != "2")
            {
                if (company.AuditStatus == 1)
                {
                    if (company.Type == 1)
                        // 名录内企业
                        CreateApproveProcess(company.Id.ToString(), uid, "5");
                    else
                        // 名录外企业
                        CreateApproveProcess(company.Id.ToString(), uid, "1");

                    Log l = new Log();
                    l.OperType = OperateType.Create;
                    l.UserId = uid;
                    l.Description = "创建公司" + company.Type == "1" ? "名录内" : "名录外" + " - " + company.Name;
                    LogContext.WriteLog(l);
                }
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        public string CreateCompany(Company company, string uid)
        {
            string sql = "";
            sql += @"insert into Company(Name, CreditNo, RegisteredCapital, BusinessType, BusinessScope, QualificationLevel,  
                                               SecurityCertificateNo, CorporateRepresentative, RepPhone, 
                                               Contact, ContactPhone, ContactAddress, ConstructionContent, Note, 
                                               Status, Type, Referre,AuditDate, AuditStatus, SubmitUserId, CreateDate)" +
            "values(N'" + company.Name + "', N'" + company.CreditNo + "', " + company.RegisteredCapital + ", " +
            "N'" + company.BusinessType + "', N'" + company.BusinessScope + "', N'" + company.QualificationLevel + "', " +
            "'" + company.SecurityCertificateNo + "', N'" + company.CorporateRepresentive + "', '" + company.RepPhone + "', " +
            "N'" + company.Contact + "', '" + company.ContactPhone + "', N'" + company.ContactAddress + "', " +
            "N'" + company.ConstructionContent + "', N'" + company.Note + "', " + company.Status + ", " +
            company.Type + ", N'" + company.Referrer + "', getdate(), " + company.AuditStatus + ", " + uid + @", getdate());";
            sql += "select max(id) from Company;";
            string i = DBHelper.ExecuteScalar(CommandType.Text, sql);

            sql = @"declare @did int
                    select @did=ID from Department where ProjectDp=1 and ID in (
                        select PID from Department where ID in (select du.departmentid from DepartmentUser du where du.UserId=" + uid + @"))
                    update Company set DepartmentID = @did where ID=" + i;
            DBHelper.ExecuteNonQuery(sql);

            if (company.AuditStatus == 1)
            {
                if (company.Type == 1)
                    // 名录内企业
                    CreateApproveProcess(i, uid, "5");
                else
                    // 名录外企业
                    CreateApproveProcess(i, uid, "1");

                Log l = new Log();
                l.OperType = OperateType.Create;
                l.UserId = uid;
                l.Description = "创建公司"+ company.Type =="1"?"名录内":"名录外"+ " - "+company.Name;
                LogContext.WriteLog(l);
            }
            return i;
        }
        public void UpdateCompanyPics(Company company)
        {
            string sql = @"declare @refpath nvarchar(400);
                           declare @blpath nvarchar(400);
                           declare @scpath nvarchar(400);
                           declare @ripath nvarchar(400);
                           declare @cipath nvarchar(400);";

            if (company.ReferreIDPic == "")
                sql += " set @refpath=''; ";
            else
                sql += " select @refpath = FilePath from BidDocument where FilePath like '%" + company.ReferreIDPic + "%'; ";

            if (company.BusinessLicensePic == "")
                sql += " set @blpath=''; ";
            else
                sql += " select @blpath = FilePath from BidDocument where FilePath like '%" + company.BusinessLicensePic + "%'; ";

            if (company.SecurityCertificatePic == "")
                sql += " set @scpath=''; ";
            else
                sql += " select @scpath = FilePath from BidDocument where FilePath like '%" + company.SecurityCertificatePic + "%'; ";

            if (company.RepIDPic == "")
                sql += " set @ripath=''; ";
            else
                sql += " select @ripath = FilePath from BidDocument where FilePath like '%" + company.RepIDPic + "%'; ";

            if (company.ContactIDPic == "")
                sql += " set @cipath=''; ";
            else
                sql += " select @cipath = FilePath from BidDocument where FilePath like '%" + company.ContactIDPic + "%'; ";

            sql += "update company set BusinessLicensePic=@blpath,ContactIDPic=@cipath,ReferreIDPic=@refpath, RepIDPic=@ripath, SecurityCertificatePic=@scpath where id="+company.Id+"; ";

            if (company.ReferreIDPic != "")
            {
                sql += "update BidDocument set ProjId=" + company.Id + " where FilePath like '%" + company.ReferreIDPic + "%'; ";
            }
            if (company.BusinessLicensePic != "")
                sql += "update BidDocument set ProjId=" + company.Id + " where FilePath like '%" + company.BusinessLicensePic + "%'; ";
            if (company.SecurityCertificatePic != "")
                sql += "update BidDocument set ProjId=" + company.Id + " where FilePath like '%" + company.SecurityCertificatePic + "%'; ";
            if (company.RepIDPic != "")
                sql += "update BidDocument set ProjId=" + company.Id + " where FilePath like '%" + company.RepIDPic + "%'; ";
            if (company.ContactIDPic != "")
                sql += "update BidDocument set ProjId=" + company.Id + " where FilePath like '%" + company.ContactIDPic + "%'; ";
            DBHelper.ExecuteNonQuery(sql);
        }

        public bool ToggleCompany(string id, string status)
        {
            string sql = "update Company set status=" + status + " where id=" + id;
            int i = DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            if (i > 0)
                return true;
            else
                return false;
        }

        public bool DeleteCompany(string id)
        {
            string sql = @"delete company where id in ("+id+")";
            int i = DBHelper.ExecuteNonQuery(CommandType.Text,sql);
            if (i > 0)
                return true;
            else
                return false;
        }

        public string GetMyRecommend(string userid, string pageSize, string pageIndex, string cname, string ctype)
        {
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps+1;
            int endIndex = pi * ps;
            string where = "";
            if (ctype != "")
            {
                where = "c.AuditStatus = " + ctype + @" and ";
            }
            string sql = @"select ID from Department where ProjectDp=1 and ID in (
	                            select PID from Department where ID in (select du.departmentid from DepartmentUser du where du.UserId="+userid+"))";
            string did = "";
            DataTable dataTable = DBHelper.GetDataTable(sql);
            for(int i=0;i<dataTable.Rows.Count;i++)
            {
                did += dataTable.Rows[i][0].ToString() + ",";
            }
            if (did.Length > 0)
                did = did.Substring(0, did.Length - 1);
            sql = @"select identity(int,1,1) as iid, c.id*1 as id, c.Name, c.QualificationLevel, c.RegisteredCapital, bt.Name as BusinessType, c.CorporateRepresentative, c.Contact, 
	                            convert(varchar(20),c.AuditDate,23) as AuditDate, c.AuditStatus
                            into #temp1
                            from Company c
                            left join CompanyType bt on bt.id=c.BusinessType
                            where c.status!=-1 and c.DepartmentID in (" + did + ") and " + where + @" c.Name like '%" + cname+@"%' order by c.id desc
                            select * from #temp1 where iid between " + startIndex + " and " + endIndex + @"
                            select count(1) from #temp1
                            drop table #temp1";
            DataSet ds = DBHelper.GetDataSet(sql);
            string data = JsonHelper.DataTableToJSON(ds.Tables[0]).Replace("\r", "").Replace("\n", "").Replace("	", "");
            string total = ds.Tables[1].Rows[0][0].ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + data + ", \"total\":" + total + ", \"PageCount\":" + pagecount + ",\"CurrentPage\":" + pageIndex + "}";
        }
        

        private void CreateApproveProcess(string cid, string uid, string apid)
        {
            SqlParameter[] paras = new SqlParameter[3];
            paras[0] = new SqlParameter("@uid",uid);
            paras[1] = new SqlParameter("@objid",cid);
            paras[2] = new SqlParameter("@apid", apid);
            DBHelper.ExecuteSP("CreateApproveProcessing", paras);
        }

        public string CheckCompanyNameUsed(string cName)
        {
            string sql = "select count(1) from Company where Name = '" + cName + "' and status<>-1;";
            return DBHelper.ExecuteScalar(sql);
        }

        public DataTable GetCompanyPics(string id)
        {
            string sql = "select FilePath, FileName from BidDocument where ProjId="+id+" and FileType=1 ";
            return DBHelper.GetDataTable(sql);
        }

        public string CheckUpdateCompanyNameUsed(string cName, string cid)
        {
            string sql = "select count(1) from Company where Name = '" + cName + "' and status<>-1 and Id<>"+cid;
            return DBHelper.ExecuteScalar(sql);
        }

        public void DisableCompany(string id)
        {
            string sql = "update Company set status=-1 where Id=" + id;
            DBHelper.ExecuteNonQuery(sql);
        }

        public void AppendCompanyInfo(Company company)
        {
            string sql = "update Company set CorporateRepresentative=N'" + company.CorporateRepresentive+"', RepPhone='"+company.RepPhone+"', Contact=N'"+company.Contact+"', ContactPhone='"+company.ContactPhone+"',ContactAddress='"+company.ContactAddress+"',ConstructionContent=N'"+company.ConstructionContent+"',Note=N'"+company.Note+"' where Id=" + company.Id;
            DBHelper.ExecuteNonQuery(sql);
        }

        public void SubmitCompany(string cid, string type, string uid)
        {
            string sql = "update Company set AuditStatus=1 where Id="+cid;
            DBHelper.ExecuteNonQuery(sql);
            if (type == "1")
                // 名录内企业
                CreateApproveProcess(cid, uid, "5");
            else
                // 名录外企业
                CreateApproveProcess(cid, uid, "1");

            Log l = new Log();
            l.OperType = OperateType.Create;
            l.UserId = uid;
            l.Description = "创建公司" + type == "1" ? "名录内" : "名录外" + " - " + cid;
            LogContext.WriteLog(l);
        }
    }
}
