using System.ComponentModel.DataAnnotations;
namespace WebBook.ViewModels
{
    public class RequestViewModel
    {
            [Key]
            public string RequestId { get; set; }
            public string? UserEmail { get; set; }
            public string? BookTitle { get; set; }
            public DateTime? ReceiveDate { get; set; }
            public string? CallNumber { get; set; }
            public string? Status { get; set; }
            public int? StatusID { get; set; }


    }

}

