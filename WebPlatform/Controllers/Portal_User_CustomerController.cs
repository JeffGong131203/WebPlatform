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
    public class Portal_User_CustomerController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Portal_User_Customer
        public ActionResult Index()
        {
            return View(db.Portal_User_Customer.ToList());
        }

        // GET: Portal_User_Customer/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_User_Customer portal_User_Customer = db.Portal_User_Customer.Find(id);
            if (portal_User_Customer == null)
            {
                return HttpNotFound();
            }
            return View(portal_User_Customer);
        }

        // GET: Portal_User_Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Portal_User_Customer/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,CusID,AreaID,StoreID")] Portal_User_Customer portal_User_Customer)
        {
            if (ModelState.IsValid)
            {
                portal_User_Customer.ID = Guid.NewGuid();
                db.Portal_User_Customer.Add(portal_User_Customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portal_User_Customer);
        }

        // GET: Portal_User_Customer/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_User_Customer portal_User_Customer = db.Portal_User_Customer.Find(id);
            if (portal_User_Customer == null)
            {
                return HttpNotFound();
            }
            return View(portal_User_Customer);
        }

        // POST: Portal_User_Customer/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,CusID,AreaID,StoreID")] Portal_User_Customer portal_User_Customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portal_User_Customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portal_User_Customer);
        }

        // GET: Portal_User_Customer/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_User_Customer portal_User_Customer = db.Portal_User_Customer.Find(id);
            if (portal_User_Customer == null)
            {
                return HttpNotFound();
            }
            return View(portal_User_Customer);
        }

        // POST: Portal_User_Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Portal_User_Customer portal_User_Customer = db.Portal_User_Customer.Find(id);
            db.Portal_User_Customer.Remove(portal_User_Customer);
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
