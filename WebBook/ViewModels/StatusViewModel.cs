using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class StatusViewModel
    {
        [Key]
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
    }

    public class CountStatusViewModel
    {
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public int Count { get; set; }
    }

}
