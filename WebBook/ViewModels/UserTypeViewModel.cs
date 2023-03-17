using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class UserTypeViewModel
    {
        [Key]
        public int UserTypeId { get; set; }
        public int? Limit { get; set; }
        public string? UserTypeName { get; set; }
        public int? Enbook { get; set; }
        public int? Thbook { get; set; }

    }


}
