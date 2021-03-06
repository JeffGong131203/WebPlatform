﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        private void GetUserYSAccount(out string accountID, out string appKey, out string secret)
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
            string recUrl = string.Format("ezopen://open.ys7.com/{0}/1.rec", sno);

            YsAPI ys;
            //string accountID = string.Empty;
            string appKey = System.Configuration.ConfigurationManager.AppSettings["YsAppKey"].ToString();
            string secret = System.Configuration.ConfigurationManager.AppSettings["YsSecret"].ToString();

            //GetUserYSAccount(out accountID, out appKey, out secret);
            ys = new YsAPI(appKey, secret);

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
        public ActionResult YSAccountManage(int pageStart = 0, int pageSize = 10)
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

            accountID = ys.addSubAccount(accountName, accountPsw);
            if (!string.IsNullOrEmpty(accountID))
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
                ys = new YsAPI(appKey, secret);
            }

            DataTable ysLiveList = ys.getDeviceLists(0, 0);

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
        [Authorize]
        public ActionResult YSLiveVideoMulti(string ysm)
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

            //Guid userid = GetUserID();
            //IEnumerable<YSMultiSet> ysMultiSetList = db.YSMultiSet.Where(ysm => ysm.UserID == userid).ToList();

            //for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
            //{
            //    string key = Request.Form.AllKeys[i];
            //    if (key == "splitNum")
            //    {
            //        splitNum = int.Parse(Request.Form[key]);
            //    }
            //    else
            //    {
            //        if (key.Contains("SEL_"))
            //        {
            //            int snoInd = int.Parse(key.Split("_".ToArray())[1]);

            //            string sno = Request.Form[key];

            //            serialNos.Add(snoInd, sno);
            //        }
            //    }
            //}

            ////Edit
            //if (serialNos.Count > 0 && ysMultiSetList.Count() > 0)
            //{
            //    YSMultiSet ysm = ysMultiSetList.ToList()[0];
            //    ysm.YSMultiSetJson = JsonConvert.SerializeObject(serialNos, Formatting.Indented);

            //    db.Entry(ysm).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            ////Create
            //if (serialNos.Count > 0 && ysMultiSetList.Count() == 0)
            //{
            //    YSMultiSet ysm = new YSMultiSet();
            //    ysm.ID = Guid.NewGuid();
            //    ysm.UserID = GetUserID();
            //    ysm.YSMultiSetJson = JsonConvert.SerializeObject(serialNos, Formatting.Indented);

            //    db.YSMultiSet.Add(ysm);
            //    db.SaveChanges();
            //}

            ////Load Set
            //if (serialNos.Count == 0 && ysMultiSetList.Count() > 0)
            //{
            //    serialNos = JsonConvert.DeserializeObject<Dictionary<int, string>>(ysMultiSetList.ToList()[0].YSMultiSetJson);
            //}

            serialNos = JsonConvert.DeserializeObject<Dictionary<int, string>>(ysm);

            foreach (DataRow dr in ysLiveList.Rows)
            {
                foreach (KeyValuePair<int, string> sno in serialNos)
                {
                    if (sno.Value == dr["deviceSerial"].ToString() && dr["status"].ToString() == "1")
                    {
                        if (!liveAddressList.ContainsKey(sno.Key))
                        {
                            liveAddressList.Add(sno.Key, new string[2] { dr["hdAddress"].ToString(), dr["rtmpHd"].ToString() });
                        }
                    }
                }
            }

            ViewBag.liveAddressList = liveAddressList;
            ViewBag.splitNum = splitNum;

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

            Guid userid = GetUserID();
            IEnumerable<YSMultiSet> ysMultiSetList = db.YSMultiSet.Where(ysm => ysm.UserID == userid).ToList();

            for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
            {
                string key = Request.Form.AllKeys[i];
                if (key == "splitNum")
                {
                    splitNum = int.Parse(Request.Form[key]);
                }
                else
                {
                    if (key.Contains("SEL_"))
                    {
                        int snoInd = int.Parse(key.Split("_".ToArray())[1]);

                        string sno = Request.Form[key];

                        serialNos.Add(snoInd, sno);
                    }
                }
            }

            //Edit
            if (serialNos.Count > 0 && ysMultiSetList.Count() > 0)
            {
                YSMultiSet ysm = ysMultiSetList.ToList()[0];
                ysm.YSMultiSetJson = JsonConvert.SerializeObject(serialNos, Formatting.Indented);

                db.Entry(ysm).State = EntityState.Modified;
                db.SaveChanges();
            }

            //Create
            if (serialNos.Count > 0 && ysMultiSetList.Count() == 0)
            {
                YSMultiSet ysm = new YSMultiSet();
                ysm.ID = Guid.NewGuid();
                ysm.UserID = GetUserID();
                ysm.YSMultiSetJson = JsonConvert.SerializeObject(serialNos, Formatting.Indented);

                db.YSMultiSet.Add(ysm);
                db.SaveChanges();
            }

            //Load Set
            if (serialNos.Count == 0 && ysMultiSetList.Count() > 0)
            {
                serialNos = JsonConvert.DeserializeObject<Dictionary<int, string>>(ysMultiSetList.ToList()[0].YSMultiSetJson);
            }

            foreach (DataRow dr in ysLiveList.Rows)
            {
                foreach (KeyValuePair<int, string> sno in serialNos)
                {
                    if (sno.Value == dr["deviceSerial"].ToString() && dr["status"].ToString() == "1")
                    {
                        if (!liveAddressList.ContainsKey(sno.Key))
                        {
                            liveAddressList.Add(sno.Key, new string[2] { dr["hdAddress"].ToString(), dr["rtmpHd"].ToString() });
                        }
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
        public ActionResult YSLiveVideoMultiSet(int splitNum, bool reSet)
        {
            Guid userid = GetUserID();
            IEnumerable<YSMultiSet> ysMultiSetList = db.YSMultiSet.Where(ysm => ysm.UserID == userid).ToList();

            if (!reSet && ysMultiSetList.Count() > 0)
            {
                return RedirectToAction("YSLiveVideoMulti", new { ysm = ysMultiSetList.ToList()[0].YSMultiSetJson });
            }

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

            DataTable ysLiveList = ys.getDeviceLists(0, 0);

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

                    //ret += ys.addSubAccount()

                }
            }

            ViewBag.retMsg = ret;

            return View("YSManage");
        }


        /// <summary>
        /// 批量添加萤石子账号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddSubAccounts()
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

                string[] acctNameList = DistinctAcctName(dev);

                Dictionary<string, string> dicSubAcct = new Dictionary<string, string>();

                for (int i = 0; i < acctNameList.Count(); i++)
                {
                    string acctName = acctNameList[i].ToString();

                    string[] subDevList = DistinctDevList(dev, acctName);

                    string accountID = ys.addSubAccount(acctName, acctName);
                    if (!string.IsNullOrEmpty(accountID))
                    {
                        ret += ys.SetSubAccountPolicy(accountID, new string[] { }, subDevList);
                    }
                    else//已存在子账号，获取后添加权限
                    {
                        accountID = ys.getSubAccountID(acctName);

                        ret += ys.SetSubAccountPolicy(accountID, new string[] { }, subDevList);
                    }

                    dicSubAcct.Add(acctName, accountID);
                }

                //平台帐号批处理
                foreach (KeyValuePair<string, string> kvp in dicSubAcct)
                {
                    string loginno = kvp.Key;
                    string accountID = kvp.Value;
                    Guid userid = Guid.Empty;

                    IEnumerable<Portal_User> userList = db.Portal_User.Where(u => u.Loginno == loginno);

                    //已有帐号
                    if (userList.ToList().Count > 0)
                    {
                        userid = userList.ToList()[0].ID;
                    }
                    else//新建帐号
                    {
                        Portal_User user_info = new Portal_User();
                        user_info.ID = Guid.NewGuid();
                        user_info.Loginno = loginno;
                        user_info.Loginpsw = loginno;
                        user_info.IsUsed = true;

                        db.Portal_User.Add(user_info);

                        userid = user_info.ID;
                    }

                    //Cloud_YS_User
                    IEnumerable<Cloud_YS_User> ysUserList = db.Cloud_YS_User.Where(ysu => ysu.UserID == userid);

                    if(ysUserList.ToList().Count == 0)
                    {
                        Cloud_YS_User ysInfo = new Cloud_YS_User();
                        ysInfo.ID = Guid.NewGuid();
                        ysInfo.UserID = userid;
                        ysInfo.YsAccount = accountID;

                        db.Cloud_YS_User.Add(ysInfo);
                    }
                }

                db.SaveChanges();
            }

            ViewBag.retMsg = ret;

            return View("YSManage");
        }

        private string[] DistinctAcctName(JArray devList)
        {
            ArrayList retArray = new ArrayList();

            for (int i = 0; i < devList.Count(); i++)
            {
                string acctName = devList[i]["AccountName"].ToString();

                if (!retArray.Contains(acctName))
                {
                    retArray.Add(acctName);
                }
            }

            string[] ret = (string[])retArray.ToArray(typeof(string));

            return ret;
        }

        private string[] DistinctDevList(JArray devList, string acctName)
        {
            ArrayList retArray = new ArrayList();

            for (int i = 0; i < devList.Count(); i++)
            {
                if (devList[i]["AccountName"].ToString() == acctName)
                {
                    retArray.Add(devList[i]["DevList"].ToString());
                }
            }

            string[] ret = (string[])retArray.ToArray(typeof(string));

            return ret;

        }
    }
}