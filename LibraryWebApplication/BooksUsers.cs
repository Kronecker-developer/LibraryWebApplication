//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryWebApplication
{
    using System;
    using System.Collections.Generic;
    
    public partial class BooksUsers
    {
        public int BookUserID { get; set; }
        public System.DateTime LendDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public Nullable<int> BookID { get; set; }
        public Nullable<int> LibraryUserID { get; set; }
    
        public virtual Books Books { get; set; }
        public virtual LibraryUsers LibraryUsers { get; set; }
    }
}
