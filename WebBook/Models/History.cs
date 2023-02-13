using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public partial class History
    {
        [Key]
        public int HistoryId { get; set; }
        public int UserId { get; set; }
        public string? ReceiveDate { get; set; }
        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public int? PublicationYear { get; set; }
        public string? Publisher { get; set; }
        public string? CallNumber { get; set; }
        public int StatusId { get; set; }
    }
}
