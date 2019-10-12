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
    public class LibraryUserController : Controller
    {
        private LibraryDBEntities db = new LibraryDBEntities();

        // GET: LibraryUser
        public ActionResult Index()
        {
            return View(db.LibraryUsers.ToList());
        }

        // GET: LibraryUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryUsers libraryUsers = db.LibraryUsers.Find(id);
            if (libraryUsers == null)
            {
                return HttpNotFound();
            }
            return View(libraryUsers);
        }

        // GET: LibraryUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibraryUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,OIB")] LibraryUsers libraryUsers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.LibraryUsers.Add(libraryUsers);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }


            return View(libraryUsers);
        }

        // GET: LibraryUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryUsers libraryUsers = db.LibraryUsers.Find(id);
            if (libraryUsers == null)
            {
                return HttpNotFound();
            }
            return View(libraryUsers);
        }

        // POST: LibraryUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FirstName,LastName,OIB")] LibraryUsers libraryUsers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(libraryUsers).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException de)
            {
                ModelState.AddModelError("", "Unable to save changes."+de.Message);
            }

            return View(libraryUsers);
        }

        // GET: LibraryUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryUsers libraryUsers = db.LibraryUsers.Find(id);
            if (libraryUsers == null)
            {
                return HttpNotFound();
            }
            return View(libraryUsers);
        }

        // POST: LibraryUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                LibraryUsers libraryUsers = db.LibraryUsers.Find(id);
                db.LibraryUsers.Remove(libraryUsers);
                db.SaveChanges();
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
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
