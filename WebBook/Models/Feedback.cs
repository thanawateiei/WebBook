using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public string? UserId { get; set; }
        public string? FeedbackLike { get; set; }
        public int? FeedbackScore { get; set; }
        public string? FeedbackHeading { get; set; }
        public string? FeedbackDetail { get; set; }
    }
}
