using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBook.Models;

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
            var pd = from b in _db.Books
                     join bt in _db.BookTypes on b.BookType1 equals bt.BookTypeId into join_b_bt1
                     from b_bt1 in join_b_bt1.DefaultIfEmpty()

                     join bt in _db.BookTypes on b.BookType2 equals bt.BookTypeId into join_b_bt2
                     from b_bt2 in join_b_bt2.DefaultIfEmpty()

                     join bt in _db.BookTypes on b.BookType3 equals bt.BookTypeId into join_b_bt3
                     from b_bt3 in join_b_bt3.DefaultIfEmpty()

                     join bt in _db.BookTypes on b.BookType4 equals bt.BookTypeId into join_b_bt4
                     from b_bt4 in join_b_bt4.DefaultIfEmpty()

                     join bt in _db.BookTypes on b.BookType5 equals bt.BookTypeId into join_b_bt5
                     from b_bt5 in join_b_bt5.DefaultIfEmpty()

                     select new ViewModels.BookViewModel
                     {
                         Book_id = b.BookId,
                         Book_Name = b.BookName,
                         Author_Name = b.AuthorName,
                         Publication_Year = b.PublicationYear,
                         Publisher = b.Publisher,
                         Book_Cover = b.BookCover,
                         Book_Detail = b.BookDetail,
                         Book_Type_1 = b_bt1.BookTypeName,
                         Book_Type_2 = b_bt2.BookTypeName,
                         Book_Type_3 = b_bt3.BookTypeName,
                         Book_Type_4 = b_bt4.BookTypeName,
                         Book_Type_5 = b_bt5.BookTypeName
                     };
            if (pd == null) return NotFound();
            return View(pd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Book")]
        public IActionResult Index(string? stext)
        {
            var pd = from b in _db.Books
                     join bt in _db.BookTypes on b.BookType1 equals bt.BookTypeId into join_b_bt1
                     from b_bt1 in join_b_bt1.DefaultIfEmpty()

                     join bt in _db.BookTypes on b.BookType2 equals bt.BookTypeId into join_b_bt2
                     from b_bt2 in join_b_bt2.DefaultIfEmpty()

                     join bt in _db.BookTypes on b.BookType3 equals bt.BookTypeId into join_b_bt3
                     from b_bt3 in join_b_bt3.DefaultIfEmpty()

                     join bt in _db.BookTypes on b.BookType4 equals bt.BookTypeId into join_b_bt4
                     from b_bt4 in join_b_bt4.DefaultIfEmpty()

                     join bt in _db.BookTypes on b.BookType5 equals bt.BookTypeId into join_b_bt5
                     from b_bt5 in join_b_bt5.DefaultIfEmpty()
                     where b.BookName.Contains(stext) ||
                           b.AuthorName.Contains(stext) 

                     select new ViewModels.BookViewModel
                     {
                         Book_id = b.BookId,
                         Book_Name = b.BookName,
                         Author_Name = b.AuthorName,
                         Publication_Year = b.PublicationYear,
                         Publisher = b.Publisher,
                         Book_Cover = b.BookCover,
                         Book_Detail = b.BookDetail,
                         Book_Type_1 = b_bt1.BookTypeName,
                         Book_Type_2 = b_bt2.BookTypeName,
                         Book_Type_3 = b_bt3.BookTypeName,
                         Book_Type_4 = b_bt4.BookTypeName,
                         Book_Type_5 = b_bt5.BookTypeName
                     };
            if (pd == null) return NotFound();
            ViewBag.stext = stext;
            return View(pd);
        }
        // GET: BookController/Details/5
       

        // GET: BookController/Create
        //[Route("Admin/Book/Create")]
        //public ActionResult Create()
        //{
        //    var bt     = from b in _db.BookTypes
        //                 select b;
        //    var idBook = from b in _db.Books
        //                 select b.BookId;
        //    var newidbook = idBook.Max();
        //    List<BookType> Btview = new List<BookType>();
        //    Btview.AddRange(bt);
        //    ViewBag.newidbook = newidbook+1;
        //    ViewBag.booktype = Btview;
        //    return View();
        //}

        public ActionResult Create()
        {
            var idBook = from b in _db.Books
                         select b.BookId;
            var newidbook = idBook.Max();
            ViewBag.newidbook = newidbook + 1;

            ViewData["Bt"] = new SelectList(_db.BookTypes, "BookTypeId", "BookTypeName");
            return  View();
        }
        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Route("Admin/Book/Create/{obj?}")]
        public ActionResult Create(Book obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _db.Books.Add(obj); 
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            ViewBag.ErrorMessage = "การบันทึกผิดพลาด";
            return View(obj);
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
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
            ViewData["Bt"] = new SelectList(_db.BookTypes, "BookTypeId", "BookTypeName");
            return View(obj);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _db.Books.Update(obj);
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
