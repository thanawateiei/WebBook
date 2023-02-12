using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBook.Models;

namespace WebBook.Controllers
{

    public class UserTypeController : Controller
    {
        private readonly webContext _db;
        public UserTypeController(webContext db)
        { _db = db; }
        // GET: UserController
        public ActionResult Index()
        {
            var btt = from bt in _db.UserTypes
                      select new ViewModels.UserTypeViewModel
                      {
                          UserTypeId = bt.UserTypeId,
                          Limit = bt.Limit,
                          UserTypeName = bt.UserTypeName
                          
                      };


            if (btt == null) return NotFound();
            return View(btt);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string? stext)
        {

            var btt = from bt in _db.UserTypes
                      where bt.UserTypeName.Contains(stext)
                      select new ViewModels.UserTypeViewModel
                      {
                          UserTypeId = bt.UserTypeId,
                          Limit = bt.Limit,
                          UserTypeName = bt.UserTypeName

                      };

            if (btt == null) return NotFound();
            return View(btt);
      
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            var idUserType = from b in _db.UserTypes
                         select b.UserTypeId;
            var newidUserType = idUserType.Max();
            ViewBag.newidUserType = newidUserType + 1;

            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( UserType obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _db.UserTypes.Add(obj);
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

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");

            }
            var obj = _db.UserTypes.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMessage = "ไม่พบรายการนี้";
                return RedirectToAction("Index");

            }
            return View(obj);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserType obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _db.UserTypes.Update(obj);
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                ViewBag.ErrorMassage = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.UserTypes.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }

            //ViewBag.btName = _db.BookTypes.FirstOrDefault(bt => bt.BookTypeId == obj.BookType1).BookTypeId;
            return View(obj);
        }
        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int UserTypeId)
        {
            try
            {
                // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
                var obj = _db.UserTypes.Find(UserTypeId);
                if (obj == null)
                {
                    ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                    return RedirectToAction("Index");
                }
                _db.UserTypes.Remove(obj); //ส่งคำสั่ง Remove ผ่าน DBContext
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
