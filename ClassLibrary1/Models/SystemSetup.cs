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
            string sql = @"select id,name,pId,Level from Department";
            return DBHelper.GetDataTable(sql);
        }
        public DataTable GetOrganizationUser()
        {
            string sql = @"select id,name,pId,Level from Department where Status=1
                            union
                            select d.ID + '-' + ui.ID as id, ui.UserName, ui.DepartmentId, d.Level from UserInfo ui inner join Department d on ui.DepartmentId = d.ID and ui.Status=1";
            return DBHelper.GetDataTable(sql);
        }

        public string AddDepartment(string pid, string level, string name = "")
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
    }
}
