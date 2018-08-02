using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Tools;
using AliMessage;

namespace RailBiding.Controllers
{
    public class ForgetPasswordController : Controller
    {
        // GET: ForgetPassword
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            string vcode = Request["code"].ToString();
            Session["uAccount"] = Request["account"].ToString();
            if (vcode != Session["VCode"].ToString())
                return View("//ForgetPassword");
            return View();
        }
        public string CheckNext(string vcode)
        {
            if (vcode == Session["VCode"].ToString())
            {
                return "1";
            }
            else
                return "0";
        }

        public string CheckSendMessage(string account, string phone)
        {
            string sql = "select count(1) from UserInfo where UserAccount='" + account + "' and Telphone='" + phone + "' and Status = 1;";
            string i = DBHelper.ExecuteScalar(sql);
            if (i != "1")
            {
                return "用户账号和手机号不匹配";
            }
            else
            {
                SendMessage sm = new SendMessage();
                string vcode = GenerateCheckCode();
                Session["VCode"] = vcode;
                sm.SendVerifyCode(phone, vcode);
                return "验证码已发送，请注意查收";
            }
        }
        //随机生成验证码
        private string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('a' + (char)(number % 26));

                if (code.ToString() == "0" || code.ToString().ToLower() == "o")
                {
                    code = 'v';
                }

                if (code.ToString().ToLower() == "z" || code.ToString().ToLower() == "i" || code.ToString().ToLower() == "b")
                {
                    code = '8';
                }
                checkCode += code.ToString();
            }
            return checkCode;
        }

        public void UpdateUserPassword(string password)
        {
            password = EncryptHelper.Encrypt(password, "IamKey12");
            string sql = "update UserInfo set Password='"+password+ "' where UserAccount='"+Session["uAccount"].ToString()+"'";
            DBHelper.ExecuteNonQuery(sql);
        }
    }
}