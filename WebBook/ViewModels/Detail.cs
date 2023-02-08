using System.ComponentModel.DataAnnotations;
namespace WebBook.ViewModels
{
    public class Detail
    {
            [Key]
            public string RequestId { get; set; } = null!;
            public string? UserEmail { get; set; }
            public string? BookTitle { get; set; }
            public string? ReceiveDate { get; set; }
            public string? CallNumber { get; set; }
            public string? Status { get; set; }


    }

}

