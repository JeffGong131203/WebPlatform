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
    public class MenuController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Menu
        public ActionResult Index()
        {
            return View(db.Portal_Menu.ToList());
        }

        // GET: Menu/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Menu portal_Menu = db.Portal_Menu.Find(id);
            if (portal_Menu == null)
            {
                return HttpNotFound();
            }
            return View(portal_Menu);
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menu/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MenuCode,MenuName,ParentID")] Portal_Menu portal_Menu)
        {
            if (ModelState.IsValid)
            {
                portal_Menu.ID = Guid.NewGuid();
                db.Portal_Menu.Add(portal_Menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portal_Menu);
        }

        // GET: Menu/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Menu portal_Menu = db.Portal_Menu.Find(id);
            if (portal_Menu == null)
            {
                return HttpNotFound();
            }
            return View(portal_Menu);
        }

        // POST: Menu/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MenuCode,MenuName,ParentID")] Portal_Menu portal_Menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portal_Menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portal_Menu);
        }

        // GET: Menu/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Menu portal_Menu = db.Portal_Menu.Find(id);
            if (portal_Menu == null)
            {
                return HttpNotFound();
            }
            return View(portal_Menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Portal_Menu portal_Menu = db.Portal_Menu.Find(id);
            db.Portal_Menu.Remove(portal_Menu);
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
