using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class UserViewModel
    {
        [Key]
        public string user_id { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? roleName { get; set; }
        public string? agencyName { get; set; }
        public string? student_id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? telephone { get; set; }
        public string? user_typeName { get; set; }



    }


}
