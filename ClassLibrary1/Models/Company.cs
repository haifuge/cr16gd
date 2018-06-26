using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL.Tools;

namespace DAL.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BusinessType { get; set; }
        public string Referrer { get; set; }
        public string CreditNo { get; set; }
        public string QualificationLevel { get; set; }
        public string SecurityCertificateNo { get; set; }
        public string BusinessScope { get; set; }
        public int RegisteredCapital { get; set; }
        public string CorporateRepresentive { get; set; }
        public string RepPhone { get; set; }
        public string Contact { get; set; }
        public string ContactPhone { get; set; }
        public string ContactAddress { get; set; }
        public string ConstructionContent { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public string AuditDate { get; set; }
        public int Type { get; set; }
    }
    public class CompanyContext
    {
        public List<Company> companies = new List<Company>();

        public CompanyContext()
        {
            
        }

        public DataTable GetMyAudit(string userId)
        {
            string sql = @"select c.id, c.Name, c.QualificationLevel, c.RegisteredCapital, c.BusinessType, c.CorporateRepresentative,  
	                            c.Contact, convert(varchar(20), c.AuditDate,23) as AuditDate, c.AuditStatus
                            from Company c, AppProcessing ap 
                            where c.ID=ap.ObjId and ap.AppProcId=1 and ap.UserId=" + userId;
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetAllCompanies(string cType)
        {
            string sql = @"select id, Name, QualificationLevel, RegisteredCapital, BusinessType, CorporateRepresentative, 
                                  Contact,ContactPhone,ContactAddress, AuditStatus 
                           from Company where Type = " + cType + " order by Name;";
            DataTable dt = DBHelper.GetDataTable(CommandType.Text, sql);
            return dt;
        }

        public DataTable GetCompany(int id)
        {
            string sql = @"select id, Name,CreditNo,CorporateRepresentative, RepPhone,RegisteredCapital, BusinessScope,
                                  Contact,ContactPhone, BusinessType, ContactAddress,QualificationLevel, ConstructionContent, Note
                           from Company where id = " + id;
            return DBHelper.GetDataTable(CommandType.Text, sql);
        }

        public DataTable GetWorkHistory(int id)
        {
            string sql = @"select ProjectId,ProjectName,ContractAmount, CONVERT(varchar(20), StartDate,23) as StartDate,
			                    CONVERT(varchar(20),EndDate,23) as EndDate, SettlementAmount, DelayStatus
                          from WorkHistory where CompanyId= " + id+" order by ProjectId";
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
        public bool CreateCompany(Company company)
        {
            string sql = @"insert into Company(Name, CreditNo, RegisteredCapital, BusinessType, BusinessScope, QualificationLevel,  
                                               SecurityCertificateNo, CorporateRepresentive, RepPhone, 
                                               Contact, ContactPhone, ContactAddress, ConstructionContent, Note, 
                                               Status, AuditDate, Type, Referrer)" +
                "values('" + company.Name + "', '" + company.CreditNo + "', " + company.RegisteredCapital + ", " +
                "'" + company.BusinessType + "', '" + company.BusinessScope + "', '" + company.QualificationLevel + "', " +
                "'" + company.SecurityCertificateNo + "', '" + company.CorporateRepresentive + "', '" + company.RepPhone + "', " +
                "'" + company.Contact + "', '" + company.ContactPhone + "', '" + company.ContactAddress + "', " +
                "'" + company.ConstructionContent + "', '" + company.Note + "', " + company.Status + ", '" + company.AuditDate + "', " + 
                company.Type + ", "+company.Referrer + ")";
            int i = DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            if (i > 0)
                return true;
            else
                return false;
        }

        public bool ToggleCompany(int id, int status)
        {
            string sql = "update Company set status='" + status + "where id=" + id;
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

        public DataTable GetMyRecommend(string userName)
        {
            string sql = @"select id, Name, QualificationLevel, RegisteredCapital, BusinessType, CorporateRepresentative, Contact, 
	                            convert(varchar(20),AuditDate,23) as AuditDate, AuditStatus
                            from Company where Referrer='" + userName+"'";
            return DBHelper.GetDataTable(sql);
        }
    }
}
