using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class APDetail
    {
        public int APID { get; set; }
        public int Level { get; set; }
        public int AppPosId { get; set; }
        public int UserId { get; set; }

    }
    public class APDetailContext
    {
        public List<APDetail> GetAPDetail(string apId)
        {
            string sql = "select * from APDetail where APID = " + apId;
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.ConvertTableToObj<APDetail>(dt);
        }

        public bool UpdateAPDetail(List<APDetail> details)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("delete APDetail where APID="+details[0].APID+";");
            foreach(var detail in details)
            {
                sb.Append("insert into APDetail values("+detail.APID+", "+detail.Level+", "+detail.AppPosId+", "+detail.UserId+");");
            }
            int i = DBHelper.ExecuteNonQuery(sb.ToString());
            if (i == 1)
                return true;
            else
                return false;
        }
    }
}
