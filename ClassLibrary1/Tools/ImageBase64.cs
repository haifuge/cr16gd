using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Tools
{
    public class ImageBase64
    {
        public static void Base64ToImage(string base64, string filefullPath)
        {
            byte[] arr = Convert.FromBase64String(base64);
            using (MemoryStream ms = new MemoryStream(arr)) { 
                Bitmap bmp = new Bitmap(ms);
                bmp.Save(filefullPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
        public string Base64ToImages(string upimgPath, string base64String)
        {
            string goodspath = "/uploadpics";//Server.MapPath(upimgPath);  //用来生成文件夹
            if (!Directory.Exists(goodspath))
            {
                Directory.CreateDirectory(goodspath);
            }
            var imgPath = string.Empty;
            if (!string.IsNullOrEmpty(base64String))
            {
                var splitBase = base64String.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitBase)
                {
                    var path = upimgPath + Guid.NewGuid() + ".jpg";

                    string filePath = "/uploadpics/" + path;//Server.MapPath(path);// Server.MapPath(upimgPath + Guid.NewGuid() + ".jpg");
                    File.WriteAllBytes(filePath, Convert.FromBase64String(item));
                    imgPath += path + ";";
                }
            }
            else { imgPath = ";"; }
            return imgPath.TrimEnd(';');
        }
    }
}
