using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebPlatform.CloudAPI;
using WebPlatform.Models;

namespace WebPlatform.Controllers
{
    public class HomeController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        [Authorize]
        public ActionResult Index()
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

            int devCount, onCount, offCount, errCount;
            devCount = onCount = offCount = errCount = 0;

            //YS设备
            devCount = ysLiveList.Rows.Count;
            if (devCount > 0)
            {
                onCount = ysLiveList.Select("status='1'").Count();
                offCount = ysLiveList.Select("status='0'").Count();
            }

            //本地设备
            int localDevCount = 0;

            Guid userid = this.GetUserID();
            IEnumerable<Portal_User_Customer> userCusList = db.Portal_User_Customer.Where(ucs => ucs.UserID == userid).ToList();

            //某个客户下所有设备
            if (userCusList.ToList().Count > 0)
            {
                for (int i = 0; i < userCusList.ToList().Count; i++)
                {
                    Guid cusid = userCusList.ToList()[i].CusID.Value;

                    int devC = db.Device_Customer.Where(dev_u => dev_u.CustomerID == cusid).ToList().Count;

                    localDevCount += devC;
                }
            }

            ViewBag.devCount = devCount + localDevCount;
            ViewBag.onCount = onCount;
            ViewBag.offCount = offCount;
            ViewBag.errCount = errCount;

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

        public ActionResult Login()
        {
            return View();
        }

        [Authorize]
        public ActionResult Manage()
        {
            if (User.IsInRole("admin"))
            {
                return PartialView("Manage");
            }
            else
            {
                return Content("");
            }
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(string timeStamp)
        {
            Guid uid = GetUserID();

            string oldpsw = Request.Form["oldpsw"];

            string newpsw1 = Request.Form["newpsw1"];
            string newpsw2 = Request.Form["newpsw2"];

            if (newpsw1 != newpsw2)
            {
                ViewBag.errMsg = "两次密码输入不一致";
            }
            else
            {
                IEnumerable<Portal_User> userList = db.Portal_User.Where(userid => userid.ID == uid && userid.Loginpsw == oldpsw).ToList();

                if (userList.Count() > 0)
                {
                    Portal_User uInfo = userList.ToList()[0];

                    uInfo.Loginpsw = newpsw1;

                    db.Entry(uInfo).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.errMsg = "密码修改成功";
                }
                else
                {
                    ViewBag.errMsg = "原密码错误";
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string timeStamp)
        {
            string loginno = Request.Form["loginno"];
            string loginpsw = Request.Form["loginpsw"];

            Portal_User portal_User = db.Portal_User.FirstOrDefault(userInfo => userInfo.Loginno == loginno);

            if (portal_User != null)
            {
                if (loginpsw == portal_User.Loginpsw)
                {
                    string role = string.Empty;

                    var authList = db.Portal_Auth.Join(db.Portal_Auth_User, a => a.ID, u => u.AuthID, (a, u) => new { a.AuthCode, a.AuthName, u.UserID }).Where(user => user.UserID == portal_User.ID).ToList();
                    for (int i = 0; i < authList.Count(); i++)
                    {
                        if (i == 0)
                        {
                            role = authList[i].AuthName;
                        }
                        else
                        {
                            role += "," + authList[i].AuthName;
                        }
                    }

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, portal_User.Loginno, DateTime.Now, DateTime.Now.AddMinutes(800), true, role);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    //FormsAuthentication.SetAuthCookie()
                    System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                    HttpCookie _cookie = new HttpCookie("User");

                    _cookie.Values.Add("UserID", HttpUtility.UrlEncode(portal_User.ID.ToString()));
                    _cookie.Values.Add("Loginno", HttpUtility.UrlEncode(portal_User.Loginno));
                    Response.Cookies.Add(_cookie);

                    return Redirect("Index");
                }
                else
                {
                    ViewBag.errMsg = "账号密码错误";
                }
            }
            else
            {
                ViewBag.errMsg = "用户名不存在";
            }

            return View();

        }

        public ActionResult Logout()
        {
            if (Request.Cookies["User"] != null)
            {
                HttpCookie _cookie = Request.Cookies["User"];
                _cookie.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(_cookie);
            }

            FormsAuthentication.SignOut();

            //Redirect(Url.Action("Login", "Home"));

            return Redirect("Login");
        }

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}


        /// <summary>
        /// 用户授权访问门店数据
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MyStore()
        {
            Guid userid = GetUserID();

            Guid?[] storeIDArray = db.Portal_User_Customer.Where(u => u.UserID == userid).Select(u => u.StoreID).ToArray();
            Guid?[] cusIDArray = db.Portal_User_Customer.Where(u => u.UserID == userid).Select(u => u.CusID).ToArray();

            IEnumerable<Portal_Customer_Store> storeList = db.Portal_Customer_Store.Where(s => storeIDArray.Contains(s.ID));

            ViewBag.storeList = storeList;

            if (cusIDArray.Count() > 0)
            {
                ViewBag.cusID = cusIDArray[0].Value;
            }
            else
            {
                ViewBag.cusID = Guid.Empty;
            }

            return View();
        }

        /// <summary>
        /// 门店设备管理（设备分组，归属到门店）
        /// </summary>
        /// <param name="storeID"></param>
        /// <param name="cusID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult StoreDeviceSet(Guid storeID, Guid cusID)
        {
            //该客户下门店所有可用设备，排除所有已分配
            Guid?[] devIDArrayIn = db.Device_Customer_Store.Where(d => d.CusID == cusID).Select(d => d.DeviceID).ToArray();
            Guid?[] devIDArrayOut = db.Device_Customer.Where(d => d.CustomerID == cusID && !devIDArrayIn.Contains(d.DeviceID)).Select(d => d.DeviceID).ToArray();
            //该门店已分配设备
            Guid?[] devIDArrayStore = db.Device_Customer_Store.Where(d => d.CusID == cusID && d.StoreID == storeID).Select(d => d.DeviceID).ToArray();

            //可分配设备清单
            IEnumerable<Device_Info> devOutList = db.Device_Info.Where(d => devIDArrayOut.Contains(d.ID)).OrderBy(d => d.DevType).OrderBy(d => d.DevCode);

            //已分配设备清单
            IEnumerable<Device_Info> devInList = db.Device_Info.Where(d => devIDArrayStore.Contains(d.ID)).OrderBy(d => d.DevType).OrderBy(d => d.DevCode);

            Portal_Customer_Store storeInfo = db.Portal_Customer_Store.Find(storeID);

            ViewBag.devOutList = devOutList;
            ViewBag.devInList = devInList;
            ViewBag.storeID = storeID;
            ViewBag.areaID = storeInfo.AreaID;
            ViewBag.cusID = cusID;

            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult StoreDeviceSet(Guid storeID, Guid areaID, Guid cusID)
        {
            List<Guid> devIDArray = new List<Guid>();

            foreach (string key in Request.Form.Keys)
            {
                if (key.Contains("Sel"))
                {
                    string[] devIDStrArray = Request.Form[key].Split(",".ToCharArray());

                    foreach(string devStr in devIDStrArray)
                    {
                        devIDArray.Add(new Guid(devStr));
                    }
                }
            }

            foreach (Guid devID in devIDArray)
            {
                Device_Customer_Store dcs = new Device_Customer_Store();
                dcs.ID = Guid.NewGuid();
                dcs.DeviceID = devID;
                dcs.StoreID = storeID;
                dcs.AreaID = areaID;
                dcs.CusID = cusID;

                db.Device_Customer_Store.Add(dcs);
            }

            db.SaveChanges();

            return RedirectToAction("StoreDeviceSet", new { storeID = storeID, cusID = cusID });
        }


        [Authorize]
        public ActionResult DeleteStoreDevice(Guid devID, Guid storeID, Guid cusID)
        {
            Device_Customer_Store device_Info = db.Device_Customer_Store.First(d => d.DeviceID == devID);
            db.Device_Customer_Store.Remove(device_Info);
            db.SaveChanges();

            return RedirectToAction("StoreDeviceSet", new { storeID = storeID, cusID = cusID });
        }

        [Authorize]
        public ActionResult DeviceDataView(Guid storeID, Guid cusID)
        {
            Guid?[] devIDArray = db.Device_Customer_Store.Where(d => d.StoreID == storeID).Select(d => d.DeviceID).ToArray();
            IEnumerable<Device_Info> devList = db.Device_Info.Where(d => devIDArray.Contains(d.ID)).OrderBy(d => d.DevType).OrderBy(d => d.DevCode);

            ViewBag.devList = devList;
            ViewBag.storeID = storeID;
            ViewBag.cusID = cusID;

            return View();
        }

    }
}