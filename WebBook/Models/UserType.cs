using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebBook.Models
{
    public partial class UserType
    {
        public int UserTypeId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกจำนวน")]
        [Display(Name = "จำนวน (เล่ม)")]
        public int? Limit { get; set; }
        [Required(ErrorMessage = "กรุณากรอกประเภทผู้ใช้")]
        [Display(Name = "ประเภทผู้ใช้")]
        public string? UserTypeName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกกำหนดเวลายืม หนังสือภาษาอังกฤษ")]
        [Display(Name = "กำหนดเวลายืม หนังสือภาษาอังกฤษ")]
        public int? Enbook { get; set; }
        [Required(ErrorMessage = "กรุณากรอกกำหนดเวลายืม หนังสือภาษาไทย")]
        [Display(Name = "กำหนดเวลายืม หนังสือภาษาไทย")]
        public int? Thbook { get; set; }
    }
}
