using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public partial class User
    {

        [Key]
        [Display(Name = "IDผู้ใช้")]
        public int UserId { get; set; }

        [Display(Name = "อีเมล")]
        public string? Email { get; set; }

        [Display(Name = "รหัสผ่าน")]
        public string? Password { get; set; }

        [Display(Name = "สถานะผู้ใช้")]
        [Range(1, 100, ErrorMessage = "กรุณาเลือกสถานะของผู้ใช้")]
        public int? Role { get; set; }

        [Display(Name = "หน่วยงานผู้ใช้")]
        [Range(1, 100, ErrorMessage = "กรุณาเลือกหน่วยงานของผู้ใช้")]
        public int? AgencyId { get; set; }

        [Display(Name = "รหัสนิสิต")]
        public string? StudentId { get; set; }

        [Display(Name = "ชื่อผู้ใช้")]
        public string? Name { get; set; }

        [Display(Name = "เบอร์โทร")]
        public string? Telephone { get; set; }

        [Display(Name = "ประเภทผู้ใช้")]
        [Range(1, 100, ErrorMessage = "กรุณาเลือกประเภทของผู้ใช้")]
        public int? UserType { get; set; }
    }
}
