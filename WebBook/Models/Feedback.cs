using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class Feedback
    {
        public string? FeedbackId { get; set; }
        public string? FeedbackLike { get; set; }
        public int? FeedbackScore { get; set; }
        public string? FeedbackDetail { get; set; }
    }
}
