using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public partial class Book
    {
        [Key]
        [Display(Name = "ID Book")]
        public int BookId { get; set; } 

        [Required(ErrorMessage = "ต้องระบุชื่อหนังสือ")]
        [Display(Name = "ชื่อหนังสือ")]
        public string? BookName { get; set; } = null!;

        [Display(Name = "ชื่อผู้เขียน")]
        public string? AuthorName { get; set; }

        [Display(Name = "ป๊ที่ตีพิม")]
        public int? PublicationYear { get; set; }

        [Display(Name = "ชื่อสำนักพิม")]
        public string? Publisher { get; set; }

        public string? BookCover { get; set; }

        [Display(Name = "รายละเอียด")]
        public string? BookDetail { get; set; }

        [Display(Name = "ประเภทที่ 1")]
        public int? BookType1 { get; set; }

        [Display(Name = "ประเภทที่ 2")]
        public int? BookType2 { get; set; }

        [Display(Name = "ประเภทที่ 3")]
        public int? BookType3 { get; set; }

        [Display(Name = "ประเภทที่ 4")]
        public int? BookType4 { get; set; }

        [Display(Name = "ประเภทที่ 5")]
        public int? BookType5 { get; set; }
    }
}
