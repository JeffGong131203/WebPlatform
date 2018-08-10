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
        public string getAccessToken(bool isSub)
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

                retStr = jStr.ToString();
            }

            //获取子账号AccessToken
            if (!string.IsNullOrEmpty(_subAccountID) && isSub)
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

                    retStr = jStr.ToString();
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
        public string addSubAccount(string accountName, string password)
        {
            string accountID = string.Empty;

            string accessToken = getAccessToken(false);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/ram/account/create";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);
                parameters.Add("accountName", accountName);
                parameters.Add("password", BLLHelper.UserMd5(_appKey + "#" + password).ToLower());

                string retStr = string.Empty;

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                if (jStr["code"].ToString() == "200")
                {
                    accountID = jStr["data"]["accountId"].ToString();
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);

                    retStr = jStr.ToString();
                }
            }

            return accountID;
        }

        /// <summary>
        /// 子账号授权
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="permisson"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public string SetSubAccountPolicy(string accountID, string[] permisson, string[] resource)
        {
            string retMsg = string.Empty;

            //string policy = "{ "Statement": [{ "Permission": "Get,Real",  "Resource": ["dev: 126044895","dev: C03225730"]}]}";
            //string policy = "{ \"Statement\": [{ \"Permission\": \"{0}\",  \"Resource\": [{1}]}]}";

            string strPermission = string.Empty;
            string strResource = string.Empty;

            if (permisson.Count() > 0)
            {
                for (int i = 0; i < permisson.Count(); i++)
                {
                    if (i == 0)
                    {
                        strPermission = permisson[i];
                    }
                    else
                    {
                        strPermission += "," + permisson[i];
                    }
                }
            }
            else
            {
                strPermission = "Get,Real";
            }

            if (resource.Count() > 0)
            {
                for (int i = 0; i < resource.Count(); i++)
                {
                    if (i == 0)
                    {
                        strResource = string.Format("\"dev:{0}\"", resource[i]);
                    }
                    else
                    {
                        strResource += "," + string.Format("\"dev:{0}\"", resource[i]);
                    }
                }
            }

            string accessToken = getAccessToken(false);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/ram/policy/set";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);
                parameters.Add("accountId", accountID);

                string strPolicy = "{\"Statement\": [{ \"Permission\": \"" + strPermission + "\",  \"Resource\": [" + strResource + "]}]}";

                parameters.Add("policy", strPolicy);

                string retStr = string.Empty;

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                if (jStr["code"].ToString() == "200")
                {
                    retMsg = jStr["msg"].ToString();
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);

                    retMsg = jStr.ToString();
                }
            }

            return retMsg;
        }

        /// <summary>
        /// 根据子账号名称获取子账号ID
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public string getSubAccountID(string accountName)
        {
            string retStr = string.Empty;

            string accessToken = getAccessToken(false);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/ram/account/get";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);
                parameters.Add("accountName", accountName);

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                if (jStr["code"].ToString() == "200")
                {
                    retStr = jStr["data"]["accountId"].ToString();
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);

                    retStr = jStr.ToString();
                }
            }

            return retStr;
        }

        /// <summary>
        /// 获取所有子账户列表
        /// </summary>
        /// <returns></returns>
        public string getSubAccountLists(int pageStart, int pageSize, out int total, out int page, out int size)
        {
            string retStr = string.Empty;

            total = 0;
            page = 0;
            size = 0;

            string accessToken = getAccessToken(false);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/ram/account/list";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);
                if (pageStart == 0 && pageSize == 0)
                {

                }
                else
                {
                    parameters.Add("pageStart", pageStart.ToString());
                    parameters.Add("pageSize", pageSize.ToString());
                }

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                if (jStr["code"].ToString() == "200")
                {
                    retStr = jStr.ToString();
                    total = int.Parse(jStr["page"]["total"].ToString());
                    page = int.Parse(jStr["page"]["page"].ToString());
                    size = int.Parse(jStr["page"]["size"].ToString());
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);

                    retStr = jStr.ToString();
                }
            }

            return retStr;
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable getDeviceLists(int pageStart, int pageSize)
        {
            DataTable dtList = new DataTable();

            int total = 0;
            int page = 0;
            int size = 0;

            string accessToken = getAccessToken(true);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/device/list";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);
                if (pageStart == 0 && pageSize == 0)
                {
                    parameters.Add("pageStart", 0.ToString());
                    parameters.Add("pageSize", 10.ToString());
                }
                else
                {
                    parameters.Add("pageStart", pageStart.ToString());
                    parameters.Add("pageSize", pageSize.ToString());

                }

                string retStr = string.Empty;

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                if (jStr["code"].ToString() == "200")
                {
                    dtList = JsonConvert.DeserializeObject<DataTable>(jStr["data"].ToString());
                    total = int.Parse(jStr["page"]["total"].ToString());
                    page = int.Parse(jStr["page"]["page"].ToString());
                    size = int.Parse(jStr["page"]["size"].ToString());

                    while (total > (page + 1) * size)
                    {
                        parameters["pageStart"] = (page + 1).ToString();

                        jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));
                        DataTable dt = new DataTable();

                        total = int.Parse(jStr["page"]["total"].ToString());
                        page = int.Parse(jStr["page"]["page"].ToString());
                        size = int.Parse(jStr["page"]["size"].ToString());

                        var retData = jStr["data"];

                        if (retData != null)
                        {
                            dt = JsonConvert.DeserializeObject<DataTable>(jStr["data"].ToString());
                            dtList.Merge(dt);
                        }
                        else
                        {
                            break;
                        }
                    }
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

            int total = 0;
            int page = 0;
            int size = 0;

            string accessToken = getAccessToken(true);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/live/video/list";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);

                parameters.Add("pageStart", 0.ToString());
                parameters.Add("pageSize", 10.ToString());

                string retStr = string.Empty;

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                if (jStr["code"].ToString() == "200")
                {
                    dtList = JsonConvert.DeserializeObject<DataTable>(jStr["data"].ToString());

                    total = int.Parse(jStr["page"]["total"].ToString());
                    page = int.Parse(jStr["page"]["page"].ToString());
                    size = int.Parse(jStr["page"]["size"].ToString());

                    while (total > (page + 1) * size)
                    {
                        parameters["pageStart"] = (page + 1).ToString();

                        jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));
                        DataTable dt = new DataTable();

                        total = int.Parse(jStr["page"]["total"].ToString());
                        page = int.Parse(jStr["page"]["page"].ToString());
                        size = int.Parse(jStr["page"]["size"].ToString());

                        var retData = jStr["data"];

                        if (retData != null)
                        {
                            dt = JsonConvert.DeserializeObject<DataTable>(jStr["data"].ToString());
                            dtList.Merge(dt);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);
                }
            }

            return dtList;
        }

        /// <summary>
        /// 主帐号添加设备
        /// </summary>
        /// <param name="deviceSerial"></param>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        public string AddDevice(string deviceSerial, string validateCode)
        {
            string ret = string.Empty;

            string accessToken = getAccessToken(false);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/device/add";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);
                parameters.Add("deviceSerial", deviceSerial);
                parameters.Add("validateCode", validateCode);

                string retStr = string.Empty;

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                ret = jStr.ToString();

                if (jStr["code"].ToString() == "200")
                {
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);

                    ret = jStr.ToString();
                }
            }

            return ret;
        }

        /// <summary>
        /// 批量开通直播
        /// </summary>
        /// <param name="LiveSrc"></param>
        /// <returns></returns>
        public string SetLive(string LiveSrc)
        {
            string ret = string.Empty;

            string accessToken = getAccessToken(false);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/live/video/open";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);
                parameters.Add("source", LiveSrc);

                string retStr = string.Empty;

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                ret = jStr.ToString();

                if (jStr["code"].ToString() == "200")
                {
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);

                    ret = jStr.ToString();
                }
            }

            return ret;
        }

        public string SetDeviceName(string deviceSerial, string deviceName)
        {
            string ret = string.Empty;

            string accessToken = getAccessToken(false);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/device/name/update";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);
                parameters.Add("deviceSerial", deviceSerial);
                parameters.Add("deviceName", deviceName);

                string retStr = string.Empty;

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                ret = jStr.ToString();

                if (jStr["code"].ToString() == "200")
                {
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);

                    ret = jStr.ToString();
                }
            }

            return ret;
        }

        /// <summary>
        /// 批量添加好友
        /// </summary>
        /// <param name="LiveSrc"></param>
        /// <returns></returns>
        public string AddFriend(string phone)
        {
            string ret = string.Empty;

            string accessToken = getAccessToken(false);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = "https://open.ys7.com/api/lapp/friend/add";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("accessToken", accessToken);
                parameters.Add("phone", phone);

                string retStr = string.Empty;

                JObject jStr = JObject.Parse(HTTPHelper.HttpPostResponse(url, parameters));

                ret = jStr.ToString();

                if (jStr["code"].ToString() == "200")
                {
                }
                else //API Error,Write Log
                {
                    APIError(url, parameters, jStr);

                    ret = jStr.ToString();
                }
            }

            return ret;
        }


    }
}