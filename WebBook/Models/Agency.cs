using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebBook.Models
{
    public partial class Agency
    {
        public int AgencyId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกชื่อหน่วยงาน")]
        [Display(Name = "ชื่อหน่วยงาน")]
        public string? AgencyName { get; set; }
    }
}
