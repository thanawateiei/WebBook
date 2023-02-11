using Microsoft.AspNetCore.Mvc;
using WebBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebBook.Controllers
{
    public class RoleController : Controller
    {
        private readonly webContext _db;
        public RoleController(webContext db)
        { _db = db; }
        [Route("admin/role")]
        public IActionResult Index()
        {
            var roleList = from r in _db.Roles
                             select r;
            if (roleList == null) return NotFound();
            return View(roleList);
        }
        [Route("admin/role/create")]
        public IActionResult Create()
        {
            var idRole = from r in _db.Roles
                           select r.RoleId;
            var maxRoleId = idRole.Max();
            ViewBag.RoleId = maxRoleId + 1;
            return View();
        }
        [Route("admin/role/create")]
        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
        //ค่าที่ส่งมาจาก Form เป็น Object ของ Model ที่ระบุ ตัว Controller ก็รับค่าเป็น Object
        public IActionResult Create(Role obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Roles.Add(obj); //ส่งคำสั่ง Add ผ่าน DBContext
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
            ViewBag.ErrorMessage = "การบันทึกผิดพลาด";
            return View(obj);
        }

        [Route("admin/role/edit/{id}")]
        public IActionResult Edit(int id)
        {
            //ตรวจสอบว่ามีการส่ง id มาหรือไม่
            if (id == null)
            {
                ViewBag.ErrorMassage = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Roles.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
        //ค่าที่ส่งมาจาก Form เป็น Object ของ Model ที่ระบุ ตัว Controller ก็รับค่าเป็น Object
        [Route("admin/role/edit/{id}")]
        public IActionResult Edit(Role obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Roles.Update(obj); //ส่งคำสั่ง Update ผ่าน DBContext
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
            return View(obj);

        }

        [Route("admin/role/delete/{id}")]
        public IActionResult Delete(int id)
        {
            //ตรวจสอบว่ามีการส่ง id มาหรือไม่
            if (id == null)
            {
                ViewBag.ErrorMassage = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Roles.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [Route("admin/role/delete/{id}")]
        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
        //**** ค่าที่ส่งมาจาก Form เป็น string  ต้องรับค่าเป็น string
        // แต่ถ้ารับค่าเป็น string จะ Error เพราะเป็นการประกาศ method ซ้ำจึงต้องตั้งชื่อใหม่ เป็น DeletePost
        // และตัวแปรที่รับ จะต้องเหมือนกับชือที่ส่งมาจาก View ด้วย จากตัวอย่างคือ PdId
        public IActionResult DeletePost(int RoleId)
        {
            try
            {
                // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
                var obj = _db.Roles.Find(RoleId);
                if (obj == null)
                {
                    ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                    return RedirectToAction("Index");
                }
                _db.Roles.Remove(obj); //ส่งคำสั่ง Remove ผ่าน DBContext
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