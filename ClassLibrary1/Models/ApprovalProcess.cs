using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ApprovalProcess
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 1:公司审批；2：竞标审批；3：竞标文件审批；4：定标文件审批
        /// </summary>
        public string Type { get; set; }
    }
    public class ApprovalProcessContext
    {
        public List<ApprovalProcess> GetApprovalProcesses()
        {
            string sql = "select * from ApprovalProcess order Id;";
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.ConvertTableToObj<ApprovalProcess>(dt);
        }

        public bool UpdateAppProcName(string id, string name)
        {
            string sql = "update ApprovalProcess set Name = '" + name + "' where ID=" + id;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
    }
}
