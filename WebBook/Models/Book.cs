using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class Book
    {
        public string BookId { get; set; } = null!;
        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public int? PublicationYear { get; set; }
        public string? Publisher { get; set; }
        public string? CallNumber { get; set; }
        public string? BookCover { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? BookLang { get; set; }
        public string? BookDetail { get; set; }
        public int BookType1 { get; set; }
        public int BookType2 { get; set; }
        public int BookType3 { get; set; }
        public int BookType4 { get; set; }
        public int BookType5 { get; set; }
        public int BookType6 { get; set; }
        public int BookType7 { get; set; }
        public int BookType8 { get; set; }
        public int BookType9 { get; set; }
        public int BookType10 { get; set; }
    }
}
