using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatform.CloudAPI;
using WebPlatform.Models;

namespace WebPlatform.Controllers
{
    public class CloudController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Cloud
        public ActionResult Index()
        {
            return View();
        }

        private Guid GetUserID()
        {
            string userid = string.Empty;

            if (Request.Cookies["User"] != null)
            {
                HttpCookie _cookie = Request.Cookies["User"];
                userid = HttpUtility.UrlDecode(_cookie["UserID"]).ToString();

            }

            return new Guid(userid);
        }

        private void GetUserYSAccount(out string accountID,out string appKey,out string secret)
        {
            accountID = appKey = secret = string.Empty;

            Guid uid = GetUserID();

            IEnumerable<Cloud_YS_User> ysUserList = db.Cloud_YS_User.Where(userid => userid.UserID == uid).ToList();
            if (ysUserList.Count() > 0)
            {
                appKey = ysUserList.ToList()[0].YsAppKey;
                secret = ysUserList.ToList()[0].YsSecret;
                accountID = ysUserList.ToList()[0].YsAccount;
            }
        }
        
        /// <summary>
        /// 萤石回看
        /// </summary>
        /// <param name="sno"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult YSRec(string sno)
        {
            string recUrl = string.Format("ezopen://open.ys7.com/{0}/1.rec",sno);

            YsAPI ys;
            //string accountID = string.Empty;
            string appKey = System.Configuration.ConfigurationManager.AppSettings["YsAppKey"].ToString();
            string secret = System.Configuration.ConfigurationManager.AppSettings["YsSecret"].ToString();

            //GetUserYSAccount(out accountID, out appKey, out secret);
            ys = new YsAPI(appKey,secret);

            ViewBag.token = ys.getAccessToken(false);
            ViewBag.appKey = appKey;
            ViewBag.url = recUrl;

            return Redirect("~/CloudAPI");
        }

        /// <summary>
        /// 萤石子账号管理Get
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public ActionResult YSAccountManage(int pageStart = 0,int pageSize=10)
        {
            YsAPI ys;
            string accountID = string.Empty;
            string appKey = string.Empty;
            string secret = string.Empty;

            GetUserYSAccount(out accountID, out appKey, out secret);
            ys = new YsAPI(appKey, secret);

            int total = 0;
            int page = 0;
            int size = 0;

            string retJstr = ys.getSubAccountLists(pageStart, pageSize, out total, out page, out size);

            ViewBag.SubAccountList = retJstr;

            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult YSAccountManage()
        {
            string accountName = Request.Form["accountName"];
            string accountPsw = Request.Form["accountPsw"];
            string[] devlist = Request.Form["devlist"].Split(",".ToCharArray());

            YsAPI ys;
            string accountID = string.Empty;
            string appKey = string.Empty;
            string secret = string.Empty;

            GetUserYSAccount(out accountID, out appKey, out secret);
            ys = new YsAPI(appKey, secret);

            string retMsg = string.Empty;

            accountID = ys.addSubAccount(accountName,accountPsw);
            if(!string.IsNullOrEmpty(accountID))
            {
                retMsg = ys.SetSubAccountPolicy(accountID, new string[] { }, devlist);
            }
            else
            {
                retMsg = "萤石子账号创建失败";
            }

            ViewBag.errMsg = retMsg;

            return View();
        }

        //获取萤石直播设备列表
        /// <summary>
        /// 获取萤石直播设备列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult YSLive()
        {
            YsAPI ys;

            string accountID = string.Empty;
            string appKey = string.Empty;
            string secret = string.Empty;

            GetUserYSAccount(out accountID, out appKey, out secret);

            //为空则是子账号
            if (string.IsNullOrEmpty(appKey) && string.IsNullOrEmpty(secret))
            {
                ys = new YsAPI(accountID);
            }
            else
            {
                ys = new YsAPI(appKey,secret);
            }

            DataTable ysLiveList = ys.getDeviceLists(0,0);

            ViewBag.ysLiveList = ysLiveList;

            return View();
        }

        //获取单个摄像头直播地址
        /// <summary>
        /// 获取单个摄像头直播地址
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult YSLiveVideo(string serialNo)
        {
            YsAPI ys;

            string accountID = string.Empty;
            string appKey = string.Empty;
            string secret = string.Empty;

            GetUserYSAccount(out accountID, out appKey, out secret);

            //为空则是子账号
            if (string.IsNullOrEmpty(appKey) && string.IsNullOrEmpty(secret))
            {
                ys = new YsAPI(accountID);
            }
            else
            {
                ys = new YsAPI(appKey, secret);
            }

            DataTable ysLiveList = ys.getLiveLists();

            string rtmpAddress = string.Empty;
            string httpAddress = string.Empty;

            foreach (DataRow dr in ysLiveList.Rows)
            {
                if (serialNo == dr["deviceSerial"].ToString())
                {
                    rtmpAddress = dr["hdAddress"].ToString();
                    httpAddress = dr["rtmpHd"].ToString();
                }
            }

            ViewBag.rtmpAddress = rtmpAddress;
            ViewBag.httpAddress = httpAddress;

            return View();
        }

        /// <summary>
        /// 多设备直播显示
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult YSLiveVideoMulti()
        {
            YsAPI ys;

            string accountID = string.Empty;
            string appKey = string.Empty;
            string secret = string.Empty;

            GetUserYSAccount(out accountID, out appKey, out secret);

            //为空则是子账号
            if (string.IsNullOrEmpty(appKey) && string.IsNullOrEmpty(secret))
            {
                ys = new YsAPI(accountID);
            }
            else
            {
                ys = new YsAPI(appKey, secret);
            }

            int splitNum = 0;

            DataTable ysLiveList = ys.getLiveLists();
            Dictionary<int, string[]> liveAddressList = new Dictionary<int, string[]>();

            Dictionary<int, string> serialNos = new Dictionary<int, string>();

            for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
            {
                string key = Request.Form.AllKeys[i];
                if ( key== "splitNum")
                {
                    splitNum = int.Parse(Request.Form[key]);
                }
                else
                {
                    if(key.Contains("SEL_"))
                    {
                        int snoInd = int.Parse(key.Split("_".ToArray())[1]);

                        string sno = Request.Form[key];

                        serialNos.Add(snoInd, sno);
                    }
                }
            }

            foreach (DataRow dr in ysLiveList.Rows)
            {
                foreach (KeyValuePair<int, string> sno in serialNos)
                {
                    if (sno.Value == dr["deviceSerial"].ToString())
                    {
                        liveAddressList.Add(sno.Key, new string[2] { dr["hdAddress"].ToString(), dr["rtmpHd"].ToString() });
                    }
                }
            }

            ViewBag.liveAddressList = liveAddressList;
            ViewBag.splitNum = splitNum;

            return View();
        }



        /// <summary>
        /// 多设备直播监控设置
        /// </summary>
        /// <param name="splitNum"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult YSLiveVideoMultiSet(int splitNum)
        {
            YsAPI ys;

            string accountID = string.Empty;
            string appKey = string.Empty;
            string secret = string.Empty;

            GetUserYSAccount(out accountID, out appKey, out secret);

            //为空则是子账号
            if (string.IsNullOrEmpty(appKey) && string.IsNullOrEmpty(secret))
            {
                ys = new YsAPI(accountID);
            }
            else
            {
                ys = new YsAPI(appKey, secret);
            }

            DataTable ysLiveList = ys.getDeviceLists(0,0);

            ViewBag.ysLiveList = ysLiveList;
            ViewBag.splitNum = splitNum;

            return View();
        }

        /// <summary>
        /// 萤石批量管理
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public ActionResult YSManage()
        {
            return View();
        }

        /// <summary>
        /// 批量添加设备
        /// </summary>
        /// <param name="devList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddDevice()
        {
            string ret = string.Empty;

            string devList = Request["SendData"];

            if (!string.IsNullOrEmpty(devList))
            {
                YsAPI ys;
                //string accountID = string.Empty;
                string appKey = System.Configuration.ConfigurationManager.AppSettings["YsAppKey"].ToString();
                string secret = System.Configuration.ConfigurationManager.AppSettings["YsSecret"].ToString();

                //GetUserYSAccount(out accountID, out appKey, out secret);
                ys = new YsAPI(appKey, secret);

                //string token = ys.getAccessToken(false);

                JArray dev = Newtonsoft.Json.Linq.JArray.Parse(devList);

                for (int i = 0; i < dev.Count(); i++)
                {
                    ret += ys.AddDevice(dev[i]["deviceSerial"].ToString(), dev[i]["validateCode"].ToString());
                    ret += "\r\n";

                    ret += ys.SetDeviceName(dev[i]["deviceSerial"].ToString(), dev[i]["deviceName"].ToString());
                    ret += "\r\n";

                    ret += ys.SetLive(dev[i]["deviceSerial"].ToString() + ":1");
                    ret += "\r\n";
                }
            }

            ViewBag.retMsg = ret;

            return View("YSManage");
        }
    }
}