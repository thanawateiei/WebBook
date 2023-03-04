using System.ComponentModel.DataAnnotations;
using WebBook.Models;

namespace WebBook.ViewModels
{
    public class BookViewModel
    {
        
        public string BookId { get; set; }
        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public int? PublicationYear { get; set; }
        public string? Publisher { get; set; }
        public string? BookCover { get; set; }
        public IFormFile? Bookimg { get; set; }
        public string? BookDetail { get; set; }
        public string? CallNumber { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? BookType1 { get; set; }
        public int? BookType2 { get; set; }
        public int? BookType3 { get; set; }
        public int? BookType4 { get; set; }
        public int? BookType5 { get; set; }
        public int? BookType6 { get; set; }
        public int? BookType7 { get; set; }
        public int? BookType8 { get; set; }
        public int? BookType9 { get; set; }
        public int? BookType10 { get; set; }


    }
    public class CBTViewModel
    {
        public int CbtId { get; set; }
        public int? BookId { get; set; }
        public string? BookTypeName { get; set; }
        public bool? CheckBt { get; set; }

    }
    public class BackCBTViewModel
    {
        public int CbtId { get; set; }
        public int? BookId { get; set; }
        public int? BookTypeId { get; set; }
        public bool? CheckBt { get; set; }

    }


}
