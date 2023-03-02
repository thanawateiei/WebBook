using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class User
    {
        public string UserId { get; set; } = null!;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? Role { get; set; }
        public int? AgencyId { get; set; }
        public string? StudentId { get; set; }
        public string? Name { get; set; }
        public string? Telephone { get; set; }
        public int? UserType { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
