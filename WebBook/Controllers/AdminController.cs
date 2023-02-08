using Microsoft.AspNetCore.Mvc;
using WebBook.Models;
using WebBook.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebBook.Controllers
{
    public class AdminController : Controller
    {
        private readonly webContext _db;
        public AdminController(webContext db)
        { _db = db; }
        public IActionResult Index()
        {
            var rq = from r in _db.Histories
                     join ue in _db.Users on r.UserId equals ue.UserId into join_r_ue
                     from r_ue in join_r_ue.DefaultIfEmpty()
                     join s in _db.Statuses on r.StatusId equals s.StatusId into join_r_s
                     from r_s in join_r_s.DefaultIfEmpty()
                     select new Detail
                     {
                         RequestId = r.HistoryId,
                         UserEmail = r_ue.Email,
                         BookTitle = r.BookName,
                         ReceiveDate = r.ReceiveDate,
                         CallNumber = r.CallNumber,
                         Status = r_s.StatusName
                     };
            if (rq == null) return NotFound();
            return View(rq);
        }
        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
        public IActionResult Index(string? stext)
        {
            if (stext == null)
            {
                return RedirectToAction("Index");
            }
            //var pd = from p in _db.Products
            //         select p;
            var rq = from r in _db.Histories
                     join ue in _db.Users on r.UserId equals ue.UserId into join_r_ue
                     from r_ue in join_r_ue.DefaultIfEmpty()
                     join s in _db.Statuses on r.StatusId equals s.StatusId into join_r_s
                     from r_s in join_r_s.DefaultIfEmpty()
                     where r_ue.Email.Contains(stext) ||
                            r.BookName.Contains(stext) ||
                            r.CallNumber.Contains(stext)
                     select new Detail
                     {
                         RequestId = r.HistoryId,
                         UserEmail = r_ue.Email,
                         BookTitle = r.BookName,
                         ReceiveDate = r.ReceiveDate,
                         CallNumber = r.CallNumber,
                         Status = r_s.StatusName
                     };
            if (rq == null) return NotFound();

            ViewBag.stext = stext;
            return View(rq);
        }
        public IActionResult Edit(string id)
        {
            //ตรวจสอบว่ามีการส่ง id มาหรือไม่
            if (id == null)
            {
                ViewBag.ErrorMassage = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Histories.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }
            //อ่านข้อมูลจากตารางลง SelectList แล้วใส่ข้อมูลลงตัว ViewData
            //และกำนหนดว่า Select ที่เลือก เป็น id ของ obj นั้นๆ
            ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName", obj.StatusId);
            return View(obj);
        }

        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
        //ค่าที่ส่งมาจาก Form เป็น Object ของ Model ที่ระบุ ตัว Controller ก็รับค่าเป็น Object
        public IActionResult Edit(History obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Histories.Update(obj); //ส่งคำสั่ง Update ผ่าน DBContext
                    _db.SaveChanges(); // Execute คำสั่ง
                    return RedirectToAction("Index"); // ย้ายทำงาน Action Index
                }
            }
            catch (Exception ex)
            {
                //ถ้าไม่ Valid ก็ สร้าง Error Message ขึ้นมา แล้ว ส่ง Obj กลับไปที่ View
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            //ถ้าไม่ Valid ก็ สร้าง Error Message ขึ้นมา แล้ว ส่ง Obj กลับไปที่ View
            ViewBag.ErrorMessage = "การแก้ไขผิดพลาด";
            //อ่านข้อมูลจากตารางลง SelectList แล้วใส่ข้อมูลลงตัว ViewData
            //และกำนหนดว่า Select ที่เลือก เป็น id ของ obj นั้นๆ
            ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName", obj.StatusId);
            return View(obj);

        }
    }
}
