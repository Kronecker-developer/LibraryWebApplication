using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryWebApplication;
using System.Data.Entity.Infrastructure;
using System.Windows.Forms;

namespace LibraryWebApplication.Controllers
{
    public class BookController : Controller
    {
        private LibraryDBEntities db = new LibraryDBEntities();

        // GET: Book
        public ActionResult Index(string searchString)
        {
            var books = from b in db.Books
                      select b;
            if (!String.IsNullOrEmpty(searchString))
            {

                books = books.Where(b => b.Title.Contains(searchString) || b.ISBN.Contains(searchString));
            }
            return View(books.ToList());
        }

        // GET: Book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            PopulateAuthorsDropDownList();
            PopulatePublishersDropDownList();
            PopulateCategoriesDropDownList();
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,Title,ISBN,Availability,AuthorID,PublisherID,CategoryID")] Books books)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Books.Add(books);
                    db.AuthorsBooks.Add(new AuthorsBooks { BookID = books.BookID, AuthorID = books.AuthorID}) ;
                    db.BooksPublishers.Add(new BooksPublishers { BookID = books.BookID, PublisherID = books.PublisherID });
                    db.BooksCategories.Add(new BooksCategories { BookID = books.BookID, CategoryID = books.CategoryID });
                    books.Availability = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }
            PopulateAuthorsDropDownList(books.AuthorID);
            PopulatePublishersDropDownList(books.PublisherID);
            PopulateCategoriesDropDownList(books.CategoryID);
            return View(books);
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = db.Books.Find(id);
            var authorsBooks = db.AuthorsBooks.SingleOrDefault(ab => ab.BookID == books.BookID);
            var booksPublishers = db.BooksPublishers.SingleOrDefault(bp => bp.BookID == books.BookID);
            var booksCategories = db.BooksCategories.SingleOrDefault(bc => bc.BookID == books.BookID);
            if (books == null)
            {
                return HttpNotFound();
            }

            if (authorsBooks != null)
            {
                PopulateAuthorsDropDownList(authorsBooks.AuthorID);
            }
            else PopulateAuthorsDropDownList(books.AuthorID);
            if (booksPublishers != null)
            {
                PopulatePublishersDropDownList(booksPublishers.PublisherID);
            }
            else PopulatePublishersDropDownList(books.PublisherID);
            if (booksCategories != null)
            {
                PopulateCategoriesDropDownList(booksCategories.CategoryID);
            }
            else PopulateCategoriesDropDownList(books.CategoryID);

            return View(books);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,Title,ISBN,Availability,AuthorID,PublisherID,CategoryID")] Books books)
        {
            try
            {
                var authorsBooks = db.AuthorsBooks.SingleOrDefault(ab => ab.BookID == books.BookID);
                authorsBooks.AuthorID = books.AuthorID;
                var booksPublishers = db.BooksPublishers.SingleOrDefault(bp => bp.BookID == books.BookID);
                booksPublishers.PublisherID = books.PublisherID;
                var booksCategories = db.BooksCategories.SingleOrDefault(bc => bc.BookID == books.BookID);
                booksCategories.CategoryID = books.CategoryID;
                db.Entry(books).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }
            PopulateAuthorsDropDownList(books.AuthorID);
            PopulatePublishersDropDownList(books.PublisherID);
            PopulateCategoriesDropDownList(books.CategoryID);
            return View(books);
        }

        private void PopulateAuthorsDropDownList(object selectedAuthor = null)
        {
            var authorsQuery = from a in db.Authors
                orderby a.LastName
                select a;
            ViewBag.AuthorID = new SelectList(authorsQuery, "AuthorID", "LastName", selectedAuthor);
        }
        private void PopulatePublishersDropDownList(object selectedPublisher = null)
        {
            var publishersQuery = from p in db.Publishers
                               orderby p.Name
                               select p;
            ViewBag.PublisherID = new SelectList(publishersQuery, "PublisherID", "Name", selectedPublisher);
        }

        private void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var categoriesQuery = from c in db.Categories
                                  orderby c.Name
                                  select c;
            ViewBag.CategoryID = new SelectList(categoriesQuery, "CategoryID", "Name", selectedCategory);
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Books books = db.Books.Find(id);
                db.Books.Remove(books);
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
