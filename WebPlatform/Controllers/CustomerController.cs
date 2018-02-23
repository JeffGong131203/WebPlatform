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
    public class CustomerController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Customer
        public ActionResult Index()
        {
            return View(db.Portal_Customer.ToList());
        }

        // GET: Customer/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Customer portal_Customer = db.Portal_Customer.Find(id);
            if (portal_Customer == null)
            {
                return HttpNotFound();
            }
            return View(portal_Customer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CusCode,CusName")] Portal_Customer portal_Customer)
        {
            if (ModelState.IsValid)
            {
                portal_Customer.ID = Guid.NewGuid();
                db.Portal_Customer.Add(portal_Customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portal_Customer);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Customer portal_Customer = db.Portal_Customer.Find(id);
            if (portal_Customer == null)
            {
                return HttpNotFound();
            }
            return View(portal_Customer);
        }

        // POST: Customer/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CusCode,CusName")] Portal_Customer portal_Customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portal_Customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portal_Customer);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Customer portal_Customer = db.Portal_Customer.Find(id);
            if (portal_Customer == null)
            {
                return HttpNotFound();
            }
            return View(portal_Customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Portal_Customer portal_Customer = db.Portal_Customer.Find(id);
            db.Portal_Customer.Remove(portal_Customer);
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
