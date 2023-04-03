using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebBook.Models
{
    public partial class BookType
    {
        public int BookTypeId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกชื่อประเภทหนังสือ")]
        [Display(Name = "ชื่อประเภทหนังสือ")]
        public string? BookTypeName { get; set; }
    }
}
