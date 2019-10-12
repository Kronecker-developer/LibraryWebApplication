using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryWebApplication;

namespace LibraryWebApplication.Controllers
{
    public class PublisherController : Controller
    {
        private LibraryDBEntities db = new LibraryDBEntities();

        // GET: Publisher
        public ActionResult Index()
        {
            return View(db.Publishers.ToList());
        }

        // GET: Publisher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publishers publishers = db.Publishers.Find(id);
            if (publishers == null)
            {
                return HttpNotFound();
            }
            return View(publishers);
        }

        // GET: Publisher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publisher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Mark")] Publishers publishers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Publishers.Add(publishers);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }

            return View(publishers);
        }

        // GET: Publisher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publishers publishers = db.Publishers.Find(id);
            if (publishers == null)
            {
                return HttpNotFound();
            }
            return View(publishers);
        }

        // POST: Publisher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Mark")] Publishers publishers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(publishers).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }

            return View(publishers);
        }

        // GET: Publisher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publishers publishers = db.Publishers.Find(id);
            if (publishers == null)
            {
                return HttpNotFound();
            }
            return View(publishers);
        }

        // POST: Publisher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Publishers publishers = db.Publishers.Find(id);
                db.Publishers.Remove(publishers);
                db.SaveChanges();
            }
            catch (DataException de)
            {
                ModelState.AddModelError("", "Unable to save changes." + de.Message);
            }
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
