using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Tools;
using System.Data;

namespace DAL.Models
{
    public class Organization
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public int Level { get; set; }
        public string CreateDate { get; set; }
        public string Creator { get; set; }
        public int Status { get; set; }
    }

    public class OrganizationContext
    {
        public static List<Organization> organizations = new List<Organization>();
        public OrganizationContext()
        {
            string sql = @"select * from Department";
            DataTable dt = DBHelper.GetDataTable(sql);
            organizations = JsonHelper.ConvertTableToObj<Organization>(dt);
        }

        public bool AddOrganization(Organization organization)
        {
            string sql = @"insert into Department values('" + organization.Name + "', " + organization.Level + ", " +
                organization.CreateDate + ", getdate(), 1)";
            int res = DBHelper.ExecuteNonQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool UpdateOrgarnization(Organization organization)
        {
            string sql = @"update Department set Name = '" + organization.Name + "', Level = " + organization.Level + ", " +
                "status = "+organization.Status;
            int res = DBHelper.ExecuteNonQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool DeleteOrganization(Organization organization)
        {
            string sql = @"delete Department where ID = " + organization.ID;
            int res = DBHelper.ExecuteNonQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }
    }
}
