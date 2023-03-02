using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class History
    {
        public string HistoryId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime? ReceiveDate { get; set; }
        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public int? PublicationYear { get; set; }
        public string? Publisher { get; set; }
        public string? CallNumber { get; set; }
        public int? StatusId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
