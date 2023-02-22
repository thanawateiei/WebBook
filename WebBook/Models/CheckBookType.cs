using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class CheckBookType
    {
        public int CbtId { get; set; }
        public int? BookId { get; set; }
        public int? BookTypeId { get; set; }
        public bool? CheckBt { get; set; }
    }
}
