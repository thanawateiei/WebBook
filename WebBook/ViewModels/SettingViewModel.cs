using System.ComponentModel.DataAnnotations;
using WebBook.Models;

namespace WebBook.ViewModels
{
    public class SettingViewModel
    {
 
            public int SettingId { get; set; }
            public string? BannerImg { get; set; }
            public string? BannerStatus { get; set; }
            public string? Detail { get; set; }
            public string? DetailStatus { get; set; }
            public IFormFile? Settingimg { get; set; }
        


    }

}
