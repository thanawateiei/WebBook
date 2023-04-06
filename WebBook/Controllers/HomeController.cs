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
            var book = (from b in _db.Books
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
                            BookType1 = b.BookType1,
                            BookType2 = b.BookType2,
                            BookType3 = b.BookType3,
                            BookType4 = b.BookType4,
                            BookType5 = b.BookType5,
                            BookType6 = b.BookType6,
                            BookType7 = b.BookType7,
                            BookType8 = b.BookType8,
                            BookType9 = b.BookType9,
                            BookType10 = b.BookType10,
                            CreatedAt = b.CreatedAt,//ต้องมีเพาะเอาไปเรียงข้อมูล
                            UpdatedAt = b.UpdatedAt
                        }).OrderByDescending(c => c.UpdatedAt.Date).ThenBy(c => c.UpdatedAt.TimeOfDay);
            //book new
            List<BookViewModel> BNew = new List<BookViewModel>();
            BNew.AddRange(book);
            BNew = BNew.Where(b => ((b.BookType1.Equals(1)) || (b.BookType2.Equals(1)) || (b.BookType3.Equals(1)) || (b.BookType4.Equals(1)) ||
                                 (b.BookType5.Equals(1)) || (b.BookType6.Equals(1)) || (b.BookType7.Equals(1)) || (b.BookType8.Equals(1)) ||
                                 (b.BookType9.Equals(1)) || (b.BookType10.Equals(1)))).ToList();
            for (var i = 0; i < BNew.Count; i++)
            {
                var p = _db.PopularBooks.Where(a => a.BookId == BNew[i].BookId).Sum(b => b.PopularCount);
                BNew[i].popview = p;
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
             
            //book Recommend 
            List<BookViewModel> BRecommend = new List<BookViewModel>();
            BRecommend.AddRange(book);
            BRecommend = BRecommend.Where(b => ((b.BookType1.Equals(2)) || (b.BookType2.Equals(2)) || (b.BookType3.Equals(2)) || (b.BookType4.Equals(2)) ||
                                (b.BookType5.Equals(2)) || (b.BookType6.Equals(2)) || (b.BookType7.Equals(2)) || (b.BookType8.Equals(2)) ||
                                (b.BookType9.Equals(2)) || (b.BookType10.Equals(2)))).ToList();
            for (var i = 0; i < BRecommend.Count; i++)
            {
                var p = _db.PopularBooks.Where(a => a.BookId == BRecommend[i].BookId).Sum(b => b.PopularCount);
                BRecommend[i].popview = p;
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

            var setImg = (from s in _db.Settings
                         where s.BannerStatus == "on" && s.BannerImg!=null
                         select new Setting
                         {
                             SettingId = s.SettingId,
                             BannerImg = s.BannerImg,
                             BannerStatus = s.BannerStatus
                         }).OrderByDescending(c => c.SettingId).ThenBy(c => c.SettingId);


            var setalert = from s in _db.Settings
                            where s.DetailStatus == "on" && s.Detail != null
                            select new Setting
                            {
                                SettingId = s.SettingId,
                                Detail = s.Detail,
                                DetailStatus = s.DetailStatus
                            };
            List<Setting> setting = new List<Setting>();
            setting.AddRange(setalert);

            if (setalert.Count() != 0)
			{
                ViewBag.setalert = setalert;
			}
			else
			{
                ViewBag.setalert = null;
			}
            
            ViewBag.setImg = setImg;



            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search(string stext, int id)
        {
            var bb = (from b in _db.Books
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
                          BookType1 = b.BookType1,
                          BookType2 = b.BookType2,
                          BookType3 = b.BookType3,
                          BookType4 = b.BookType4,
                          BookType5 = b.BookType5,
                          BookType6 = b.BookType6,
                          BookType7 = b.BookType7,
                          BookType8 = b.BookType8,
                          BookType9 = b.BookType9,
                          BookType10 = b.BookType10,
                          CreatedAt = b.CreatedAt,//ต้องมีเพาะเอาไปเรียงข้อมูล
                          UpdatedAt = b.UpdatedAt
                      }).OrderByDescending(c => c.UpdatedAt).ThenBy(c => c.UpdatedAt.TimeOfDay);

            var book = new List<BookViewModel>();
            book.AddRange(bb);
            for (var i = 0; i < book.Count; i++)
            {
                var p = _db.PopularBooks.Where(a => a.BookId == book[i].BookId).Sum(b => b.PopularCount);
                book[i].popview = p;
                try
                {
                    var path = "wwwroot\\img\\imgbook\\" + book[i].BookCover;
                    IEnumerable<string> items = Directory.EnumerateFileSystemEntries(path);
                }
                catch (Exception ex)
                {
                    book[i].BookCover = "img-notFound.jpg";
                }
            }
            var search = "";
            if (stext != null)
            {
                book = book.Where(b => (b.BookName.Contains(stext)) || (b.AuthorName.Contains(stext)) || (b.Publisher.Contains(stext))).ToList();
                search = stext;
            }
            if (id != 0)
            {
                book = book.Where(b => ((b.BookType1.Equals(id)) || (b.BookType2.Equals(id)) || (b.BookType3.Equals(id)) || (b.BookType4.Equals(id)) ||
                                 (b.BookType5.Equals(id)) || (b.BookType6.Equals(id)) || (b.BookType7.Equals(id)) || (b.BookType8.Equals(id)) ||
                                 (b.BookType9.Equals(id)) || (b.BookType10.Equals(id)))).ToList();
                search = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == id).BookTypeName;
            }
            ViewBag.search = search;
            return View(book);
        }
        public IActionResult BookDetail(string id)
        {
            var pop = _db.PopularBooks.AsNoTracking().FirstOrDefault(ue => ue.BookId == id && ue.PopularDate == DateTime.Now.Date);
            if (pop == null)
            {
                Guid myuuid = Guid.NewGuid();
                string myuuidAsString = "POP-" + myuuid.ToString();
                PopularBook PopularBook = new PopularBook();
                PopularBook.PopularId = myuuidAsString;
                PopularBook.BookId = id;
                PopularBook.PopularDate = DateTime.Now.Date;
                PopularBook.PopularCount = 1;
                _db.PopularBooks.Add(PopularBook);
                _db.SaveChanges();
            }
            else
            {
                pop.PopularCount++;
                _db.PopularBooks.Update(pop);
                _db.SaveChanges();
            }
            var obj = _db.Books.Find(id);
            BookViewModel book = new BookViewModel();
            List<BookTypeViewModel> booktype = new List<BookTypeViewModel>();
            if (true)
            {
                if (obj.BookType1 != 0)
                {
                    BookTypeViewModel bookt = new BookTypeViewModel();
                    bookt.BookTypeId = obj.BookType1;
                    bookt.BookTypeName = _db.BookTypes.FirstOrDefault(ue => ue.BookTypeId == obj.BookType1).BookTypeName;
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
            }


            try
            {
                var path = "wwwroot\\img\\imgbook\\" + book.BookCover;
                IEnumerable<string> items = Directory.EnumerateFileSystemEntries(path);
            }
            catch (Exception ex)
            {
                book.BookCover = "img-notFound.jpg";
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