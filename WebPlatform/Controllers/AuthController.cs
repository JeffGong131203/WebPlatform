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
    public class AuthController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Auth
        public ActionResult Index()
        {
            return View(db.Portal_Auth.ToList());
        }

        // GET: Auth/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Auth portal_Auth = db.Portal_Auth.Find(id);
            if (portal_Auth == null)
            {
                return HttpNotFound();
            }
            return View(portal_Auth);
        }

        // GET: Auth/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AuthCode,AuthName")] Portal_Auth portal_Auth)
        {
            if (ModelState.IsValid)
            {
                portal_Auth.ID = Guid.NewGuid();
                db.Portal_Auth.Add(portal_Auth);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portal_Auth);
        }

        // GET: Auth/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Auth portal_Auth = db.Portal_Auth.Find(id);
            if (portal_Auth == null)
            {
                return HttpNotFound();
            }
            return View(portal_Auth);
        }

        // POST: Auth/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AuthCode,AuthName")] Portal_Auth portal_Auth)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portal_Auth).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portal_Auth);
        }

        // GET: Auth/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Auth portal_Auth = db.Portal_Auth.Find(id);
            if (portal_Auth == null)
            {
                return HttpNotFound();
            }
            return View(portal_Auth);
        }

        // POST: Auth/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Portal_Auth portal_Auth = db.Portal_Auth.Find(id);
            db.Portal_Auth.Remove(portal_Auth);
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
