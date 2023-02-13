using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public partial class UserType
    {
        [Key]
        [Display(Name = "IDประเภทผู้ใช้")]
        public int UserTypeId { get; set; }

        [Range(1, 1000, ErrorMessage = "กรุณาเลือกจำนวนการยืม")]
        [Display(Name = "จำนวนการยืม")]
        public int? Limit { get; set; }

        [Required(ErrorMessage = "ต้องระบุชื่อประเภทผู้ใช้")]
        [Display(Name = "ชื่อประเภทผู้ใช้")]
        public string? UserTypeName { get; set; } = null!;
    }
}
