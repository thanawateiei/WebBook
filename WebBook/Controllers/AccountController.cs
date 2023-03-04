using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBook.ViewModels;
using WebBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly webContext _db;
        public AccountController(webContext db)
        { _db = db; }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Account/BookReq")]
        public IActionResult BookReq()
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            ViewBag.HistoryId = "His-" + myuuidAsString;
            ViewData["Location"] = new SelectList(_db.Locations, "LocationId", "LocationName");
            return View();
        }
        [Route("Account/BookReq/{id}")]
        public IActionResult BookReq(string id)
        {
            //ตรวจสอบว่ามีการส่ง id มาหรือไม่
            if (id == null)
            {
                ViewBag.ErrorMassage = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
            var bookInfo = _db.Books.Find(id);
            if (bookInfo == null)
            {
                ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            ViewBag.HistoryId = "His-" + myuuidAsString;
            History his = new History();
            his.BookName = bookInfo.BookName;
            his.AuthorName = bookInfo.AuthorName;
            his.PublicationYear = bookInfo.PublicationYear;
            his.Publisher = bookInfo.Publisher;
            his.CallNumber = bookInfo.CallNumber;
            ViewData["Location"] = new SelectList(_db.Locations, "LocationId", "LocationName");
            return View(his);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookReqCreate(History obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var limit = from lm in _db.Histories
                                       where lm.UserId.Equals(obj.UserId) && lm.StatusId.Equals(4)
                                       select lm;
                    if (limit.ToList().Count >= 10 )
                    {
                        ViewBag.ErrorMessage = "ไม่สามารถยืมได้เกิน Limit";
                        return RedirectToAction("BookReq", "Account");
                    }
                    else
                    {
                        obj.CreatedAt = DateTime.Now;
                        obj.UpdatedAt = DateTime.Now;
                        obj.StatusId = 1;
                        _db.Histories.Add(obj);
                        _db.SaveChanges();
                        return RedirectToAction("BookReq", "Account");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            ViewBag.ErrorMessage = "การแก้ไขผิดพลาด";
            return View(obj);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string userEmail, string userPass)
        {
            //Query หาว่ามี Login Password ที่ระบุหรือไม่
            var user = from u in _db.Users
                       where u.Email.Equals(userEmail)
                       select u;
            var userinfo = _db.Users.SingleOrDefault(u => u.Email == userEmail);
            if (userinfo == null)
            {
                //ถ้าใช้ RedirectToAction ไม่สามารถใช้ ViewBag ได้ ต้องใช้ TempData
                TempData["ErrorMessage"] = "ไม่พบผู้ใช้";
                //ViewBag.ErrorMessage = "ระบุผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
                return RedirectToAction("Login");
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(userPass, userinfo.Password);
            // var cus = _db.Customers.Find(userName);
            //ถ้าข้อมูลเท่ากับ 0 ให้บอกว่าหาข้อมูลไม่พบ
            if (user.ToList().Count == 0)
            {
                //ถ้าใช้ RedirectToAction ไม่สามารถใช้ ViewBag ได้ ต้องใช้ TempData
                TempData["ErrorMessage"] = "ไม่พบผู้ใช้";
                //ViewBag.ErrorMessage = "ระบุผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
                return RedirectToAction("Login");
            }
            if (!isValidPassword)
            {
                TempData["ErrorMessage"] = "รหัสผ่านผิด";
                return RedirectToAction("Login");
            }
            //ถ้าหาข้อมูลพบ ให้เก็บค่าเข้า Session 
            string UserId;
            string UserEmail;
            int UserRole;
            int UserType;
            foreach (var item in user)
            {
                //อ่านค่าจาก Object เข้าตัวแปร
                UserId = item.UserId;
                UserEmail = item.Email;
                UserRole = (int)item.Role;
                UserType = (int)item.UserType;

                //เอาค่าจากตัวแปรเข้า Session
                HttpContext.Session.SetString("UserId", UserId);
                HttpContext.Session.SetString("UserEmail", UserEmail);
                HttpContext.Session.SetInt32("UserRole", UserRole);
                HttpContext.Session.SetInt32("UserType", UserType);
                //Update  Column ของตารางที่ระบุ
                //โดยทำการอ่านค่าตาม รหัส หรือ id ที่ระบุ
                //และระบุค่าแต่ละ Field ของ Record นั้นๆ
                //แล้วกำหนดให้ทำการปรับเปลี่ยน (Modified)
                var theRecord = _db.Users.Find(UserId);
                //theRecord.LastLogin = DateTime.Now.Date;
                //theRecord.CusAdd = "ABCD";
                //theRecord.CusEmail = "a@b.com";
                //_db.Entry(theRecord).State = EntityState.Modified;
            }
            //ทำการบันทึกทุก Record ที่สั่ง Modified ไว้
            _db.SaveChanges();
            //ทำการย้ายไปหน้าที่ต้องการ
            return RedirectToAction("Index","Home");
        }
        public IActionResult Register()
        {
            var idUser = from u in _db.Users
                             select u.UserId;
            var maxUserId = idUser.Max();
            ViewBag.UserId = maxUserId + 1;
            return View();
        }
        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
        //ค่าที่ส่งมาจาก Form เป็น Object ของ Model ที่ระบุ ตัว Controller ก็รับค่าเป็น Object
        public IActionResult Register(User obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obj.Role = 3;
                    obj.Password = BCrypt.Net.BCrypt.HashPassword(obj.Password);
                    _db.Users.Add(obj); //ส่งคำสั่ง Add ผ่าน DBContext
                    _db.SaveChanges(); // Execute คำสั่ง
                    return RedirectToAction("Login"); // ย้ายทำงาน Action Index
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
        public IActionResult Logout()
        {
            //ล้างทุก Session และย้ายกลับหน้า Index
            HttpContext.Session.Clear();
            return RedirectToAction("Home","Index");
        }
    }
}
