using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class BookTypeViewModel
    {
        [Key]
        public int BookTypeId { get; set; }
        public string? BookTypeName { get; set; }
    }


}
