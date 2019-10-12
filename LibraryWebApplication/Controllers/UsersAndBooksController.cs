using LibraryWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryWebApplication.Controllers
{
    public class UsersAndBooksController : Controller
    {
        private LibraryDBEntities db = new LibraryDBEntities();
        // GET: UsersAndBooks
        public ActionResult Index(string searchStringBook, string searchStringFirstName, string searchStringLastName, string searchStringOIB)
        {
            List<Books> listOfBooks = new List<Books>();
            List<LibraryUsers> listOfLibraryUsers = new List<LibraryUsers>();
            UsersAndBooksModel usersAndBooksModel = new UsersAndBooksModel();
            var books = from b in db.Books
                        where b.Availability == true
                        select b;

            if (!String.IsNullOrEmpty(searchStringBook))
            {

                books = books.Where(b => b.Title.Contains(searchStringBook) || b.ISBN.Contains(searchStringBook));
            }

            listOfBooks = books.ToList();

            var users = from u in db.LibraryUsers
                        select u;
            
            if (!String.IsNullOrEmpty(searchStringFirstName) || !String.IsNullOrEmpty(searchStringLastName) || !String.IsNullOrEmpty(searchStringOIB))
            {

                users = users.Where(u => u.FirstName.Contains(searchStringFirstName) & u.LastName.Contains(searchStringLastName) & u.OIB.Contains(searchStringOIB));
            }
            
            listOfLibraryUsers = users.ToList();

            usersAndBooksModel.BooksViewModel = listOfBooks;
            usersAndBooksModel.LibraryUsersViewModel = listOfLibraryUsers;

            return View(usersAndBooksModel);
        }

        [HttpPost, ActionName("Lend")]
        [ValidateAntiForgeryToken]
        public ActionResult Lend(int? selectedBook, int? selectedUser)
        {
            try
            {
                if (selectedBook.HasValue && selectedUser.HasValue)
                {
                    db.BooksUsers.Add(new BooksUsers { BookID = selectedBook, LibraryUserID = selectedUser, LendDate = DateTime.Now });
                    var books = db.Books.SingleOrDefault(b => b.BookID == selectedBook);
                    books.Availability = false;
                    db.SaveChanges();

                    System.Windows.Forms.MessageBox.Show("Book lended!");
                    return RedirectToAction("Index");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please select Library User and Book to be lended!");
                    return new HttpStatusCodeResult(204);
                }
            }

            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
                return new HttpStatusCodeResult(204);
            }

        }
    }
}