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
    public class Cloud_YS_UserController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();

        // GET: Cloud_YS_User
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.Cloud_YS_User.ToList());
        }

        // GET: Cloud_YS_User/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cloud_YS_User cloud_YS_User = db.Cloud_YS_User.Find(id);
            if (cloud_YS_User == null)
            {
                return HttpNotFound();
            }
            return View(cloud_YS_User);
        }

        // GET: Cloud_YS_User/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cloud_YS_User/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "ID,UserID,YsAccount,YsPsw,YsAppKey,YsSecret")] Cloud_YS_User cloud_YS_User)
        {
            if (ModelState.IsValid)
            {
                cloud_YS_User.ID = Guid.NewGuid();
                db.Cloud_YS_User.Add(cloud_YS_User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cloud_YS_User);
        }

        // GET: Cloud_YS_User/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cloud_YS_User cloud_YS_User = db.Cloud_YS_User.Find(id);
            if (cloud_YS_User == null)
            {
                return HttpNotFound();
            }
            return View(cloud_YS_User);
        }

        // POST: Cloud_YS_User/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,UserID,YsAccount,YsPsw,YsAppKey,YsSecret")] Cloud_YS_User cloud_YS_User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cloud_YS_User).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cloud_YS_User);
        }

        // GET: Cloud_YS_User/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cloud_YS_User cloud_YS_User = db.Cloud_YS_User.Find(id);
            if (cloud_YS_User == null)
            {
                return HttpNotFound();
            }
            return View(cloud_YS_User);
        }

        // POST: Cloud_YS_User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Cloud_YS_User cloud_YS_User = db.Cloud_YS_User.Find(id);
            db.Cloud_YS_User.Remove(cloud_YS_User);
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
