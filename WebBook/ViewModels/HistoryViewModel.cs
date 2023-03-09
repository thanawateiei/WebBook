using System.ComponentModel.DataAnnotations;
namespace WebBook.ViewModels
{
    public class HistoryViewModel
    {
            [Key]
            public string HistoryId { get; set; }
            public string? UserEmail { get; set; }
            public string? BookTitle { get; set; }
            public string? CallNumber { get; set; }
            public string? Location { get; set; }
            public string? ReceiveDate { get; set; }
            public string? ReturnDate { get; set; }
            public string? Status { get; set; }


    }

}

