using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Tools;

namespace DAL.Models
{
    public class UserInfo
    {
        public int ID { get; set; }
        public string UserAccount { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public string Telphone { get; set; }
        public string Email { get; set; }
        //public string CreateDate { get; set; }
        public int Status { get; set; }
        public int RoleId { get; set; }

    }

    public class UserInfoContext
    {
        public List<UserInfo> GetUsersByDepartment(int dId)
        {
            string sql = @"select u.ID, u.UserAccount, u.Telphone, u.Email, u.Status 
                            from UserInfo u, UserStation us 
                            where u.ID=us.UserId and us.StationId="+dId;
            DataTable dt = DBHelper.GetDataTable(sql);
            return JsonHelper.ConvertTableToObj<UserInfo>(dt);
        }

        public string GetUserDepartment(int uid)
        {
            string sql = "select d.Name from Department d inner join DepartmentUser du on d.id=du.DepartmentId where du.MainDeparment=1 and du.Status=1 and du.UserId=" + uid;
            return DBHelper.ExecuteScalar(sql);
        }

        public bool AddUser(UserInfo user, int dId)
        {
            List<string> sqls = new List<string>();
            string sql = @"insert into UserInfo values('" + user.UserAccount + "', '" + user.UserName + "', '" + user.Password+"', '"+
                user.Telphone + "', '"+user.Email+"', getdate(), 1);";
            sqls.Add(sql);
            sql = @" declare @id int;
                    select @id = max(ID) from UserInfo;
                    insert into UserStation values(@id, "+dId+", 1, 1)";
            sqls.Add(sql);
            bool res = DBHelper.ExecuteSqlsWithTransaction(sqls);
            return res;
        }

        public bool AddUserToDepartment(int uId, int dId)
        {
            string sql = "insert into UserStation values(" + uId + ", " + dId + ", 0, 1)";
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i==1)
                return true;
            else
                return false;
        }

        public bool RemoveUserFromDepartment(int uId, int dId)
        {
            string sql = "delete UserStation where UserId = " + uId + " and StationId = " + dId;
            int i = DBHelper.ExecuteNonQuery(sql);
            if (i == 1)
                return true;
            else
                return false;
        }

        public bool UpdateUser(UserInfo user)
        {
            string sql = @"update UserInfo set UserName='"+user.UserName+"', Telphone = '"+user.Telphone+"', Email = '"+user.Email+
                "' Status = "+user.Status+" where ID = "+user.ID;
            int res = DBHelper.ExecuteNonQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool EnableUser(int uId)
        {
            string sql = @"update UserInfo set Status = 1 where ID = " + uId;
            int res = DBHelper.ExecuteNonQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public UserInfo CheckLogin(string account, string password)
        {
            //password = EncryptHelper.Encrypt(password, "IamKey12");
            //string sql = "select * from UserInfo where UserAccount='" + account + "' and Password = '" + password + "'";
            string sql = "select * from UserInfo where UserAccount='" + account + "'";
            DataTable dt = DBHelper.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
                return JsonHelper.ConvertTableToObj<UserInfo>(dt)[0];
            return null;
        }
    }
}
