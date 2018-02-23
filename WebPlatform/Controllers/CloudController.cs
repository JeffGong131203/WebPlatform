using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatform.CloudAPI;

namespace WebPlatform.Controllers
{
    public class CloudController : Controller
    {
        // GET: Cloud
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult YSLive()
        {
            YsAPI ys = new YsAPI("1f03ba35bf5d4c92af54a6cd2ee1fe3e", "ecc176d67e0d57f654ec35caa3095cef");

            DataTable ysLiveList = ys.getLiveLists();

            ViewBag.ysLiveList = ysLiveList;

            return View();
        }

        public ActionResult YSLiveVideo(string serialNo)
        {
            YsAPI ys = new YsAPI("1f03ba35bf5d4c92af54a6cd2ee1fe3e", "ecc176d67e0d57f654ec35caa3095cef");

            DataTable ysLiveList = ys.getLiveLists();

            string rtmpAddress = string.Empty;
            string httpAddress = string.Empty;

            foreach (DataRow dr in ysLiveList.Rows)
            {
                if(serialNo == dr["deviceSerial"].ToString())
                {
                    rtmpAddress = dr["hdAddress"].ToString();
                    httpAddress = dr["rtmpHd"].ToString();
                }
            }

            ViewBag.rtmpAddress = rtmpAddress;
            ViewBag.httpAddress = httpAddress;

            return View();
        }
    }
}