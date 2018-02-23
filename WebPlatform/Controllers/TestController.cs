using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatform.CloudAPI;

namespace WebPlatform.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult YsTest()
        {
            YsAPI ys = new YsAPI("1f03ba35bf5d4c92af54a6cd2ee1fe3e", "ecc176d67e0d57f654ec35caa3095cef");

            return Content(JsonConvert.SerializeObject(ys.getLiveLists(), Formatting.Indented));
        }
    }
}