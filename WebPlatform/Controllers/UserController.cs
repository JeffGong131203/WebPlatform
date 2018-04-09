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
    public class UserController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: User
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.Portal_User.ToList());
        }

        // GET: User/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_User portal_User = db.Portal_User.Find(id);
            if (portal_User == null)
            {
                return HttpNotFound();
            }
            return View(portal_User);
        }

        // GET: User/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "ID,Loginno,Loginpsw,IsUsed")] Portal_User portal_User)
        {
            if (ModelState.IsValid)
            {
                portal_User.ID = Guid.NewGuid();
                db.Portal_User.Add(portal_User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portal_User);
        }

        // GET: User/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_User portal_User = db.Portal_User.Find(id);
            if (portal_User == null)
            {
                return HttpNotFound();
            }
            return View(portal_User);
        }

        // POST: User/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,Loginno,Loginpsw,IsUsed")] Portal_User portal_User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portal_User).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portal_User);
        }

        // GET: User/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_User portal_User = db.Portal_User.Find(id);
            if (portal_User == null)
            {
                return HttpNotFound();
            }
            return View(portal_User);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Portal_User portal_User = db.Portal_User.Find(id);
            db.Portal_User.Remove(portal_User);
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
