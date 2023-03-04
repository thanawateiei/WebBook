using Microsoft.AspNetCore.Mvc;
using WebBook.Models;
using WebBook.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebBook.Controllers
{
    public class RequestController : Controller
    {
        private readonly webContext _db;
        public RequestController(webContext db)
        { _db = db; }
        [Route("admin/request")]
        public IActionResult Index()
        {
            var rq = from r in _db.Histories
                     join ue in _db.Users on r.UserId equals ue.UserId into join_r_ue
                     from r_ue in join_r_ue.DefaultIfEmpty()
                     join s in _db.Statuses on r.StatusId equals s.StatusId into join_r_s
                     from r_s in join_r_s.DefaultIfEmpty()
                     select new RequestViewModel
                     {
                         RequestId = r.HistoryId,
                         UserEmail = r_ue.Email,
                         BookTitle = r.BookName,
                         ReceiveDate = r.ReceiveDate.ToString(),
                         CallNumber = r.CallNumber,
                         Status = r_s.StatusName,
                         StatusID = r.StatusId
                     };
            if (rq == null) return NotFound();
            return View(rq);
        }
        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
        [Route("admin/request")]
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
                            r_s.StatusName.Contains(stext) ||
                            r.CallNumber.Contains(stext)
                     select new RequestViewModel
                     {
                         RequestId = r.HistoryId,
                         UserEmail = r_ue.Email,
                         BookTitle = r.BookName,
                         //ReceiveDate = r.ReceiveDate,
                         CallNumber = r.CallNumber,
                         Status = r_s.StatusName
                     };
            if (rq == null) return NotFound();

            ViewBag.stext = stext;
            return View(rq);
        }
        [Route("admin/request/edit/{id}")]
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
            
           
            ViewBag.UserEmail = _db.Users.FirstOrDefault(ue => ue.UserId == obj.UserId).Email;
            ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName", obj.StatusId);
            return View(obj);
        }

        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
        //ค่าที่ส่งมาจาก Form เป็น Object ของ Model ที่ระบุ ตัว Controller ก็รับค่าเป็น Object
        [Route("admin/request/edit/{id}")]
        public IActionResult Edit(History obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obj.UpdatedAt = DateTime.Now;
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
        [Route("admin/request/delete")]
        public IActionResult Delete(string id)
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

            ViewBag.UserEmail = _db.Users.FirstOrDefault(ue => ue.UserId == obj.UserId).Email;
            ViewBag.StatusName = _db.Statuses.FirstOrDefault(sn => sn.StatusId == obj.StatusId).StatusName;
            return View(obj);
        }

        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
        //**** ค่าที่ส่งมาจาก Form เป็น string  ต้องรับค่าเป็น string
        // แต่ถ้ารับค่าเป็น string จะ Error เพราะเป็นการประกาศ method ซ้ำจึงต้องตั้งชื่อใหม่ เป็น DeletePost
        // และตัวแปรที่รับ จะต้องเหมือนกับชือที่ส่งมาจาก View ด้วย จากตัวอย่างคือ PdId
        public IActionResult DeletePost(string RequestId)
        {
            try
            {
                // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
                var obj = _db.Histories.Find(RequestId);
                if (obj == null)
                {
                    ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                    return RedirectToAction("Index");
                }
                _db.Histories.Remove(obj); //ส่งคำสั่ง Remove ผ่าน DBContext
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