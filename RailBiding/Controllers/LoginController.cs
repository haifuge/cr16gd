using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Models;
using DAL.Tools;

namespace RailBiding.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public string Login(string account, string psd)
        {
            if(account==null || psd==null)
                return "0";
            UserInfoContext uc = new UserInfoContext();
            UserInfo ui = uc.CheckLogin(account, psd);
            if (ui is null)
            {
                return "0";
            }
            Session["UserId"] = ui.ID;
            Session["UserName"] = ui.UserName;
            Session["MenuList"]=MenuHelper.GetMainMenu(ui.RoleId);
            HttpCookie c1 = new HttpCookie("UserName");
            c1.Value= ui.UserName;
            c1.Expires = DateTime.Now.AddHours(3);
            Response.Cookies.Add(c1);
            HttpCookie c2 = new HttpCookie("UserDepartment");
            c2.Value= uc.GetUserDepartment(ui.DepartmentId); 
            c2.Expires = DateTime.Now.AddHours(3);
            Response.Cookies.Add(c2);
            HttpCookie c3 = new HttpCookie("RoleId");
            c3.Value= ui.RoleId.ToString();
            c3.Expires = DateTime.Now.AddHours(3);
            Response.Cookies.Add(c3);
            return "1";
        }

        public ActionResult GenerateVerifyCode()
        {
            Session["VerifyCode"] = GenerateCheckCode();
            byte[] bytes = CreateCheckCodeImage(Session["VerifyCode"].ToString());
            Session.Timeout = 60;
            return File(bytes, @"image");
        }

        //随机生成验证码
        private string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                if (code.ToString() == "0" || code.ToString().ToLower() == "o")
                {
                    code = 'V';
                }

                if (code.ToString().ToLower() == "z" || code.ToString().ToLower() == "i" || code.ToString().ToLower() == "b")
                {
                    code = '8';
                }
                checkCode += code.ToString();
            }
            return checkCode;
        }

        //生成验证码图片
        public byte[] CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return null;
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(70, 24);
            Graphics g = Graphics.FromImage(image);
            try
            {
                Random random = new Random();
                g.Clear(Color.Cornsilk);
                for (int i = 0; i < 8; i++)
                {
                    int x1 = random.Next(image.Width - 25);
                    int y1 = random.Next(image.Height - 15);
                    int x2 = random.Next(1, 25);
                    int y2 = random.Next(1, 15);
                    int x3 = random.Next(0, 45);
                    int y3 = random.Next(-170, 270);
                    g.DrawArc(new Pen(Brushes.Gray), x1, y1, x2, y2, x3, y3);
                }
                //Maiandra GD
                Font font = new Font("Arial", 13, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                //System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);

                g.DrawString(checkCode, font, Brushes.Black, 5, 3);
                for (int i = 0; i < 45; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                g.DrawRectangle(new Pen(Brushes.Gray), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public string GetVerifyCode()
        {
            return Session["VerifyCode"].ToString();
        }
    }
}