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

        //获取萤石直播设备列表
        /// <summary>
        /// 获取萤石直播设备列表
        /// </summary>
        /// <returns></returns>
        public ActionResult YSLive()
        {
            YsAPI ys;

            IEnumerable<Cloud_YS_User> ysUserList = db.Cloud_YS_User.Where(userid => userid.UserID == GetUserID()).ToList();
            string appKey = ysUserList.ToList()[0].YsAppKey;
            string secret = ysUserList.ToList()[0].YsSecret;

            //为空则是子账号
            if(string.IsNullOrEmpty(appKey) && string.IsNullOrEmpty(secret))
            {
                
            }

            ys = new YsAPI("1f03ba35bf5d4c92af54a6cd2ee1fe3e", "ecc176d67e0d57f654ec35caa3095cef");

            DataTable ysLiveList = ys.getLiveLists();

            ViewBag.ysLiveList = ysLiveList;

            return View();
        }

        //获取单个摄像头直播地址
        /// <summary>
        /// 获取单个摄像头直播地址
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public ActionResult YSLiveVideo(string serialNo)
        {
            YsAPI ys = new YsAPI("1f03ba35bf5d4c92af54a6cd2ee1fe3e", "ecc176d67e0d57f654ec35caa3095cef");

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
        public ActionResult YSLiveVideoMulti()
        {
            YsAPI ys = new YsAPI("1f03ba35bf5d4c92af54a6cd2ee1fe3e", "ecc176d67e0d57f654ec35caa3095cef");

            int splitNum = 0;

            DataTable ysLiveList = ys.getLiveLists();
            Dictionary<int, string[]> liveAddressList = new Dictionary<int, string[]>();

            Dictionary<int, string> serialNos = new Dictionary<int, string>();

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
        public ActionResult YSLiveVideoMultiSet(int splitNum)
        {
            YsAPI ys = new YsAPI("1f03ba35bf5d4c92af54a6cd2ee1fe3e", "ecc176d67e0d57f654ec35caa3095cef");

            DataTable ysLiveList = ys.getLiveLists();

            ViewBag.ysLiveList = ysLiveList;
            ViewBag.splitNum = splitNum;

            return View();
        }
    }
}