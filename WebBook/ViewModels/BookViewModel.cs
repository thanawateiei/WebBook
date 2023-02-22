using System.ComponentModel.DataAnnotations;
using WebBook.Models;

namespace WebBook.ViewModels
{
    public class BookViewModel
    {
        
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public int? PublicationYear { get; set; }
        public string? Publisher { get; set; }
        public string? BookCover { get; set; }
        public string? BookDetail { get; set; }
        public List<BackCBTViewModel> CheckBT { get; set; }
      

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
