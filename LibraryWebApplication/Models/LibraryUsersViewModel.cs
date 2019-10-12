using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWebApplication.Models
{
    public class LibraryUsersViewModel
    {
        public int LibraryUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OIB { get; set; }
    }
}