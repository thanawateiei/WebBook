using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class AgencyViewModel
    {
        [Key]
        public int AgencyId { get; set; }
        public string? AgencyName { get; set; }

    }


}
