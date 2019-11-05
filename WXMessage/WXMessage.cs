using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WXMessage
{
    public class WXMessage
    {
        public void sendInfoByWinXin(DataTable dt, string apid, string oid, string pname, string puser)
        {
            if (dt.Rows[0][0].ToString() == "0")
                return;
            string uid = dt.Rows[0][1].ToString();
            string reurl = "";
            string aptype = "";
            switch (apid)
            {
                case "1":
                    aptype = "名录外企业审批";
                    reurl = "http://cr16gd.saibo.net.cn/MobileCompany?id=" + oid + "&approved=1&userid=" + uid;
                    break;
                case "5":
                    aptype = "名录内企业审批";
                    reurl = "http://cr16gd.saibo.net.cn/MobileCompany?id=" + oid + "&approved=1&userid=" + uid;
                    break;
                case "2":
                    aptype = "招标文件审批";
                    reurl = "http://cr16gd.saibo.net.cn/MobileBidingFile?pid=" + oid + "&approved=1&userid=" + uid;
                    break;
                case "3":
                    aptype = "招标审批";
                    reurl = "http://cr16gd.saibo.net.cn/MobileBid?pid=" + oid + "&approved=1&userid=" + uid;
                    break;
                case "4":
                    aptype = "定标文件审批";
                    reurl = "http://cr16gd.saibo.net.cn/MobileMakeBidFile?pid=" + oid + "&approved=1&userid=" + uid;
                    break;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[0][0].ToString() != "0")
                    SendTempletMessge(dt.Rows[0][0].ToString(), reurl, aptype, pname, puser);
            }

        }

        private string SendTempletMessge(string userid, string reurl="", string aptype="", string pname="", string puser="")
        {
            string strReturn = string.Empty;
            try
            {
                string token = AccessTokenHelper.AccessToken;

                string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token;
                string temp = "{\"touser\": \"" + userid + "\"," +
                       "\"template_id\": \"085OHAqvsR4y6kB_CCArLwioyEnxwTBmiFp0Jka5sYo\", " +
                       "\"topcolor\": \"#FF0000\", " +
                       "\"url\": \""+ reurl + "\", " +
                       "\"data\": " +
                       "{\"first\": {\"value\": \"" + aptype + "\"}," +
                       "\"keyword1\": { \"value\": \""+ pname + "\"}," +
                       "\"keyword2\": { \"value\": \""+puser+"\"}," +
                       "\"keyword3\": { \"value\": \"" + DateTime.Now.ToString("G") + "\"}," +
                       "\"remark\": {\"value\": \"请尽快处理，谢谢合作。\" }}}";

                //核心代码
                GetResponseData(temp, @url);
                strReturn = "推送成功";
            }
            catch (Exception ex)
            {
                strReturn = ex.Message;
            }
            return strReturn;
        }
        /// <summary>
        /// 返回JSon数据
        /// </summary>
        /// <param name="JSONData">要处理的JSON数据</param>
        /// <param name="Url">要提交的URL</param>
        /// <returns>返回的JSON处理字符串</returns>
        public string GetResponseData(string JSONData, string Url)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "json";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);
            //声明一个HttpWebRequest请求
            request.Timeout = 90000;
            //设置连接超时时间
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();
            return strResult;
        }
    }
}
