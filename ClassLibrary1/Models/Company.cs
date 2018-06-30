﻿using System;
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
        private int type = 1;
        public int Type {
            get { return type; }
            set { type = value; }
        }
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
            string sql = @"select ProjectName,ContractAmount, CONVERT(varchar(20), StartDate,23) as StartDate,
			                    CONVERT(varchar(20),EndDate,23) as EndDate, SettlementAmount, DelayStatus
                          from WorkHistory where CompanyId= " + id;
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
        public int CreateCompany(Company company)
        {
            string sql = @"insert into Company(Name, CreditNo, RegisteredCapital, BusinessType, BusinessScope, QualificationLevel,  
                                               SecurityCertificateNo, CorporateRepresentative, RepPhone, 
                                               Contact, ContactPhone, ContactAddress, ConstructionContent, Note, 
                                               Status, Type, Referre)" +
                "values('" + company.Name + "', '" + company.CreditNo + "', " + company.RegisteredCapital + ", " +
                "'" + company.BusinessType + "', '" + company.BusinessScope + "', '" + company.QualificationLevel + "', " +
                "'" + company.SecurityCertificateNo + "', '" + company.CorporateRepresentive + "', '" + company.RepPhone + "', " +
                "'" + company.Contact + "', '" + company.ContactPhone + "', '" + company.ContactAddress + "', " +
                "'" + company.ConstructionContent + "', '" + company.Note + "', " + company.Status + ", " + 
                company.Type + ", '"+company.Referrer + "');";
            sql += "select max(id) from Company;";
            int i = int.Parse(DBHelper.ExecuteScalar(CommandType.Text, sql));
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
                            from Company where Referre='" + userName+"'";
            return DBHelper.GetDataTable(sql);
        }
    }
}
