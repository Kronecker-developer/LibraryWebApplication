﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWebApplication.Models
{
    public class UsersAndBooksModel
    {
 
        public IEnumerable<Books> BooksViewModel { get; set; }
        public IEnumerable<LibraryUsers> LibraryUsersViewModel { get; set; }

    }
}