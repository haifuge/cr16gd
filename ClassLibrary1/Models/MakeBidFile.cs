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

        public DataTable GetMakeBidFileDetail(string id)
        {
            string sql = @"select Abstract, CONVERT(varchar(20), PublishDate, 23) as PublishDate, Status,PublisherId,FileExplain 
                            from MakeBidingFile where ProjId=" + id;
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
        public DataTable GetFiles(string pid)
        {
            string sql = "select FileName from BidDocument where ProjId=" + pid + " and FileType=3";
            return DBHelper.GetDataTable(sql);
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
