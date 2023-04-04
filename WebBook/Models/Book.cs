using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebBook.Models
{
    public partial class Book
    {
        public string BookId { get; set; } = null!;
        [Display(Name = "ชื่อหนังสือ")]
        public string? BookName { get; set; }
        [Display(Name = "ชื่อผู้แต่ง")]
        public string? AuthorName { get; set; }
        [Display(Name = "ปีที่พิมพ์")]
        public int? PublicationYear { get; set; }
        [Display(Name = "ชื่อผู้แต่ง")]
        public string? Publisher { get; set; }
        [Display(Name = "เลขหมู่หนังสือ")]
        public string? CallNumber { get; set; }
        [Display(Name = "รูปปกหนังสือ")]
        public string? BookCover { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [Display(Name = "ภาษาของหนังสือ")]
        public string? BookLang { get; set; }
        [Display(Name = "รายละเอียดหนังสือ")]
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
