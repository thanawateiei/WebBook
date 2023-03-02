using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class Location
    {
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? LocationDetail { get; set; }
    }
}
