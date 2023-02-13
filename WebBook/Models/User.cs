using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebBook.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? Role { get; set; }
        public int? AgencyId { get; set; }
        public string? StudentId { get; set; }
        public string? Name { get; set; }
        public string? Telephone { get; set; }
        public int? UserType { get; set; }
    }
}
