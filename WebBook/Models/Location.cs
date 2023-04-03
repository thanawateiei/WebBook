using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebBook.Models
{
    public partial class Location
    {
        public int LocationId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกจุดรับหนังสือ")]
        [Display(Name = "จุดรับหนังสือ")]
        public string? LocationName { get; set; }
        [Display(Name = "รายละเอียดจุดรับหนังสือ")]
        public string? LocationDetail { get; set; }
    }
}
