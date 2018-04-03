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
        private string _subAccountID = string.Empty;

        public YsAPI(string appKey, string secret)
        {
            _appKey = appKey;
            _secret = secret;
        }

        public YsAPI(string subAccountID)
        {
            _subAccountID = subAccountID;

            _appKey = System.Configuration.ConfigurationManager.AppSettings["YsAppKey"].ToString();
            _secret = System.Configuration.ConfigurationManager.AppSettings["YsSecret"].ToString();
        }

        /// <summary>
        /// 接口调用错误处理  TODO
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parapeters"></param>
        /// <param name="retJson"></param>
        private void APIError(string url, Dictionary<string, string> parapeters, JObject retJson)
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
                APIError(url, parameters, jStr);
            }

            //获取子账号AccessToken
            if (string.IsNullOrEmpty(_subAccountID))
            {
                url = "https://open.ys7.com/api/lapp/ram/token/get";

                parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", retStr);
                parameters.Add("accountId", _subAccountID);

                jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                if (jStr["code"].ToString() == "200")
                {
                    retStr = jStr["data"]["accessToken"].ToString();
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);
                }
            }

            return retStr;
        }

        /// <summary>
        /// 创建子账号
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string addSubAccount(string accountName,string password)
        {
            string accountID = string.Empty;

            return accountID;
        }

        /// <summary>
        /// 子账号授权
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="permisson"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public string SetSubAccountPolicy(string accountID,string[] permisson,string[] resource)
        {
            string retCode = string.Empty;

            return retCode;
        }


        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable getDeviceLists()
        {
            DataTable dtList = new DataTable();

            string accessToken = getAccessToken();
            if (!string.IsNullOrEmpty(accessToken))
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