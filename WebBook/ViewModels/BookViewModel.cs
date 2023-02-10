using System.ComponentModel.DataAnnotations;

namespace WebBook.ViewModels
{
    public class BookViewModel
    {
        [Key]
        public int Book_id { get; set; }
        public string? Book_Name { get; set; }
        public string? Author_Name { get; set; }
        public int? Publication_Year { get; set; }
        public string? Publisher { get; set; }
        public string? Book_Cover { get; set; }
        public string? Book_Detail { get; set; }
        public string? Book_Type_1 { get; set; }
        public string? Book_Type_2 { get; set; }
        public string? Book_Type_3 { get; set; }
        public string? Book_Type_4 { get; set; }
        public string? Book_Type_5 { get; set; }


    }


}
