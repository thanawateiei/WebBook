using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public int? Password { get; set; }
        public string? Role { get; set; }
        public int? AgencyId { get; set; }
        public string? StudentId { get; set; }
        public string? Name { get; set; }
        public int? Telephone { get; set; }
        public int? UserType { get; set; }
    }
}
