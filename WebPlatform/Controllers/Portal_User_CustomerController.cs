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


        public ActionResult ShowArea(Guid? cusID)
        {
            if(cusID == null)
            {
                cusID = db.Portal_Customer.First().ID;
            }

            var result = db.Portal_Customer_Area.Where(area => area.CusID == cusID);
            ViewBag.Area = result;
            return PartialView("ShowArea");
        }

        public ActionResult ShowStore(Guid? areaID)
        {
            if (areaID == null)
            {
                areaID = db.Portal_Customer_Area.First().ID;
            }

            var result = db.Portal_Customer_Store.Where(store => store.AreaID == areaID);
            ViewBag.Store = result;
            return PartialView("ShowStore");
        }

        // GET: Portal_User_Customer
        [Authorize(Roles = "admin")]
        public ActionResult Index(Guid? id)
        {
            ViewBag.userID = id;

            return View(db.Portal_User_Customer.Where(userinfo => userinfo.UserID==id).ToList());
        }

        // GET: Portal_User_Customer/Details/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public ActionResult Create(Guid? id)
        {
            ViewBag.userID = id;

            ViewBag.Customer = db.Portal_Customer.Where(cus => cus.ID != null);
            ViewBag.Area = db.Portal_Customer_Area.Where(cus => cus.ID != null);
            ViewBag.Store = db.Portal_Customer_Store.Where(cus => cus.ID != null);

            return View();
        }

        // POST: Portal_User_Customer/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "ID,UserID,CusID,AreaID,StoreID")] Portal_User_Customer portal_User_Customer)
        {
            if (ModelState.IsValid)
            {
                portal_User_Customer.ID = Guid.NewGuid();

                portal_User_Customer.CusID = new Guid(Request.Form["customer_dropdownlist"]);
                portal_User_Customer.AreaID = new Guid(Request.Form["area_dropdownlist"]);
                portal_User_Customer.StoreID = new Guid(Request.Form["store_dropdownlist"]);


                db.Portal_User_Customer.Add(portal_User_Customer);
                db.SaveChanges();
                return RedirectToAction(string.Format("Index?id={0}",portal_User_Customer.UserID));
            }

            return View(portal_User_Customer);
        }

        // GET: Portal_User_Customer/Edit/5
        [Authorize(Roles = "admin")]
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

            ViewBag.Customer = db.Portal_Customer.Where(cus => cus.ID != null);
            ViewBag.Area = db.Portal_Customer_Area.Where(cus => cus.ID != null);
            ViewBag.Store = db.Portal_Customer_Store.Where(cus => cus.ID != null);

            ViewBag.userID = portal_User_Customer.UserID;

            return View(portal_User_Customer);
        }

        // POST: Portal_User_Customer/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,UserID,CusID,AreaID,StoreID")] Portal_User_Customer portal_User_Customer)
        {
            if (ModelState.IsValid)
            {
                portal_User_Customer.CusID = new Guid(Request.Form["customer_dropdownlist"]);
                portal_User_Customer.AreaID = new Guid(Request.Form["area_dropdownlist"]);
                portal_User_Customer.StoreID = new Guid(Request.Form["store_dropdownlist"]);

                db.Entry(portal_User_Customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction(string.Format("Index?id={0}", portal_User_Customer.UserID));
            }
            return View(portal_User_Customer);
        }

        // GET: Portal_User_Customer/Delete/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
