using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Sucool.PushRemind
{
    public static class SMSHelper
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobiles"></param>
        /// <returns></returns>
        public static string SendMsg(string mobiles, string contents)
        {
            string url = "http://wcfservice.sucool.com/WCF_T3_0/Other.svc/SendMsg?t=vvZ57V1Z7xvF5lHxLJyKA5WA8UyNBjT9&m="
            + mobiles + "&c=" + contents;
            return RequestHttpGetUrl(url);
        }
        /// <summary>
        /// URL Get请求
        /// </summary>
        /// <param name="url">请求url</param>        
        /// <param name="ecoding">编码</param>
        /// <returns></returns>
        public static string RequestHttpGetUrl(this string requestUrl)
        {
            try
            {
                string ct = string.Empty;
                byte[] postBytes = Encoding.ASCII.GetBytes(requestUrl);
                HttpWebRequest Rst = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                HttpWebResponse Rsp = (HttpWebResponse)Rst.GetResponse();
                using (StreamReader reader = new StreamReader(Rsp.GetResponseStream(), Encoding.UTF8))
                {
                    //ct = reader.ReadToEnd();
                }
                return ct;
            }
            catch (System.Net.WebException WebExcp)
            {
                string.Format("\r\n>>>短信发送异常：{0},time:{1}\r\n详细参数:{2}\r\n",
                    WebExcp.Message,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    requestUrl).WriteLog("ERROR");
                return WebExcp.Message;
            }
        }
    }
}
