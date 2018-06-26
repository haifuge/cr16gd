using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class MakeBidFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abstract { get; set; }
        public string ProjAbstract { get; set; }
        public string Publisher { get; set; }
        public string PublishDate { get; set; }
        public string FileExplain { get; set; }
        public int Status { get; set; }
        public List<MakeBidFileCompany> BidCompanies { get; set; }
    }
    public class MakeBidFileCompany
    {
        public int MBFId;
        public int CompanyId;
        public int BidAmount;
        public int Status;
    }

    public class MakeBidFileContext
    {
        public List<MakeBidFile> GetMakeBidFiles()
        {
            string sql = "select Id, Name, ProjAbstract, Publisher, PublishDate, Status from MakeBidFile order by Id Desc";
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.ConvertTableToObj<MakeBidFile>(dt);
        }

        public MakeBidFile GetMakeBidFileDetail(string id)
        {
            string sql = "select * from MakeBidFile where Id="+id;
            DataTable dt = DBHelper.GetDataTable(sql);
            if (dt.Rows.Count == 0)
                return null;
            DataRow dr = dt.Rows[0];
            MakeBidFile makeBidFile = new MakeBidFile();
            makeBidFile.Id = int.Parse(dr["Id"].ToString());
            makeBidFile.Name = dr["Name"].ToString();
            makeBidFile.Abstract = dr["Abstract"].ToString();
            makeBidFile.ProjAbstract = dr["ProjAbstract"].ToString();
            makeBidFile.Publisher = dr["Publisher"].ToString();
            makeBidFile.PublishDate = dr["PublishDate"].ToString();
            makeBidFile.FileExplain = dr["FileExplain"].ToString();
            makeBidFile.Status = int.Parse(dr["Status"].ToString());
            makeBidFile.BidCompanies = new List<MakeBidFileCompany>();
            sql = "select * from MakeBidingFileCompany where MBFId=" + id;
            dt= DBHelper.GetDataTable(sql);
            makeBidFile.BidCompanies = JsonHelper.ConvertTableToObj<MakeBidFileCompany>(dt);
            return makeBidFile;
        }
        /// <summary>
        /// 新建定标文件
        /// </summary>
        /// <param name="makeBidFile"></param>
        /// <returns></returns>
        public bool AddMakeBidFile(MakeBidFile makeBidFile)
        {
            List<string> sqls = new List<string>();
            string sql = @"insert into MakeBidFile(Name, Abstract, ProjAbstract, Publisher, PublishDate, FileExplain, Status) values('{0}','{1}','{2}','{3}','{4}','{5}', 0)";
            string.Format(sql, makeBidFile.Name, makeBidFile.Abstract, makeBidFile.ProjAbstract, makeBidFile.Publisher, makeBidFile.PublishDate, makeBidFile.FileExplain);
            sqls.Add(sql);
            sql = @"declare @maxId int;
                    select @maxId = max(Id) from MakeBidFile;";
            foreach(var c in makeBidFile.BidCompanies)
            {
                sql += "insert into MakeBidingFileCompany values(@maxId, " + c.CompanyId + ", " + c.BidAmount + ", 0);";
            }
            return DBHelper.ExecuteSqlsWithTransaction(sqls);
        }

        public bool AddMakeBidFileCompany(string mbfId, string cId, string amount)
        {
            string sql = "insert into MakeBidingFileCompany values("+mbfId+", "+cId+", "+amount+", 0)";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
        public bool RemoveMakeBidFileCompany(string mbfId, string cId)
        {
            string sql = "delete MakeBidingFileCompany where MBFId=" + mbfId + " and CompanyId = " + cId;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 更新定标文件，参加投标单位状态。0：参加；1：拟中标
        /// </summary>
        /// <param name="mbfId"></param>
        /// <param name="cId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateMakeBidFileCompanyStatus(string mbfId, string cId, string status)
        {
            string sql = "update MakeBidingFileCompany set Status = "+status+" where MBFId=" + mbfId + " and CompanyId = " + cId;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

    }
}
