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
        public int Status { get; set; }
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

        public DataTable GetMyAudit(string userId)
        {
            string sql = @"select c.id, c.Name, c.QualificationLevel, c.RegisteredCapital, bt.name as BusinessType, c.CorporateRepresentative,  
	                            c.Contact, convert(varchar(20), c.AuditDate,23) as AuditDate, c.AuditStatus, a.Approved 
                            from Company c inner join(
                                select distinct a.ObjId, a.Approved 
                                from vw_AppPLevel a 
                                inner join (select MAX(level) as level,AppProcId, ObjId 
			                                from vw_AppPLevel where AppProcId=1 and Approved=1 group by ObjId, AppProcId
                            ) b on a.AppProcId=b.AppProcId and a.Level>=b.level and a.ObjId=b.ObjId
                            where a.UserId=" + userId + @") a on c.ID=a.ObjId
                            left join BusinessType bt on bt.id=c.BusinessType
                            where c.AuditStatus<>3
                            union
                            select c.id, c.Name, c.QualificationLevel, c.RegisteredCapital, bt.name as BusinessType, c.CorporateRepresentative,  
	                            c.Contact, convert(varchar(20), c.AuditDate,23) as AuditDate, c.AuditStatus, a.Approved 
                            from Company c inner join vw_AppPLevel a on c.ID=a.ObjId and a.AppProcId=1
                            left join BusinessType bt on bt.id=c.BusinessType
                            where a.UserId=" + userId + " and a.Approved=3";
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetAllCompanies(string cType)
        {
            string sql = @"select c.id, c.Name, c.QualificationLevel, c.RegisteredCapital, bt.name as BusinessType, c.CorporateRepresentative, 
                                  c.Contact,c.ContactPhone,c.ContactAddress, c.AuditStatus, c.Status 
                           from Company c
                           left join BusinessType bt on bt.id=c.BusinessType
                           where c.AuditStatus=2 and c.Type = " + cType + " order by c.Name;";
            DataTable dt = DBHelper.GetDataTable(CommandType.Text, sql);
            return dt;
        }

        public DataTable GetCompany(int id)
        {
            string sql = @"select c.id, c.Name,c.CreditNo,c.CorporateRepresentative, c.RepPhone,c.RegisteredCapital, c.BusinessScope,c.SecurityCertificateNo,
                                  c.Contact,c.ContactPhone, bt.name as BusinessType, c.ContactAddress,c.QualificationLevel, c.ConstructionContent, c.Note,
								  c.ReferreIDPic, c.BusinessLicensePic,c.SecurityCertificatePic,c.RepIDPic,c.ContactIDPic,c.Referre
                           from Company c
                           left join BusinessType bt on bt.id=c.BusinessType 
                           where c.id = " + id;
            return DBHelper.GetDataTable(CommandType.Text, sql);
        }

        public DataTable GetWorkHistory(int id)
        {
            string sql = @"select ProjectName,ContractAmount, CONVERT(varchar(20), StartDate,23) as StartDate,
			                    CONVERT(varchar(20),EndDate,23) as EndDate, SettlementAmount, DelayStatus
                          from WorkHistory where CompanyId= " + id;
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetZiZhiPics(int cid)
        {
            string sql = "select ZZName, PicPath, ZZCode from CompanyZiZhiPic where CompanyId="+ cid;
            return DBHelper.GetDataTable(sql);
        }

        public bool UpdateCompany(Company company)
        {
            string sql = "update Company set Name='" + company.Name + "', CreditNo='" + company.CreditNo + "', RegisteredCapital=" + company.RegisteredCapital + ", " +
                "BusinessType='" + company.BusinessType + "', BusinessScope='" + company.BusinessScope + "', QualificationLevel='" + company.QualificationLevel + "', " +
                "SecurityCertificateNo='" + company.SecurityCertificateNo + "', CorporateRepresentive='" + company.CorporateRepresentive + "', RepPhone='" + company.RepPhone + "', " +
                "Contact = '" + company.Contact + "', ContactPhone = '" + company.ContactPhone + "', ContactAddress = '" + company.ContactPhone + "', " +
                "ConstructionContent = '" + company.ConstructionContent + "', Note'" + company.Note + "', Status=" + company.Status + ", AuditDate='" + company.AuditDate + "'" +
                "Type=" + company.Type + ", Referrer = "+company.Referrer + " where id=" + company.Id;
            int i = DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            if (i > 0)
                return true;
            else
                return false;
        }
        public string CreateCompany(Company company, string uid)
        {
            string sql = @"insert into Company(Name, CreditNo, RegisteredCapital, BusinessType, BusinessScope, QualificationLevel,  
                                               SecurityCertificateNo, CorporateRepresentative, RepPhone, 
                                               Contact, ContactPhone, ContactAddress, ConstructionContent, Note, 
                                               Status, Type, Referre,AuditDate, AuditStatus, SubmitUserId)" +
                "values(N'" + company.Name + "', '" + company.CreditNo + "', " + company.RegisteredCapital + ", " +
                "N'" + company.BusinessType + "', N'" + company.BusinessScope + "', N'" + company.QualificationLevel + "', " +
                "'" + company.SecurityCertificateNo + "', N'" + company.CorporateRepresentive + "', '" + company.RepPhone + "', " +
                "N'" + company.Contact + "', '" + company.ContactPhone + "', N'" + company.ContactAddress + "', " +
                "N'" + company.ConstructionContent + "', N'" + company.Note + "', " + company.Status + ", " + 
                company.Type + ", N'"+company.Referrer + "', getdate(), "+company.AuditStatus+", "+ uid + ");";
            sql += "select max(id) from Company;";
            string i = DBHelper.ExecuteScalar(CommandType.Text, sql);
            CreateApproveProcess(i, uid);
            return i;
        }
        public bool UpdateCompanyPics(Company company)
        {
            string sql = "update Company set ReferreIDPic='"+company.ReferreIDPic+
                                        "', BusinessLicensePic='"+company.BusinessLicensePic+
                                        "', SecurityCertificatePic='"+company.SecurityCertificatePic+
                                        "', RepIDPic='"+company.RepIDPic+
                                        "',ContactIDPic='"+company.ContactIDPic+
                                        "' where ID="+company.Id;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            return false;
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

        public bool DeleteCompany(Company company)
        {
            string sql = @"delete company where id"+company.Id;
            int i = DBHelper.ExecuteNonQuery(CommandType.Text,sql);
            if (i > 0)
                return true;
            else
                return false;
        }

        public DataTable GetMyRecommend(string userid)
        {
            string sql = @"select id, Name, QualificationLevel, RegisteredCapital, BusinessType, CorporateRepresentative, Contact, 
	                            convert(varchar(20),AuditDate,23) as AuditDate, AuditStatus
                            from Company where SubmitUserId=" + userid;
            return DBHelper.GetDataTable(sql);
        }
        

        private void CreateApproveProcess(string cid, string uid)
        {
            string sql = @"insert into AppProcessing(AppProcId, ObjId,Approved, Comment, DealDatetime,DUGUID)
                            select APID, " + cid + ", 1, null, null, DUGUID from APDetail where APID = 1";
            SqlParameter[] paras = new SqlParameter[3];
            paras[0] = new SqlParameter("@uid",uid);
            paras[1] = new SqlParameter("@objid",cid);
            paras[2] = new SqlParameter("@apid", 1);
            DBHelper.ExecuteSP("CreateApproveProcessing", paras);
        }
    }
}
