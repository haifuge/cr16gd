using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json;


namespace WXMessage
{
    public class AccessTokenHelper
    {
        //填写自己微信的秘钥
        private static string appId = "wxe1b146eaa5796cc2";
        private static string appSecret = "6721578a1cf1993a747b508f97f32815";//7cbebf147616b5ca77a7306fe093c79d

        private static DateTime GetAccessToken_Time;
        /// <summary>
        /// 过期时间为7200秒
        /// </summary>
        private static int Expires_Period = 7200;
        /// <summary>
        /// 
        /// </summary>
        private static string mAccessToken;
        /// <summary>
        /// 
        /// </summary>
        public static string AccessToken
        {
            get
            {
                //如果为空，或者过期，需要重新获取
                if (string.IsNullOrEmpty(mAccessToken) || HasExpired())
                {
                    //获取
                    mAccessToken = GetAccessToken(appId, appSecret);
                }
                return mAccessToken;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        private static string GetAccessToken(string appId, string appSecret)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret);
            string result = HttpUtility.GetData(url);
            var obj = JsonConvert.DeserializeAnonymousType(result, new {access_token="access_token", expires_in=7200});
            GetAccessToken_Time = DateTime.Now.AddSeconds(obj.expires_in);
            return obj.access_token;
        }

        /// <summary>
        /// 判断Access_token是否过期
        /// </summary>
        /// <returns>bool</returns>
        private static bool HasExpired()
        {
            if (GetAccessToken_Time != null)
            {
                //过期时间，允许有一定的误差，一分钟。获取时间消耗
                if (DateTime.Now > GetAccessToken_Time.AddSeconds(Expires_Period).AddSeconds(-60))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
