using System;

namespace LibCheck.Model
{
    public class BorrowedItem
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime ReturnDate { get; set; }
        public string LibraryName { get; set; }
    }
}
