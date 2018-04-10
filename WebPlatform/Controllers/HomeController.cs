using System;
using System.Collections.Generic;
using System.Data;
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

            ViewBag.devCount = ysLiveList.Rows.Count;
            ViewBag.onCount = ysLiveList.Select("status='1'").Count();
            ViewBag.offCount = ysLiveList.Select("status='0'").Count();
            ViewBag.errCount = 0;

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
                    for(int i=0;i<authList.Count();i++)
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
         
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,portal_User.Loginno,DateTime.Now,DateTime.Now.AddMinutes(800),true,role);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    //FormsAuthentication.SetAuthCookie()
                    System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                    HttpCookie _cookie = new HttpCookie("User");

                    _cookie.Values.Add("UserID",HttpUtility.UrlEncode(portal_User.ID.ToString()));
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}