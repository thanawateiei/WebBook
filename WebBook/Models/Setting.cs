using System;
using System.Collections.Generic;

namespace WebBook.Models
{
    public partial class Setting
    {
        public int SettingId { get; set; }
        public string? BannerImg { get; set; }
        public string? BannerStatus { get; set; }
        public string? Detail { get; set; }
        public string? DetailStatus { get; set; }
    }
}
