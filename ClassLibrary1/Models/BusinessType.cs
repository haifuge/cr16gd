using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class BusinessType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BusinessTypeContext
    {
        public DataTable GetBusinessTypes()
        {
            string sql = "select * from BusinessType order by Id";
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
        public bool AddBusinessType(string name)
        {
            string sql = "insert BusinessType values('"+name+"')";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
        public bool RemoveBusinessType(string id)
        {
            string sql = "delete BusinessType where Id in (" + id + ")";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
    }
}
