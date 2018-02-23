using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WebPlatform.BLL
{
    public class HTTPHelper
    {
        /// 创建POST方式的HTTP请求  
        public static string HttpPostResponse(string url, IDictionary<string, string> parameters)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");

            using (Stream s = request.GetResponse().GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);

                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 带参请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="ContentType"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static string WechatRequest(string url, MethodTypeEnum method, DataTypeEnum ContentType, string strData)
        {
            //MyLog.WriteLog("WechatRequest:" + url + "***" + method.ToString() + "***" + ContentType.ToString());

            string result = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Method = method.ToString();
                webRequest.ContentType = "application/" + ContentType.ToString();
                byte[] reqBodyBytes = System.Text.Encoding.UTF8.GetBytes(strData); //指定编码，微信用的是UTF8，我起初用的是default，以为默认是utf8的，后来发现这受操作系统影响的。
                Stream reqStream = webRequest.GetRequestStream();//加入需要发送的参数
                reqStream.Write(reqBodyBytes, 0, reqBodyBytes.Length);
                reqStream.Close();
                using (StreamReader reader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch
            {
                throw;
            }
            return result;
        }


        /// <summary>
        /// 不带参请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static string WechatRequest(string url, MethodTypeEnum method, DataTypeEnum ContentType)
        {
            string result = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Method = method.ToString();
                webRequest.ContentType = "application/" + ContentType.ToString();
                using (StreamReader reader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
    }

    /// <summary>
    /// 带参数据类型
    /// </summary>
    public enum DataTypeEnum
    {
        json,
        xml,
        jsonp
    }

    /// <summary>
    /// 带参数据类型
    /// </summary>
    public enum MethodTypeEnum
    {
        Get,
        Post
    }
}