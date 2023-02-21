using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebBook.Models
{
    public partial class Location
    {
        [Key]
        [Display(Name = "ID Location")]
        public int LocationId { get; set; }
        [Required]
        [Display(Name = "ชื่อจุดรับหนังสือ")]
        public string? LocationName { get; set; }
        [Display(Name = "รายละเอียดจุดรับหนังสือ")]
        public string? LocationDetail { get; set; }
    }
}
