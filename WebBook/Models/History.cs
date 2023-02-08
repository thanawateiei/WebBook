using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public partial class History
    {
        [Key]
        public string HistoryId { get; set; } = null!;
        public string? UserId { get; set; }
        public string? ReceiveDate { get; set; }
        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public string? PublicationYear { get; set; }
        public string? Publisher { get; set; }
        public string? CallNumber { get; set; }
        public string? StatusId { get; set; }
    }
}
