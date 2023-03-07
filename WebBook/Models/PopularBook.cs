using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class PopularBook
    {
        public string PopularId { get; set; } = null!;
        public string? BookId { get; set; }
        public DateTime? PopularDate { get; set; }
        public int? PopularCount { get; set; }
    }
}
