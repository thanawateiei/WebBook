using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class Book
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public int? PublicationYear { get; set; }
        public string? Publisher { get; set; }
        public string? BookCover { get; set; }
        public string? BookDetail { get; set; }
        public int? BookType1 { get; set; }
        public int? BookType2 { get; set; }
        public string? BookType3 { get; set; }
        public string? BookType4 { get; set; }
        public string? BookType5 { get; set; }
    }
}
