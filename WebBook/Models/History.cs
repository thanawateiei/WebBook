using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebBook.Models
{
    public partial class History
    {
        public string? HistoryId { get; set; } = null!;
        public string? UserId { get; set; } = null!;
        public DateTime ReceiveDate { get; set; }
        [Required(ErrorMessage = "กรุณากรอกชื่อหนังสือ")]
        [Display(Name = "ชื่อหนังสือ")]
        public string? BookName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกชื่อผู้แต่ง")]
        [Display(Name = "ชื่อผู้แต่ง")]
        public string? AuthorName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกปีที่พิมพ์")]
        [Display(Name = "ปีที่พิมพ์")]
        public int? PublicationYear { get; set; }
        [Required(ErrorMessage = "กรุณากรอกสำนักพิมพ์")]
        [Display(Name = "สำนักพิมพ์")]
        public string? Publisher { get; set; }
        [Required(ErrorMessage = "กรุณากรอกเลขหมู่หนังสือ")]
        [Display(Name = "เลขหมู่หนังสือ")]
        public string? CallNumber { get; set; }
        public int? StatusId { get; set; }
        [Required(ErrorMessage = "กรุณาเลือกจุดรับหนังสือ")]
        [Display(Name = "จุดรับหนังสือ")]
        public int? LocationId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required(ErrorMessage = "กรุณาเลือกวันรับหนังสือ")]
        [DataType(DataType.Date)]
        [Display(Name = "วันรับหนังสือ")]
        public DateTime ReturnDate { get; set; }
        [Required(ErrorMessage = "กรุณาเลือกภาษาของหนังสือ")]
        [Display(Name = "ภาษาของหนังสือ")]
        public string? BookLang { get; set; }
    }
}
