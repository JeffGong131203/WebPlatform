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
    public class AreaController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Area
        public ActionResult Index()
        {
            return View(db.Portal_Customer_Area.ToList());
        }

        // GET: Area/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Customer_Area portal_Customer_Area = db.Portal_Customer_Area.Find(id);
            if (portal_Customer_Area == null)
            {
                return HttpNotFound();
            }
            return View(portal_Customer_Area);
        }

        // GET: Area/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Area/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CusID,AreaCode,AreaName")] Portal_Customer_Area portal_Customer_Area)
        {
            if (ModelState.IsValid)
            {
                portal_Customer_Area.ID = Guid.NewGuid();
                db.Portal_Customer_Area.Add(portal_Customer_Area);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portal_Customer_Area);
        }

        // GET: Area/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Customer_Area portal_Customer_Area = db.Portal_Customer_Area.Find(id);
            if (portal_Customer_Area == null)
            {
                return HttpNotFound();
            }
            return View(portal_Customer_Area);
        }

        // POST: Area/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CusID,AreaCode,AreaName")] Portal_Customer_Area portal_Customer_Area)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portal_Customer_Area).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portal_Customer_Area);
        }

        // GET: Area/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Customer_Area portal_Customer_Area = db.Portal_Customer_Area.Find(id);
            if (portal_Customer_Area == null)
            {
                return HttpNotFound();
            }
            return View(portal_Customer_Area);
        }

        // POST: Area/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Portal_Customer_Area portal_Customer_Area = db.Portal_Customer_Area.Find(id);
            db.Portal_Customer_Area.Remove(portal_Customer_Area);
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
