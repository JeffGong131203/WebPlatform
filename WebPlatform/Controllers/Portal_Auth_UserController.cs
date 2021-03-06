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
    public class Portal_Auth_UserController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Portal_Auth_User
        [Authorize(Roles = "admin")]
        public ActionResult Index(Guid? id)
        {
            ViewBag.userID = id;

            return View(db.Portal_Auth_User.Where(userAuth => userAuth.UserID == id).ToList());
        }

        // GET: Portal_Auth_User/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Auth_User portal_Auth_User = db.Portal_Auth_User.Find(id);
            if (portal_Auth_User == null)
            {
                return HttpNotFound();
            }
            return View(portal_Auth_User);
        }

        // GET: Portal_Auth_User/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create(Guid? userid)
        {
            ViewBag.userID = userid;

            var authList = db.Portal_Auth.ToList();
            var selectList = new SelectList(authList, "ID", "AuthName");
            var selectItemList = new List<SelectListItem>();

            selectItemList.AddRange(selectList);

            ViewBag.authList = selectItemList;

            return View();
        }

        // POST: Portal_Auth_User/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "ID,AuthID,UserID")] Portal_Auth_User portal_Auth_User)
        {
            if (ModelState.IsValid)
            {
                portal_Auth_User.ID = Guid.NewGuid();

                //权限赋值
                portal_Auth_User.AuthID = new Guid(Request.Form["authList"]);

                db.Portal_Auth_User.Add(portal_Auth_User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portal_Auth_User);
        }

        // GET: Portal_Auth_User/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Auth_User portal_Auth_User = db.Portal_Auth_User.Find(id);
            if (portal_Auth_User == null)
            {
                return HttpNotFound();
            }
            return View(portal_Auth_User);
        }

        // POST: Portal_Auth_User/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,AuthID,UserID")] Portal_Auth_User portal_Auth_User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portal_Auth_User).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portal_Auth_User);
        }

        // GET: Portal_Auth_User/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portal_Auth_User portal_Auth_User = db.Portal_Auth_User.Find(id);
            if (portal_Auth_User == null)
            {
                return HttpNotFound();
            }
            return View(portal_Auth_User);
        }

        // POST: Portal_Auth_User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Portal_Auth_User portal_Auth_User = db.Portal_Auth_User.Find(id);
            db.Portal_Auth_User.Remove(portal_Auth_User);
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
