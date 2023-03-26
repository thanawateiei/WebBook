using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class Issue
    {
        public int IssueId { get; set; }
        public string? UserId { get; set; }
        public string? IssueTitle { get; set; }
        public string? IssueDetail { get; set; }
        public string? IssueStatus { get; set; }
    }
}
