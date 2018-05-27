using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebPlatform.Models;

namespace WebPlatform.Controllers
{
    public class DeviceController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Device
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Device_Info.ToList());
        }

        // GET: Device/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device_Info device_Info = db.Device_Info.Find(id);
            if (device_Info == null)
            {
                return HttpNotFound();
            }
            return View(device_Info);
        }

        // GET: Device/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Device/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,DevCode,DevName,ParentID,DevType,PropertyJson")] Device_Info device_Info)
        {
            //if (ModelState.IsValid)
            //{
                device_Info.ID = Guid.NewGuid();
                db.Device_Info.Add(device_Info);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}

            //return View(device_Info);
        }

        // GET: Device/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device_Info device_Info = db.Device_Info.Find(id);
            if (device_Info == null)
            {
                return HttpNotFound();
            }
            return View(device_Info);
        }

        // POST: Device/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,DevCode,DevName,ParentID,DevType,PropertyJson")] Device_Info device_Info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device_Info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(device_Info);
        }

        // GET: Device/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device_Info device_Info = db.Device_Info.Find(id);
            if (device_Info == null)
            {
                return HttpNotFound();
            }
            return View(device_Info);
        }

        // POST: Device/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Device_Info device_Info = db.Device_Info.Find(id);
            db.Device_Info.Remove(device_Info);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 空气传感器读取状态
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AirDeviceData(Guid devID)
        {
            WebComSvc.SerialPortData spd = new WebComSvc.SerialPortData();
            spd.Url = GetWebComUrl(devID);

            spd.SendData("01 03 00 00 00 05 85 c9",devID.ToString());

            string retData = spd.GetReciveData();

            DateTime t1 = DateTime.Now;
            DateTime t2 = DateTime.Now;
            TimeSpan ts = t2 - t1;

            while(string.IsNullOrEmpty(retData))
            {
                retData = spd.GetReciveData();

                Thread.Sleep(200);

                t2 = DateTime.Now;
                ts = t2 - t1;
                if(ts.TotalSeconds >5)
                {
                    break;
                }
            }

            Dictionary<string,string> dicRet= JsonConvert.DeserializeObject<Dictionary<string, string>>(retData);

            ArrayList retArray = ResolveAirDeviceData(dicRet["ReciveData"]);

            ViewBag.DevID = devID;
            ViewBag.retArray = retArray;

            return View();
        }

        /// <summary>
        /// 空气传感器解析数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ArrayList ResolveAirDeviceData(string data)
        {
            ArrayList retArray = new ArrayList();

            string[] byteData = data.Replace("0x", "@").Split("@".ToCharArray());

            if(byteData.Length == 15)
            {
                retArray.Add(Int32.Parse(byteData[4] + byteData[5], System.Globalization.NumberStyles.HexNumber).ToString());
                retArray.Add(Int32.Parse(byteData[6] + byteData[7], System.Globalization.NumberStyles.HexNumber).ToString());
                retArray.Add(Int32.Parse(byteData[8] + byteData[9], System.Globalization.NumberStyles.HexNumber).ToString());
                retArray.Add(Int32.Parse(byteData[10] + byteData[11], System.Globalization.NumberStyles.HexNumber).ToString());
                retArray.Add(Int32.Parse(byteData[12] + byteData[13], System.Globalization.NumberStyles.HexNumber).ToString());
            }

            return retArray;
        }

        private string GetWebComUrl(Guid devID)
        {
            string serverUrl = string.Empty;
            Device_Info dinfo = db.Device_Info.Find(devID);

            Device_Info webCom = db.Device_Info.Find(dinfo.ParentID);

            if(webCom.DevType == "COMPort")
            {
                serverUrl = webCom.PropertyJson;
            }

            return serverUrl;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
