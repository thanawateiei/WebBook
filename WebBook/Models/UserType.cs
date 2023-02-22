using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class UserType
    {
        public int UserTypeId { get; set; }
        public int? Limit { get; set; }
        public string? UserTypeName { get; set; }
    }
}
