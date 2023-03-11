using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBook.Models;

namespace WebBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly webContext _db;
        public HomeController(webContext db)
        { _db = db; }

        public IActionResult Index()
        {
            var booknew = (from b in _db.Books
                           where b.BookType1 == 13 || b.BookType2 == 13 || b.BookType3 == 13 || b.BookType4 == 13 || b.BookType5 == 13 ||
                                 b.BookType6 == 13 || b.BookType7 == 13 || b.BookType8 == 13 || b.BookType9 == 13 || b.BookType10 == 13 
                           select new ViewModels.BookViewModel
                          {
                              BookId = b.BookId,
                              BookName = b.BookName,
                              AuthorName = b.AuthorName,
                              PublicationYear = b.PublicationYear,
                              Publisher = b.Publisher,
                              BookCover = b.BookCover,
                              BookDetail = b.BookDetail,
                              BookLang = b.BookLang,
                              CreatedAt = b.CreatedAt//ต้องมีเพาะเอาไปเรียงข้อมูล
                          }).OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);

            var bookRecommend = (from b in _db.Books
                           where b.BookType1 == 14 || b.BookType2 == 14 || b.BookType3 == 14 || b.BookType4 == 14 || b.BookType5 == 14 ||
                                 b.BookType6 == 14 || b.BookType7 == 14 || b.BookType8 == 14 || b.BookType9 == 14 || b.BookType10 == 14
                           select new ViewModels.BookViewModel
                           {
                               BookId = b.BookId,
                               BookName = b.BookName,
                               AuthorName = b.AuthorName,
                               PublicationYear = b.PublicationYear,
                               Publisher = b.Publisher,
                               BookCover = b.BookCover,
                               BookDetail = b.BookDetail,
                               BookLang = b.BookLang,
                               CreatedAt = b.CreatedAt//ต้องมีเพาะเอาไปเรียงข้อมูล
                           }).OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);

            ViewBag.BookNew = booknew;
            ViewBag.BookRecommend = bookRecommend;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}