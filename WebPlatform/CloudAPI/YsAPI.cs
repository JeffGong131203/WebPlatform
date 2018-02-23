using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebPlatform.BLL;

namespace WebPlatform.CloudAPI
{
    public class YsAPI
    {
        private string _appKey = string.Empty;
        private string _secret = string.Empty;

        public YsAPI(string appKey, string secret)
        {
            _appKey = appKey;
            _secret = secret;
        }

        /// <summary>
        /// 接口调用错误处理  TODO
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parapeters"></param>
        /// <param name="retJson"></param>
        private void APIError(string url,Dictionary<string,string> parapeters,JObject retJson)
        { }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public string getAccessToken()
        {
            string url = "https://open.ys7.com/api/lapp/token/get";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("appKey", _appKey);
            parameters.Add("appSecret", _secret);

            string retStr = string.Empty;

            JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

            if (jStr["code"].ToString() == "200")
            {
                retStr = jStr["data"]["accessToken"].ToString();
            }
            else //API Error,Write Log
            {
                APIError(url,parameters,jStr);
            }

            return retStr;
        }


        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable getDeviceLists()
        {
            DataTable dtList = new DataTable();

            string accessToken = getAccessToken();
            if(!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/device/list";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);

                string retStr = string.Empty;

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                if (jStr["code"].ToString() == "200")
                {
                    dtList = JsonConvert.DeserializeObject<DataTable>(jStr["data"].ToString());
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);
                }
            }

            return dtList;
        }

        /// <summary>
        /// 获取直播设备列表及直播地址
        /// </summary>
        /// <returns></returns>
        public DataTable getLiveLists()
        {
            DataTable dtList = new DataTable();

            string accessToken = getAccessToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/live/video/list";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);

                string retStr = string.Empty;

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                if (jStr["code"].ToString() == "200")
                {
                    dtList = JsonConvert.DeserializeObject<DataTable>(jStr["data"].ToString());
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);
                }
            }

            return dtList;
        }

    }
}