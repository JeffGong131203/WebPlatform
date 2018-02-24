using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatform.Models;

namespace WebPlatform.Controllers
{
    public class HomeController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
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
                    return View("Index");
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}