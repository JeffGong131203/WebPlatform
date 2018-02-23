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
    public class Portal_Auth_MenuController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Portal_Auth_Menu
        public ActionResult Index()
        {
            return View(db.Portal_Auth_Menu.ToList());
        }

        // GET: Portal_Auth_Menu/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Auth_Menu portal_Auth_Menu = db.Portal_Auth_Menu.Find(id);
            if (portal_Auth_Menu == null)
            {
                return HttpNotFound();
            }
            return View(portal_Auth_Menu);
        }

        // GET: Portal_Auth_Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Portal_Auth_Menu/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AuthID,MenuID")] Portal_Auth_Menu portal_Auth_Menu)
        {
            if (ModelState.IsValid)
            {
                portal_Auth_Menu.ID = Guid.NewGuid();
                db.Portal_Auth_Menu.Add(portal_Auth_Menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portal_Auth_Menu);
        }

        // GET: Portal_Auth_Menu/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Auth_Menu portal_Auth_Menu = db.Portal_Auth_Menu.Find(id);
            if (portal_Auth_Menu == null)
            {
                return HttpNotFound();
            }
            return View(portal_Auth_Menu);
        }

        // POST: Portal_Auth_Menu/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AuthID,MenuID")] Portal_Auth_Menu portal_Auth_Menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portal_Auth_Menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portal_Auth_Menu);
        }

        // GET: Portal_Auth_Menu/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Auth_Menu portal_Auth_Menu = db.Portal_Auth_Menu.Find(id);
            if (portal_Auth_Menu == null)
            {
                return HttpNotFound();
            }
            return View(portal_Auth_Menu);
        }

        // POST: Portal_Auth_Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Portal_Auth_Menu portal_Auth_Menu = db.Portal_Auth_Menu.Find(id);
            db.Portal_Auth_Menu.Remove(portal_Auth_Menu);
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
