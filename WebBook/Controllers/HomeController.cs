using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBook.Models;
using WebBook.Models;
using WebBook.ViewModels;
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
                           select new BookViewModel
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

                            List<BookViewModel> BNew = new List<BookViewModel>();
                            BNew.AddRange(booknew);
                            for (var i = 0; i < BNew.Count; i++)
                            {
                                try
                                {
                                    var path = "wwwroot\\img\\imgbook\\" + BNew[i].BookCover;
                                    IEnumerable<string> items = Directory.EnumerateFileSystemEntries(path);
                                }
                                catch (Exception ex)
                                {
                                     BNew[i].BookCover = "img-notFound.jpg";
                                }
                            }
                           var bookRecommend = (from b in _db.Books
                           where b.BookType1 == 14 || b.BookType2 == 14 || b.BookType3 == 14 || b.BookType4 == 14 || b.BookType5 == 14 ||
                                 b.BookType6 == 14 || b.BookType7 == 14 || b.BookType8 == 14 || b.BookType9 == 14 || b.BookType10 == 14
                           select new BookViewModel
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
                        List<BookViewModel> BRecommend = new List<BookViewModel>();
                         BRecommend.AddRange(bookRecommend);
                        for (var i = 0; i < BRecommend.Count; i++)
                        {
                            try
                            {
                                var path = "wwwroot\\img\\imgbook\\" + BRecommend[i].BookCover;
                                IEnumerable<string> items = Directory.EnumerateFileSystemEntries(path);
                            }
                            catch (Exception ex)
                            {
                            BRecommend[i].BookCover = "img-notFound.jpg";
                            }
                        }
            ViewBag.BookNew = BNew;
            ViewBag.BookRecommend = BRecommend;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search(string stext)
        {
            
            return View();
        }
        public IActionResult BookDetail(string id)
        {
            var obj = _db.Books.Find(id);
            BookViewModel book = new BookViewModel();
            List<BookTypeViewModel> booktype = new List<BookTypeViewModel>();
            if (obj.BookType1 != 0) {
                BookTypeViewModel bookt = new BookTypeViewModel();
                bookt.BookTypeId = obj.BookType1;
                bookt.BookTypeName  = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType1).BookTypeName;
                booktype.Add(bookt);
            }
            if (obj.BookType2 != 0)
            {
                BookTypeViewModel bookt = new BookTypeViewModel();
                bookt.BookTypeId = obj.BookType2;
                bookt.BookTypeName = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType2).BookTypeName;
                booktype.Add(bookt);
            }
            if (obj.BookType3 != 0)
            {
                BookTypeViewModel bookt = new BookTypeViewModel();
                bookt.BookTypeId = obj.BookType3;
                bookt.BookTypeName = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType3).BookTypeName;
                booktype.Add(bookt);
            }
            if (obj.BookType4 != 0)
            {
                BookTypeViewModel bookt = new BookTypeViewModel();
                bookt.BookTypeId = obj.BookType4;
                bookt.BookTypeName = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType4).BookTypeName;
                booktype.Add(bookt);
            }
            if (obj.BookType5 != 0)
            {
                BookTypeViewModel bookt = new BookTypeViewModel();
                bookt.BookTypeId = obj.BookType5;
                bookt.BookTypeName = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType5).BookTypeName;
                booktype.Add(bookt);
            }
            if (obj.BookType6 != 0)
            {
                BookTypeViewModel bookt = new BookTypeViewModel();
                bookt.BookTypeId = obj.BookType6;
                bookt.BookTypeName = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType6).BookTypeName;
                booktype.Add(bookt);
            }
            if (obj.BookType7 != 0)
            {
                BookTypeViewModel bookt = new BookTypeViewModel();
                bookt.BookTypeId = obj.BookType7;
                bookt.BookTypeName = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType7).BookTypeName;
                booktype.Add(bookt);
            }
            if (obj.BookType8 != 0)
            {
                BookTypeViewModel bookt = new BookTypeViewModel();
                bookt.BookTypeId = obj.BookType8;
                bookt.BookTypeName = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType8).BookTypeName;
                booktype.Add(bookt);
            }
            if (obj.BookType9 != 0)
            {
                BookTypeViewModel bookt = new BookTypeViewModel();
                bookt.BookTypeId = obj.BookType9;
                bookt.BookTypeName = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType9).BookTypeName;
                booktype.Add(bookt);
            }
            if (obj.BookType10 != 0)
            {
                BookTypeViewModel bookt = new BookTypeViewModel();
                bookt.BookTypeId = obj.BookType10;
                bookt.BookTypeName = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType10).BookTypeName;
                booktype.Add(bookt);
            }


            ViewBag.booktype = booktype;
            ViewBag.Book = obj;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}