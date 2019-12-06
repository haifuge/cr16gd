using Newtonsoft.Json;
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
        public void sendRefuseNotification(string openid, string apid, string pname)
        {
            if (openid.Length < 25)
                return;
            string aptype = "";
            switch (apid)
            {
                case "1":
                    aptype = "名录外企业审批";
                    break;
                case "5":
                    aptype = "名录内企业审批";
                    break;
                case "2":
                    aptype = "招标文件审批";
                    break;
                case "3":
                    aptype = "招标审批";
                    break;
                case "4":
                    aptype = "定标文件审批";
                    break;
            }
            string token = AccessTokenHelper.AccessToken();
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token;
            string temp = "{\"touser\": \"" + openid + "\"," +
                    "\"template_id\": \"3k7WBrJEN6ngpLDOtSopdflhft8nJ0KrgVO41_2B3ck\", " +
                    "\"topcolor\": \"#FF0000\", " +
                    "\"data\": " +
                    "{\"first\": {\"value\": \"提交的" + aptype + "被驳回。\"}," +
                    "\"keyword1\": { \"value\": \"" + pname + "\"}," +
                    "\"keyword2\": { \"value\": \"" + DateTime.Now.ToString("G") + "\"}," +
                    "\"remark\": {\"value\": \"请尽快处理，谢谢合作。\" }}}";

            GetResponseData(temp, @url);
        }
        /// <summary>
        /// 发送微信通知
        /// </summary>
        /// <param name="userid">要处理人的id</param>
        /// <param name="reurl">跳转地址</param>
        /// <param name="aptype">审批类型</param>
        /// <param name="pname">项目名称</param>
        /// <param name="puser">当前处理人姓名</param>
        /// <returns></returns>
        public string SendTempletMessge(string userid, string reurl="", string aptype="", string pname="", string puser="")
        {
            string strReturn = string.Empty;
            string token = AccessTokenHelper.AccessToken();

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
            try { 
                strReturn = GetResponseData(temp, @url);
            }
            catch
            {
                // 预防AccessToken失效导致发送失败
                token = AccessTokenHelper.AccessToken(true);
                strReturn = GetResponseData(temp, @url);
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
            var obj = JsonConvert.DeserializeAnonymousType(strResult, new { errcode = 0, errmsg = "ok" });
            streamReceive.Dispose();
            streamReader.Dispose();
            if (obj.errcode != 0)
                throw new Exception(obj.errmsg);
            return strResult;
        }
    }
}
