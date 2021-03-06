﻿using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProjType { get; set; }
        public string Location { get; set; }
        public int PublisherId { get; set; }
        public string PublishDate { get; set; }
        public string ProDescription { get; set; }
        public string Status { get; set; }
    }

    public class ProjectContext
    {
        public string GetAllProjects(string pageSize, string pageIndex, string uid, string ptype, string status, string pname)
        {
            int ps = int.Parse(pageSize);
            SqlParameter[] paras = new SqlParameter[6];
            paras[0] = new SqlParameter("@uid", int.Parse(uid));
            paras[1] = new SqlParameter("@pageSize", int.Parse(pageSize));
            paras[2] = new SqlParameter("@pageIndex", int.Parse(pageIndex));
            paras[3] = new SqlParameter("@pname", pname);
            paras[4] = new SqlParameter("@status", status);
            if(ptype=="")
                paras[5] = new SqlParameter("@ptype", "0");
            else
                paras[5] = new SqlParameter("@ptype", int.Parse(ptype));
            DataSet ds = DBHelper.ExecuteDataset(DBHelper.GetConnection(), "GetAllProjectsByUserId", paras);
            DataTable dt = ds.Tables[0];
            string data = JsonHelper.DataTableToJSON(dt);
            string total = ds.Tables[1].Rows[0][0].ToString();
            int pagecount = (int)Math.Ceiling(decimal.Parse(total) / ps);
            return "{\"List\":" + data + ", \"total\":" + total + ", \"PageCount\":" + pagecount + ",\"CurrentPage\":" + pageIndex + "}";
        }
        public DataTable GetProject(string id)
        {
            string sql = @"select p.Id, p.Name, bt.Name as ProjType, Location, d.Name+' '+ui.UserName as publisher, ui.id as uid,
                                convert(varchar(20),p.PublishDate, 23) as PublishDate, p.ProDescription, p.Status 
                            from Project p 
                            left join CompanyType bt on bt.id=p.ProjType
                            left join UserInfo ui on p.PublisherId=ui.ID
                            left join DepartmentUser du on ui.ID=du.UserId and du.Status=1
                            left join Department d on d.ID=du.DepartmentId
                            where p.id=" + id;
            return DBHelper.GetDataTable(sql);
        }
        /// <summary>
        /// 数据库中添加项目中文件
        /// </summary>
        /// <param name="pid">项目id</param>
        /// <param name="ftype">1：招标文件；2：标文件；3：定标文件</param>
        /// <param name="fullPath">文件全路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="comment">对文件的评注</param>
        public void AddProjectFile(string pid, string ftype, string fullPath, string fileName, string comment)
        {
            string sql = "insert into BidDocument values(" + pid + ", " + ftype + ", N'" + fullPath + "',N'" + fileName + "', N'" + comment + "');";
            DBHelper.ExecuteNonQuery(sql);
        }

        public void RemoveProjectFile(string filefullpath)
        {
            string sql = "delete BidDocument where FilePath='"+filefullpath+"'";
            DBHelper.ExecuteNonQuery(sql);
        }
        public DataTable GetProjectFiles(string pid, string ftype)
        {
            //string sql = @"select right(FilePath, charindex('\', reverse(FilePath) + '\') - 1) as 'FilePath', FileName from BidDocument where ProjId=" + pid + " and FileType=" + ftype;
            string sql = @"select FilePath, FileName from BidDocument where ProjId=" + pid + " and FileType=" + ftype;
            return DBHelper.GetDataTable(sql);
        }

        public bool AddProject(Project p)
        {
            string sql = @"select ID from Department 
                           where ProjectDp=1 and ID in (
	                                    select PID from Department where ID in (select du.departmentid from DepartmentUser du where du.UserId="+p.PublisherId.ToString()+"))";
            string did = DBHelper.ExecuteScalar(sql);
            sql = @"insert into Project values(N'"+p.Name+"',N'"+p.ProjType+"',N'"+p.Location+"',"+p.PublisherId+", getdate(), N'"+p.ProDescription+ "',N'未发布', "+ did + ")";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
            {
                return true;
            }
            return false;
        }

        public bool UpdateProject(Project p)
        {
            string sql = @"update Project set Name = N'" + p.Name + "', ProjType = N'" + p.ProjType + "', Location = N'" + p.Location + "', ProDescription = N'" + p.ProDescription + "' where id="+p.Id;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
            {
                return true;
            }
            return false;
        }

        public string GetForeReference(string g)
        {
            string sql = "select FilePath from BidDocument where FileType=4 and FilePath like '%" + g + "%'";
            return DBHelper.ExecuteScalar(sql);
        }

        //未发布；招标文件审核中，招标文件审核通过，招标审核中；招标审核通过；定标文件审核中；已通过
        public bool UpdateProjectStatus(string pid, string s)
        {
            string sql = @"update Project set Status = N'" + s + "' where id=" + pid;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 创建审批流程
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pid"></param>
        /// <param name="apid">1：公司；2：发表文件；3：发表；4：定标文件</param>
        public DataTable CreateApproveProcess(string uid, string pid, int apid)
        {
            SqlParameter[] paras = new SqlParameter[3];
            paras[0] = new SqlParameter("@uid", uid);
            paras[1] = new SqlParameter("@objid", pid);
            paras[2] = new SqlParameter("@apid", apid);
            return DBHelper.ExecuteSP("CreateApproveProcessing", paras).Tables[0];
        }

        public void DeleteBidFile(string pid)
        {
            string sql = "delete Project where Id="+pid;
            sql += "; delete BidingFile where ProjId="+pid;
            sql += "; delete AppProcessing where AppProcId=2 and ObjId="+pid;
            DBHelper.ExecuteNonQuery(sql);
        }

        public bool AllowDelete(string pid)
        {
            string sql = "select max(Approved) from AppProcessing where AppProcId=2 and ObjId=" + pid;
            string approved = DBHelper.ExecuteScalar(sql);
            if (approved == "" || approved == "1")
                return true;
            else
                return false;
        }
    }
}
