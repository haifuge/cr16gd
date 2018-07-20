using DAL.Tools;
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

        public string GetAdmins(string page, string pagesize)
        {
            int pi = int.Parse(page);
            int ps = int.Parse(pagesize);
            int startIndex = (pi - 1) * ps + 1;
            int endIndex = pi * ps;
            string sql = @"select identity(int,1,1) as id, UserAccount, UserName, Status into #temp1 from UserInfo where RoleId=1 order by UserAccount asc;
                            select * from #temp1 where id between "+startIndex+" and "+endIndex+@"
                            drop table #temp1";
            DataTable dt = DBHelper.GetDataTable(sql);
            sql = "select count(1) from UserInfo where RoleId=1 ";
            string total = DBHelper.ExecuteScalar(sql);
            int pagecount = (int)Math.Ceiling(decimal.Parse(total)/ps);
            return "{\"List\":" + JsonHelper.DataTableToJSON(dt)+", \"total\":"+total+ ", \"pagecount\":" + pagecount+ "}";
        }

        public DataTable GetSystemTypes()
        {
            string sql = "select ID, Name from BusinessType order by ID";
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetOrganizations()
        {
            string sql = @"select id,name,pId,Level as splevel,ProjectDp, '/img/icon-fclose.png' as icon, '/img/icon-fclose.png' as iconClose, '/img/icon-fopen.png' as iconOpen 
                            from Department where Status=1";
            return DBHelper.GetDataTable(sql);
        }
        public DataTable GetOrganizationUser(string apid)
        {
            string sql = @"select convert(varchar(10),d.id) as id,d.name,d.pId,d.Level, dbo.GetRootName(d.id) as rName, 0 as checked, '' as duguid, 
                                '/img/icon-fclose.png' as icon, '/img/icon-fclose.png' as iconClose, '/img/icon-fopen.png' as iconOpen, d.ProjectDp
                            from Department d left join DepartmentUser du on d.ID=du.DepartmentId left join APDetail ad on du.Guid=ad.DUGUID where d.Status=1
                            union
                            select convert(varchar(10),d.id)+'-'+convert(varchar(10),ui.ID) as id, ui.UserName, du.DepartmentId, d.Level, dbo.GetRootName(d.ID) as rName, 
                                case when ad.APID is null then 0 else 1 end as checked, du.guid, 
                                '/img/icon-treeuser.png' as icon, '/img/icon-treeuser.png' as iconClose, '/img/icon-treeuser.png' as iconOpen, 0 as ProjectDp
                            from UserInfo ui inner join DepartmentUser du on ui.ID=du.UserId inner join Department d on du.DepartmentId = d.ID
                            left join APDetail ad on du.Guid=ad.DUGUID and ad.APID=" + apid+" where ui.Status=1 and du.Status=1";
            return DBHelper.GetDataTable(sql);
        }

        public string AddDepartment(string pid, string name, string level, string projectdp)
        {
            string sql = "insert into Department values(N'"+name+"',"+level+","+pid+@", 1, "+projectdp+"); select MAX(id) from Department";
            return DBHelper.ExecuteScalar(sql);
        }

        public void UpdateDepartment(string id, string name, string level, string projectdp)
        {
            string sql = "update Department set name = N'"+name+"', Level="+level+ ", ProjectDp=" + projectdp + " where id="+id;
            DBHelper.ExecuteNonQuery(sql);
        }
        public void RemoveDepartment(string id)
        {
            string sql = "update Department set Status=0 where id=" + id+"; update DepartmentUser set Status=0 where DepartmentId="+id;
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
                           from UserInfo ui 
                           left join DepartmentUser du on ui.ID=du.UserId 
                           left join Department d on du.DepartmentId = d.ID where ui.ID =" + uid;
            return DBHelper.GetDataTable(sql);
        }

        public string UpdateUserInfo(string account, string uname, string psd, string tel, string email, string roleid)
        {
            string sql;
            if (psd != "") { 
                psd = EncryptHelper.Encrypt(psd, "IamKey12");
                sql = "update UserInfo set UserName=N'"+uname+"', Password='"+psd+"', Telphone='"+tel+"',Email='"+email+"', RoleId="+roleid+" where UserAccount='"+ account+"'";
            }
            else
            {
                sql = "update UserInfo set UserName=N'" + uname + "', Telphone='" + tel + "',Email='" + email + "', RoleId=" + roleid + " where UserAccount='" + account + "'";
            }
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return "1";
            else
                return "0";

        }

        public string CreateUser(string acc, string pasd, string nam, string telephone, string em, string did, string roleid)
        {
            pasd = EncryptHelper.Encrypt(pasd, "IamKey12");
            string sql = "insert into UserInfo values('" + acc + "', N'" + nam + "', '" + pasd + "','" + telephone+"', '"+em+"', getdate(),1,"+ roleid + "); ";
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

        public string GetDepartmentUsers(string did, string pageSize, string pageIndex)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@did", did);
            DataTable dt = DBHelper.ExecuteSP("GetUsersByDepartmentId", parameters).Tables[0];
            DataTable ndt = dt.Clone();
            int pi = int.Parse(pageIndex);
            int ps = int.Parse(pageSize);
            int startIndex = (pi - 1) * ps+1;
            int endIndex = pi * ps;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i >= startIndex && i < endIndex)
                    ndt.ImportRow(dt.Rows[i]);
                else if (i >= endIndex)
                    break;
            }
            string total = dt.Rows.Count.ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + JsonHelper.DataTableToJSON(dt) + ", \"total\":" + total + ", \"pagecount\":" + pagecount + "}";
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

        public void UpdateCompanyBusinessType(string id, string name)
        {
            string sql = "update CompanyType set Name=N'" + name + "' where id = " + id;
            DBHelper.ExecuteNonQuery(sql);
        }

        public void AddUserToDepartment(string did, string uid)
        {
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter("@uid", uid);
            paras[1] = new SqlParameter("@did", did);
            DBHelper.ExecuteSP("AddUser2Department", paras);
        }

        public DataTable GetCompanyBusinessTypes()
        {
            string sql = "select * from CompanyType order by ID";
            return DBHelper.GetDataTable(sql);
        }

        public void AddCompanyBusinessType(string bt)
        {
            string sql = "insert into CompanyType values(N'"+bt+"'); ";
            DBHelper.ExecuteNonQuery(sql);
        }

        public void DeleteCompanyBusinessType(string id)
        {
            string sql = "delete CompanyType where ID = "+id;
            DBHelper.ExecuteNonQuery(sql);
        }
    }
}
