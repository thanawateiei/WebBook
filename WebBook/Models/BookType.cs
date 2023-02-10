using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public partial class BookType
    {
        [Key]
        [Display(Name = "ID BookType")]
        public int BookTypeId { get; set; }

        [Display(Name = "ประเภทหนังสือ")]
        public string? BookTypeName { get; set; } = null!;
    }
}
