using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
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
            string sql = @"select d.id,d.name,d.pId,d.Level, dbo.GetRootName(d.id) as rName, 0 as checked , 
                                    '/img/icon-fclose.png' as icon, '/img/icon-fclose.png' as iconClose, '/img/icon-fopen.png' as iconOpen
                        from Department d left join APDetail ad on d.id=ad.AppPosId
                        union
                        select d.id+'-'+ui.ID as id, ui.UserName, ui.DepartmentId, d.Level, dbo.GetRootName(d.ID) as rName, 
                                case when ad.AppPosId is null then 0 else 1 end as checked,
                                '/img/icon-fclose.png' as icon, '/img/icon-fclose.png' as iconClose, '/img/icon-fclose.png' as iconOpen
                        from UserInfo ui inner join Department d on ui.DepartmentId = d.ID
                        left join APDetail ad on ui.ID=ad.UserId and ad.AppPosId=d.ID";
            return DBHelper.GetDataTable(sql);
        }

        public string AddDepartment(string pid, string name, string level)
        {
            string sql = "insert into Department values(N'"+name+"',"+level+","+pid+@");
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
            string sql = @"select ad.Level, dbo.GetRootName(ad.AppPosId) as pname, ui.UserName, d.Name as dname, ui.Id as uid, d.id as did
                            from APDetail ad inner join UserInfo ui on ad.UserId=ui.ID left join Department d on d.ID=ad.AppPosId
                            where APID=" + appPid+" order by ad.Level desc";
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetPersenalInfo(string uid)
        {
            string sql = @"select ui.ID, ui.UserAccount, ui.UserName, ui.Telphone, ui.Email, d.Name as DepartmentName
                            from UserInfo ui left join Department d on ui.DepartmentId = d.ID where ui.ID = "+uid;
            return DBHelper.GetDataTable(sql);
        }

        public string UpdateUserInfo(string id, string uname, string psd, string tel, string email)
        {
            psd = EncryptHelper.Encrypt(psd, "IamKey12");
            string sql = "update UserInfo set UserName=N'"+uname+"', Password='"+psd+"', Telphone='"+tel+"',Email='"+email+"' where ID="+id;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return "1";
            else
                return "0";

        }

        public string CreateUser(string acc, string pasd, string nam, string telephone, string em, string did)
        {
            pasd = EncryptHelper.Encrypt(pasd, "IamKey12");
            string sql = "insert into UserInfo values('" + acc + "', N'" + nam + "', '" + pasd + "'," + did + ",'" + telephone+"', '"+em+"', getdate(),1,1)";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return "1";
            else
                return "0";
        }

        public DataTable GetDepartmentUsers(string did)
        {
            string sql = @"select ui.ID, ui.UserAccount, ui.UserName, d.Name as department, ui.Telphone, ui.Email  
                            from UserInfo ui inner join Department d on ui.DepartmentId=d.ID 
                            where ui.DepartmentId="+did;
            return DBHelper.GetDataTable(sql);
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
            string sql = "delete UserInfo where id = " + uid;
            DBHelper.ExecuteNonQuery(sql);
        }
    }
}
