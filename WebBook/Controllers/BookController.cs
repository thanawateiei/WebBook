using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using WebBook.Models;
using WebBook.ViewModels;

namespace WebBook.Controllers
{
    public class BookController : Controller
    {
        // GET: BookController
        private readonly webContext _db;
        public BookController(webContext db)
        { _db = db; }

     
        [Route("Admin/Book")]
        public IActionResult Index()
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
                         CreatedAt = b.CreatedAt//ต้องมีเพาะเอาไปเรียงข้อมูล
                     }).OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);

            List<BookViewModel> book = new List<BookViewModel>();
            book.AddRange(bb);
            //var ad = DateTime.Now;
            //ViewBag.CheckBT = cbt.ToList();
            //ViewData["CheckBT"] = new SelectList(_db.CheckBookTypes, "BookId", "BookTypeId", "CheckBt");
            //for (var i = 0; i <= book.Count(); i++)
            //{   //var path = "wwwroot\\img\\imgbook\\" + book.BookCover;
            //    try
            //    {
            //        var path = "wwwroot\\img\\imgbook\\" + book[i].BookCover;
            //        IEnumerable<string> items = Directory.EnumerateFileSystemEntries(path);
            //    }
            //    catch (Exception ex)
            //    {
            //        book[i].BookCover = "img-notFound.jpg";
            //    }
            //}
            //    //var imgg = Path.Combine(path, book.BookCover);
            //    //var exists = System.IO.File.Exists(imgg);
            //}
            if (book == null) return NotFound();
            return View(book);

        }

        [HttpPost]
        [Route("Admin/Book")]
        public IActionResult Index(string? stext)
        {
                      

            var bb = (from b in _db.Books
                    where b.BookName.Contains(stext) ||
                          b.AuthorName.Contains(stext)
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
            if (bb == null) return NotFound();
            return View(bb);
        }
        // GET: BookController/Details/5

        public IActionResult Detail(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            var obj = _db.Books.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMessage = "ไม่พบรายการนี้";
                return RedirectToAction("Index");

            }
            return PartialView(obj);
        }


        [Route("Admin/Book/Create")]
        public ActionResult Create()
        {
            //var idBook = from b in _db.Books
            //             select b.BookId;
            //var newidbook = idBook.Max();

            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();

            ViewBag.newidbook = "BOOK-"+ myuuidAsString;
         

            //ViewData["CheckBt"] = new SelectList(_db.CheckBookTypes, "BookId", "BookTypeId", "CheckBt");
            ViewData["BTT"] = new SelectList(_db.BookTypes, "BookTypeId", "BookTypeName");


            return View();
        }
        // POST: BookController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("Admin/Book/Create")]
        public ActionResult Create(BookViewModel obj)
        {
            try
            {
                //CheckBookType cbt = new CheckBookType();
                
                Book book = new Book();
                if (ModelState.IsValid)
                {
                   

                    book.BookId = obj.BookId;
                    book.BookName = obj.BookName;
                    book.AuthorName = obj.AuthorName;
                    book.PublicationYear = obj.PublicationYear;
                    book.Publisher = obj.Publisher;
                    book.BookDetail = obj.BookDetail;
                    book.CallNumber = obj.CallNumber;
                    book.BookType1 = obj.BookType1;
                    book.BookType2 = obj.BookType2;
                    book.BookType3 = obj.BookType3;
                    book.BookType4 = obj.BookType4;
                    book.BookType5 = obj.BookType5;
                    book.BookType6 = obj.BookType6;
                    book.BookType7 = obj.BookType7;
                    book.BookType8 = obj.BookType8;
                    book.BookType9 = obj.BookType9;
                    book.BookType10 = obj.BookType10;
                    book.CreatedAt = DateTime.Now;
                    book.UpdatedAt = DateTime.Now;

                    if (obj.Bookimg != null)
                    {
                        var LocalfileName = Path.GetFileName(obj.Bookimg.FileName);
                        var NewFileName = obj.BookId;
                        var FileExtension = Path.GetExtension(LocalfileName);
                        var UpFileName =  NewFileName + FileExtension;
                        var savedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgbook");
                        var Filepath = Path.Combine(savedir, UpFileName);

                        using (FileStream fs = System.IO.File.Create(Filepath))
                        {
                            obj.Bookimg.CopyTo(fs);
                            fs.Flush();
                        }
                        book.BookCover = UpFileName;

                    }
                    _db.Books.Add(book);
                    _db.SaveChanges();

                    return RedirectToAction("index");
                } 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            ViewBag.ErrorMessage = "การบันทึกผิดพลาด";

            return RedirectToAction("index");
        }

        // GET: BookController/Edit/5
        [Route("Admin/Book/Edit")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            var obj = _db.Books.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMessage = "ไม่พบรายการนี้";
                return RedirectToAction("Index");

            }

            BookViewModel book = new BookViewModel();
            book.BookId = obj.BookId;
            book.BookName = obj.BookName;
            book.AuthorName = obj.AuthorName;
            book.PublicationYear = obj.PublicationYear;
            book.Publisher = obj.Publisher;
            book.BookCover = obj.BookCover;
            book.BookDetail = obj.BookDetail;
            book.CallNumber = obj.CallNumber;
            book.BookType1 = obj.BookType1;
            book.BookType2 = obj.BookType2;
            book.BookType3 = obj.BookType3;
            book.BookType4 = obj.BookType4;
            book.BookType5 = obj.BookType5;
            book.BookType6 = obj.BookType6;
            book.BookType7 = obj.BookType7;
            book.BookType8 = obj.BookType8;
            book.BookType9 = obj.BookType9;
            book.BookType10 = obj.BookType10;
            book.CreatedAt = obj.CreatedAt;





            ViewBag.BLang = obj.BookLang;
            ViewData["BTT"] = new SelectList(_db.BookTypes, "BookTypeId", "BookTypeName");
            return View(book);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [Route("Admin/Book/Edit")]
        public ActionResult Edit(BookViewModel obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                   
                    Book book = new Book();
                    book.BookId = obj.BookId;
                    book.BookName = obj.BookName;
                    book.AuthorName = obj.AuthorName;
                    book.PublicationYear = obj.PublicationYear;
                    book.Publisher = obj.Publisher;
                    book.CallNumber = obj.CallNumber;
                    book.BookDetail = obj.BookDetail;
                    book.BookType1 = obj.BookType1;
                    book.BookType2 = obj.BookType2;
                    book.BookType3 = obj.BookType3;
                    book.BookType4 = obj.BookType4;
                    book.BookType5 = obj.BookType5;
                    book.BookType6 = obj.BookType6;
                    book.BookType7 = obj.BookType7;
                    book.BookType8 = obj.BookType8;
                    book.BookType9 = obj.BookType9;
                    book.BookType10 = obj.BookType10;
                    book.CreatedAt = obj.CreatedAt;
                    book.UpdatedAt = DateTime.Now;


                    if (obj.Bookimg != null)
                    {
                        //delete
                        var fileName = obj.BookCover;
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgbook");
                        var filePath = Path.Combine(imagePath, fileName);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        //Save
                        var LocalfileName = Path.GetFileName(obj.Bookimg.FileName);
                        var NewFileName = obj.BookId;
                        var FileExtension = Path.GetExtension(LocalfileName);
                        var UpFileName = "Book-" + NewFileName + FileExtension;
                        var savedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgbook");
                        var Filepath = Path.Combine(savedir, UpFileName);
                        using (FileStream fs = System.IO.File.Create(Filepath))
                        {
                            obj.Bookimg.CopyTo(fs);
                            fs.Flush();
                        }
                        book.BookCover = UpFileName;

					}
					else
					{
                        book.BookCover = obj.BookCover;
                    }

                    _db.Books.Update(book);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = "การบันทึกผิดพลาด";
            return RedirectToAction("Index");
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(string id)
        {
            //ตรวจสอบว่ามีการส่ง id มาหรือไม่
            if (id == null)
            {
                ViewBag.ErrorMassage = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Books.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }

            //ViewBag.btName = _db.BookTypes.FirstOrDefault(bt => bt.BookTypeId == obj.BookType1).BookTypeId;
            return View(obj);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(string BookId)
        {
            try
            {
                // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
                var obj = _db.Books.Find(BookId);
                if (obj == null)
                {
                    ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                    return RedirectToAction("Index");
                }

                var fileName = obj.BookCover;
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgbook");
                var filePath = Path.Combine(imagePath, fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _db.Books.Remove(obj); //ส่งคำสั่ง Remove ผ่าน DBContext
                _db.SaveChanges(); // Execute คำสั่ง
                return RedirectToAction("Index"); // ย้ายทำงาน Action Index              
            }
            catch (Exception ex)
            {
                //ถ้าไม่ Valid ก็ สร้าง Error Message ขึ้นมา แล้ว ส่ง Obj กลับไปที่ View
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
        }


    }
}


