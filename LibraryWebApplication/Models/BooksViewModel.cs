using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWebApplication.Models
{
    public class BooksViewModel
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public Nullable<bool> Availability { get; set; }
    }
}