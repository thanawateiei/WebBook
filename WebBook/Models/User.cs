using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebBook.Models
{
    public partial class User
    {
        public string? UserId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกชื่อจริง")]
        [DataType(DataType.Text)]
        [Display(Name = "ชื่อจริง")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "กรุณากรอกนามสกุล")]
        [DataType(DataType.Text)]
        [Display(Name = "นามสกุล")]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "กรุณากรอกอีเมล")]
        [DataType(DataType.EmailAddress,ErrorMessage = "กรุณากรอกอีเมลให้ถูกต้อง")]
        [Display(Name = "อีเมล")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+(@ku\.th|@live\.ku\.th)$", ErrorMessage = "ใช้อีเมล @ku.th หรือ @live.ku.th")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสผ่าน")]
        public string? Password { get; set; }
		[Display(Name = "หน้าที่")]
		public int? Role { get; set; }
        [Required(ErrorMessage = "กรุณาเลือกหน่วยงาน")]
        [Display(Name = "หน่วยงาน")]
        public int? AgencyId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกรหัสประจำตัว")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "กรุณากรอกรหัสประจำตัวให้ถูกต้อง")]
        [Display(Name = "รหัสประจำตัว")]
        public string? StudentId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "กรุณากรอกเป็นตัวเลข")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์ให้ถูกต้อง")]
        [Display(Name = "เบอร์โทรศัพท์")]
        public string? Telephone { get; set; }
        [Required(ErrorMessage = "กรุณาเลือกประเภทผู้ใช้บริการ")]
        [Display(Name = "ประเภทผู้ใช้บริการ")]
        public int? UserType { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
