using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AppProcessing
    {
        /// <summary>
        /// 审批流程Id
        /// </summary>
        public int AppProcId { get; set; }
        /// <summary>
        /// 审批对象Id，公司id，文件Id，竞标Id
        /// </summary>
        public int ObjId { get; set; }
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 0:未处理；1：同意；2：不同意
        /// </summary>
        public int Approved { get; set; }
        public string Comment { get; set; }
        public string DealDatetime { get; set; }
    }
    public class AppProcessingContext
    {
        public bool InitAppProcess(string objId, string appProcId)
        {
            string sql = @"insert into AppProcessing(AppProcId, ,ObjId, DepartmentId, UserId，Approved)
                            select " + appProcId + ", "+ objId+ @", DepartmentId, UserId, 0
                            from APDetail where APID = " + appProcId;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool UpdateAppProcess(AppProcessing ap)
        {
            string sql = @"update AppProcessing set Approved ="+ap.Approved+", Comment='"+ap.Comment+@"', DealDatetime=getdate() 
                            where AppProcId= "+ap.AppProcId+" and ObjId = "+ap.ObjId+ " and DepartmentId = " + ap.DepartmentId+" and  UserId ="+ap.UserId;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i > 0)
                return true;
            else
                return false;
        }
        public List<AppProcessing> GetAppProcessing(string objId, string appProcId)
        {
            string sql = "select * from AppProcessing where AppProcId="+objId+" and ObjId="+objId;
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.ConvertTableToObj<AppProcessing>(dt);
        }
    }
}
