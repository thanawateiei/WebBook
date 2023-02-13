using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class StatusViewModel
    {
        [Key]
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
    }


}
