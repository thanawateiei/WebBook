using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class LocationViewModel
    {
        [Key]
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? LocationDetail { get; set; }
    }


}
