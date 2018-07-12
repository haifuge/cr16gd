﻿using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class SystemSetup
    {
        public DataTable GetApproveProcesses()
        {
            string sql = "select ID, Name, Status from ApprovalProcess order by ID";
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetAdmins()
        {
            string sql = "select UserAccount, UserName, Status from UserInfo where RoleId=1 order by UserAccount";
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetSystemTypes()
        {
            string sql = "select ID, Name from BusinessType order by ID";
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetOrganizations()
        {
            string sql = @"select id,name,pId,Level, '/img/icon-fclose.png' as icon, '/img/icon-fclose.png' as iconClose, '/img/icon-fopen.png' as iconOpen 
                            from Department";
            return DBHelper.GetDataTable(sql);
        }
        public DataTable GetOrganizationUser()
        {
            string sql = @"select convert(varchar(10),d.id) as id,d.name,d.pId,d.Level, dbo.GetRootName(d.id) as rName, 0 as checked , 
                                    '/img/icon-fclose.png' as icon, '/img/icon-fclose.png' as iconClose, '/img/icon-fopen.png' as iconOpen
                        from Department d left join DepartmentUser du on d.ID=du.DepartmentId left join APDetail ad on du.Guid=ad.DUGUID
                        union
                        select convert(varchar(10),d.id)+'-'+convert(varchar(10),ui.ID) as id, ui.UserName, du.DepartmentId, d.Level, dbo.GetRootName(d.ID) as rName, 
                                case when du.DepartmentId is null then 0 else 1 end as checked,
                                '/img/icon-treeuser.png' as icon, '/img/icon-treeuser.png' as iconClose, '/img/icon-treeuser.png' as iconOpen
                        from UserInfo ui inner join DepartmentUser du on ui.ID=du.UserId inner join Department d on du.DepartmentId = d.ID
                        left join APDetail ad on du.Guid=ad.DUGUID";
            return DBHelper.GetDataTable(sql);
        }

        public string AddDepartment(string pid, string name, string level)
        {
            string sql = "insert into Department values(N'"+name+"',"+level+","+pid+@", 1);
                            select MAX(id) from Department";
            return DBHelper.ExecuteScalar(sql);
        }

        public void UpdateDepartment(string id, string name)
        {
            string sql = "update Department set name = N'"+name+"' where id="+id;
            DBHelper.ExecuteNonQuery(sql);
        }
        public void RemoveDepartment(string id)
        {
            string sql = "delete Department where id=" + id;
            DBHelper.ExecuteNonQuery(sql);
        }

        public DataTable GetApprovePrcess(string appPid)
        {
            string sql = @"select d.Level, dbo.GetRootName(du.DepartmentId) as pname, ui.UserName, d.Name as dname,
                            convert(varchar(10),d.id)+'-'+convert(varchar(10),ui.ID) as uid, d.id as did
                            from APDetail ad inner join DepartmentUser du on ad.DUGUID=du.Guid inner join UserInfo ui on du.UserId=ui.ID left join Department d on d.ID=du.DepartmentId
                            where APID=" + appPid + " order by d.Level desc";
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetPersenalInfo(string uid)
        {
            string sql = @"select ui.ID, ui.UserAccount, ui.UserName, ui.Telphone, ui.Email, d.Name as DepartmentName
                            from UserInfo ui left join Department d on ui.DepartmentId = d.ID where ui.ID = "+uid;
            return DBHelper.GetDataTable(sql);
        }

        public string UpdateUserInfo(string account, string uname, string psd, string tel, string email)
        {
            string sql;
            if (psd != "") { 
                psd = EncryptHelper.Encrypt(psd, "IamKey12");
                sql = "update UserInfo set UserName=N'"+uname+"', Password='"+psd+"', Telphone='"+tel+"',Email='"+email+"' where UserAccount='"+ account+"'";
            }
            else
            {
                sql = "update UserInfo set UserName=N'" + uname + "', Telphone='" + tel + "',Email='" + email + "' where UserAccount='" + account + "'";
            }
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return "1";
            else
                return "0";

        }

        public string CreateUser(string acc, string pasd, string nam, string telephone, string em, string did)
        {
            pasd = EncryptHelper.Encrypt(pasd, "IamKey12");
            string sql = "insert into UserInfo values('" + acc + "', N'" + nam + "', '" + pasd + "','" + telephone+"', '"+em+"', getdate(),1,1); ";
            sql += @"declare @uid int
                     select @uid=max(id) from UserInfo
                     insert into DepartmentUser values(NEWID(), "+did+", @uid, 1, 1)";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i > 0)
                return "1";
            else
                return "0";
        }

        public string AddExistUserToDepartment(string uid, string did)
        {
            string sql = "insert into DepartmentUser values(NEWID(), " + did + ", "+uid+", 0)";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i > 0)
                return "1";
            else
                return "0";
        }

        public DataTable GetDepartmentUsers(string did)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@did", did);
            return DBHelper.ExecuteSP("GetUsersByDepartmentId", parameters).Tables[0] ;
        }

        public string AddBusinessType(string bt)
        {
            string sql = "insert into BusinessType values(N'"+bt+ "'); select max(id) from BusinessType; ";
            return DBHelper.ExecuteScalar(sql);
        }

        public void DeleteBusinessType(string id)
        {
            string sql = "delete BusinessType where id = "+id;
            DBHelper.ExecuteNonQuery(sql);
        }

        public void UpdateBusinessType(string id, string name)
        {
            string sql = "update BusinessType set Name=N'"+name+"' where id = " + id;
            DBHelper.ExecuteNonQuery(sql);
        }

        public void DeleteUser(string uid)
        {
            string sql = "update DepartmentUser set Status = 0 where Guid = '" + uid+"'";
            DBHelper.ExecuteNonQuery(sql);
        }

        public string SearchUsers(string uname)
        {
            string sql = "select ID, UserAccount, UserName, Telphone from UserInfo where UserAccount like '%"+uname+ @"%'
                          union
                          select ID, UserAccount, UserName, Telphone from UserInfo where UserName like '%" + uname + "%'";
            DataTable dt= DBHelper.GetDataTable(sql);
            return JsonHelper.DataTableToJSON(dt);
        }
    }
}
