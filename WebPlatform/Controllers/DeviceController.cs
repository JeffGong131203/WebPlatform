using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebPlatform.Models;

namespace WebPlatform.Controllers
{
    public class DeviceController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Device
        public ActionResult Index()
        {
            return View(db.Device_Info.ToList());
        }

        // GET: Device/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Device/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public ActionResult DeleteConfirmed(Guid id)
        {
            Device_Info device_Info = db.Device_Info.Find(id);
            db.Device_Info.Remove(device_Info);
            db.SaveChanges();
            return RedirectToAction("Index");
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
