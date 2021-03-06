﻿using System;
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
    public class StoreController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Store
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.Portal_Customer_Store.ToList());
        }

        // GET: Store/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Customer_Store portal_Customer_Store = db.Portal_Customer_Store.Find(id);
            if (portal_Customer_Store == null)
            {
                return HttpNotFound();
            }
            return View(portal_Customer_Store);
        }

        // GET: Store/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            var areaList = db.Portal_Customer_Area.ToList();
            var selectList = new SelectList(areaList, "ID", "AreaName");
            var selectItemList = new List<SelectListItem>();

            selectItemList.AddRange(selectList);

            ViewBag.areaList = selectItemList;

            return View();
        }

        // POST: Store/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "ID,AreaID,StoreCode,StoreName")] Portal_Customer_Store portal_Customer_Store)
        {
            if (ModelState.IsValid)
            {
                portal_Customer_Store.ID = Guid.NewGuid();

                portal_Customer_Store.AreaID = new Guid(Request.Form["areaList"]);

                db.Portal_Customer_Store.Add(portal_Customer_Store);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portal_Customer_Store);
        }

        // GET: Store/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Customer_Store portal_Customer_Store = db.Portal_Customer_Store.Find(id);
            if (portal_Customer_Store == null)
            {
                return HttpNotFound();
            }
            return View(portal_Customer_Store);
        }

        // POST: Store/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,AreaID,StoreCode,StoreName")] Portal_Customer_Store portal_Customer_Store)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portal_Customer_Store).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portal_Customer_Store);
        }

        // GET: Store/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Customer_Store portal_Customer_Store = db.Portal_Customer_Store.Find(id);
            if (portal_Customer_Store == null)
            {
                return HttpNotFound();
            }
            return View(portal_Customer_Store);
        }

        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Portal_Customer_Store portal_Customer_Store = db.Portal_Customer_Store.Find(id);
            db.Portal_Customer_Store.Remove(portal_Customer_Store);
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
