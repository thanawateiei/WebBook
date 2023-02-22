using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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

           
            var bb = from b in _db.Books
                     select new ViewModels.BookViewModel
                     {
                         BookId = b.BookId,
                         BookName = b.BookName,
                         AuthorName = b.AuthorName,
                         PublicationYear = b.PublicationYear,
                         Publisher = b.Publisher,
                         BookCover = b.BookCover,
                         BookDetail = b.BookDetail,
                     };
            var cbt = from cbtt in _db.CheckBookTypes
                      join bt in _db.BookTypes on cbtt.BookTypeId equals bt.BookTypeId into join_cbtt_bt
                      from cbtt_bt in join_cbtt_bt.DefaultIfEmpty()
                      select new ViewModels.CBTViewModel
                      {
                          BookId = cbtt.BookId,
                          BookTypeName = cbtt_bt.BookTypeName,
                          CheckBt = cbtt.CheckBt
                      };

            ViewBag.CheckBT = cbt.ToList();
            //ViewData["CheckBT"] = new SelectList(_db.CheckBookTypes, "BookId", "BookTypeId", "CheckBt");
            if (bb == null) return NotFound();
            return View(bb);

        }

        [HttpPost]
        [Route("Admin/Book")]
        public IActionResult Index(string? stext)
        {
                      

            var bb = from b in _db.Books
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
                     };
            var cbt = from cbtt in _db.CheckBookTypes
                      join bt in _db.BookTypes on cbtt.BookTypeId equals bt.BookTypeId into join_cbtt_bt
                      from cbtt_bt in join_cbtt_bt.DefaultIfEmpty()
                      select new ViewModels.CBTViewModel
                      {
                          BookId = cbtt.BookId,
                          BookTypeName = cbtt_bt.BookTypeName,
                          CheckBt = cbtt.CheckBt
                      };


            ViewBag.CheckBT = cbt.ToList();
            //ViewData["CheckBT"] = new SelectList(_db.CheckBookTypes, "BookId", "BookTypeId", "CheckBt");
            if (bb == null) return NotFound();
            return View(bb);
        }
        // GET: BookController/Details/5


        public IActionResult Createupload(int id)
        {
            
            // ทำการเขียน Query หา Record ของ Customer.CusId จาก id ที่ส่งมา
            if (id == 0)
            {
                ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }
            var cbt = from c in _db.CheckBookTypes
                      where c.BookId == id
                      select new ViewModels.BackCBTViewModel
                      {
                          CbtId = c.CbtId,
                          BookId = c.BookId,
                          BookTypeId = c.BookTypeId,
                          CheckBt = c.CheckBt
                      };
            var BT = new SelectList(_db.BookTypes, "BookTypeId", "BookTypeName");
            ViewBag.BTT = JsonConvert.SerializeObject(BT);
            ViewBag.CBT = JsonConvert.SerializeObject(cbt);
            //ViewBag.Book = JsonConvert.SerializeObject(obj);
            return View();

        }

        [HttpPost] //ระบุว่าเป็นการทำงานแบบ POST
        //[ValidateAntiForgeryToken] //ป้องกันการโจมตี Cross-Site Request Forgery
        public IActionResult ImgUpload(IFormFile imgfiles, string theid)
        {
            if (imgfiles == null)
            {
                ViewBag.ErrorMessage = "ID Not Found";
                return RedirectToAction("Show");
            }
            //Getting FileName
            var LocalfileName = Path.GetFileName(imgfiles.FileName);

            var NewFileName = theid;

            //Getting file Extension
            var FileExtension = Path.GetExtension(LocalfileName);

            //ต่อ FileName กับ FileExtension
            var UpFileName = "Book-"+NewFileName + FileExtension;

            //กำหนดตำแหน่งที่ต้องการเก็บ File
            var savedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgbook");
            //ต่อ ต่ำแหน่งที่ต้องการเก็บ File และ ชื่อFile
            var Filepath = Path.Combine(savedir, UpFileName);
            //ทำการ Upload
            using (FileStream fs = System.IO.File.Create(Filepath))
            {
                imgfiles.CopyTo(fs);
                fs.Flush();
            }

            //ย้ายหน้า controller = "Home", action = "Index", id = ""
            return RedirectToAction("Create", new { id = theid });
        }


        public IActionResult ImgDelete(string id)
        {
            var fileName = id.ToString() + ".jpg";
            //กำหนดตำแหน่งที่ตั้งของ File
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgcus");
            //เชื่อต่อ ตำแหน่ง กับ ชื่อFile
            var filePath = Path.Combine(imagePath, fileName);
            //ทำการตรวจสอบว่ามี File อยู่หรือไม่
            if (System.IO.File.Exists(filePath))
            {
                //ถ้ามีให้ลบ
                System.IO.File.Delete(filePath);
            }
            //controller = "Home", action = "Index", id = ""
            return RedirectToAction("Show", new { id = id });
        }


        [Route("Admin/Book/Create")]
        public ActionResult Create()
        {
            var idBook = from b in _db.Books
                         select b.BookId;
            var newidbook = idBook.Max();
            ViewBag.newidbook = newidbook + 1;

            var idcbt = from b in _db.CheckBookTypes
                         select b.CbtId;
            var newidcbt = idcbt.Max();
            ViewBag.newidcbt = newidcbt + 1;

            ViewData["CheckBt"] = new SelectList(_db.CheckBookTypes, "BookId", "BookTypeId", "CheckBt");
            var BT = new SelectList(_db.BookTypes, "BookTypeId", "BookTypeName");
            ViewBag.BTT = JsonConvert.SerializeObject(BT);
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
					foreach (var b in obj.CheckBT)
					{
                        CheckBookType cbt = new CheckBookType();
                        cbt.CbtId = b.CbtId;
						cbt.BookId = b.BookId;
						cbt.BookTypeId = b.BookTypeId;
						cbt.CheckBt = b.CheckBt;
						//_db.CheckBookTypes.Add(cbt);
					}
					book.BookId = obj.BookId;
                    book.BookName = obj.BookName;
                    book.AuthorName = obj.AuthorName;
                    book.PublicationYear = obj.PublicationYear;
                    book.Publisher = obj.Publisher;
                    book.BookCover = obj.BookCover;
                    book.BookDetail = obj.BookDetail;
                    //_db.Books.Add(book);
                    //_db.SaveChanges();
                    TempData["obj"] = new BookViewModel();
                    return RedirectToAction("Createupload", "obj");
                } 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            ViewBag.ErrorMessage = "การบันทึกผิดพลาด";

            return RedirectToAction("Createupload", "obj");
        }

        // GET: BookController/Edit/5
        [Route("Admin/Book/Edit")]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");

            }
            var obj = _db.Books.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMessage = "ไม่พบรายการนี้";
                return RedirectToAction("Index");

            }
            //ViewBag.book = _db.Books.Find(id);
            var cbt = from c in _db.CheckBookTypes
                      where c.BookId == id
                      select new ViewModels.BackCBTViewModel
                      {
                          CbtId = c.CbtId,
                          BookId = c.BookId,
                          BookTypeId = c.BookTypeId,
                          CheckBt = c.CheckBt
                      };
            var BT = new SelectList(_db.BookTypes, "BookTypeId", "BookTypeName");
            ViewBag.BTT = JsonConvert.SerializeObject(BT);
            ViewBag.CBT = JsonConvert.SerializeObject(cbt);
            ViewBag.Book = JsonConvert.SerializeObject(obj);
            return RedirectToAction();
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
                    foreach (var b in obj.CheckBT)
                    {
                        CheckBookType cbt = new CheckBookType();
                        cbt.CbtId = b.CbtId;
                        cbt.BookId = b.BookId;
                        cbt.BookTypeId = b.BookTypeId;
                        cbt.CheckBt = b.CheckBt;
                        _db.CheckBookTypes.Update(cbt);
                        _db.SaveChanges();
                    }
                    book.BookId = obj.BookId;
                    book.BookName = obj.BookName;
                    book.AuthorName = obj.AuthorName;
                    book.PublicationYear = obj.PublicationYear;
                    book.Publisher = obj.Publisher;
                    book.BookCover = obj.BookCover;
                    book.BookDetail = obj.BookDetail;
                    _db.Books.Update(book);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            ViewBag.ErrorMessage = "การบันทึกผิดพลาด";
            return View(obj);
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            //ตรวจสอบว่ามีการส่ง id มาหรือไม่
            if (id == 0)
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
        public ActionResult DeletePost(int BookId)
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
                var cbtt = from c in _db.CheckBookTypes
                          where c.BookId == BookId
                          select c;
                foreach (var b in cbtt)
                {
                    var objj = _db.CheckBookTypes.Find(b.CbtId);
                    _db.CheckBookTypes.Remove(objj);
                   
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


