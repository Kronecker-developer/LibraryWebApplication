using LibraryWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryWebApplication.Controllers
{
    public class UsersAndBooksReturnController : Controller
    {
        private LibraryDBEntities db = new LibraryDBEntities();
        // GET: UsersAndBooks
        public ActionResult Index(string searchStringBook, string searchStringFirstName, string searchStringLastName, string searchStringOIB)
        {
            List<Books> listOfBooks = new List<Books>();
            List<LibraryUsers> listOfLibraryUsers = new List<LibraryUsers>();
            UsersAndBooksReturnModel usersAndBooksReturnModel = new UsersAndBooksReturnModel();
            var books = from b in db.Books
                        where b.Availability == false
                        select b;

            if (!String.IsNullOrEmpty(searchStringBook))
            {

                books = books.Where(b => b.Title.Contains(searchStringBook) || b.ISBN.Contains(searchStringBook));
            }

            listOfBooks = books.ToList();

            var users = from u in db.LibraryUsers
                        select u;
            var booksUsers = db.BooksUsers
                .Where(bu => bu.ReturnDate == null)
                .Select(bu => bu.LibraryUserID)
                .ToList();
            users = users.Where(u => booksUsers.Contains(u.LibraryUserID));

            if (!String.IsNullOrEmpty(searchStringFirstName) || !String.IsNullOrEmpty(searchStringLastName) || !String.IsNullOrEmpty(searchStringOIB))
            {

                users = users.Where(u => u.FirstName.Contains(searchStringFirstName) & u.LastName.Contains(searchStringLastName) & u.OIB.Contains(searchStringOIB));
            }

            listOfLibraryUsers = users.ToList();

            usersAndBooksReturnModel.BooksViewModel = listOfBooks;
            usersAndBooksReturnModel.LibraryUsersViewModel = listOfLibraryUsers;

            return View(usersAndBooksReturnModel);
        }

        [HttpPost, ActionName("Return")]
        [ValidateAntiForgeryToken]
        public ActionResult Return(int? selectedBook)
        {
            try
            {
                if (selectedBook.HasValue)
                {
                    var booksUsers = db.BooksUsers.SingleOrDefault(bu => bu.BookID == selectedBook && bu.ReturnDate == null);
                    booksUsers.ReturnDate = DateTime.Now;
                    var books = db.Books.SingleOrDefault(b => b.BookID == selectedBook);
                    books.Availability = true;
                    db.SaveChanges();

                    System.Windows.Forms.MessageBox.Show("Book returned!");
                    return RedirectToAction("Index");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please select book to be returned!");
                    return new HttpStatusCodeResult(204);
                }
            }

            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
                return new HttpStatusCodeResult(204);
            }
        }
        public ActionResult Filter(int? selectedUser)
        {
            List<Books> listOfBooks = new List<Books>();
            List<LibraryUsers> listOfLibraryUsers = new List<LibraryUsers>();
            UsersAndBooksReturnModel usersAndBooksReturnModel = new UsersAndBooksReturnModel();
            var books = from b in db.Books
                        where b.Availability == false
                        select b;
            var users = from u in db.LibraryUsers
                        select u;

            if (selectedUser.HasValue)
            {
                var booksUsers = db.BooksUsers
                                .Where(bu => bu.LibraryUserID == selectedUser && bu.ReturnDate == null)
                                .Select(bu => bu.BookID)
                                .ToList();
                books = books.Where(b => booksUsers.Contains(b.BookID));
                users = users.Where(u => u.LibraryUserID == selectedUser);
            }

            listOfBooks = books.ToList();
            listOfLibraryUsers = users.ToList();

            usersAndBooksReturnModel.BooksViewModel = listOfBooks;
            usersAndBooksReturnModel.LibraryUsersViewModel = listOfLibraryUsers;

            return View("Return", usersAndBooksReturnModel);
        }
    }
}