using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebBook.Models
{
    public partial class Status
    {
        [Key]
        [Display(Name = "ID Status")]
        public int StatusId { get; set; }
        [Required]
        [Display(Name = "ชื่อสถานะ")]
        public string? StatusName { get; set; }
    }
}
