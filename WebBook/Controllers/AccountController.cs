using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBook.ViewModels;
using WebBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;

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
            if (HttpContext.Session.GetString("UserId") == null)
            {
				TempData["Message"] = "กรุณาเข้าสู่ระบบ";
				return RedirectToAction("Login");
            }
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            ViewBag.HistoryId = "His-" + myuuidAsString;
            ViewData["Location"] = new SelectList(_db.Locations, "LocationId", "LocationName");
            return View();
        }
        [Route("Account/BookReq/{id}")]
        public IActionResult BookReq(string id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                TempData["Message"] = "กรุณาเข้าสู่ระบบ";
				return RedirectToAction("Login");
            }
            //ตรวจสอบว่ามีการส่ง id มาหรือไม่
            if (id == null)
            {
                TempData["Message"] = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
            var bookInfo = _db.Books.Find(id);
            if (bookInfo == null)
            {
				TempData["Message"] = "ไม่พบข้อมูลที่ระบุ";
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
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var userType = _db.UserTypes.Find(HttpContext.Session.GetInt32("UserType"));
                    var limit = from lm in _db.Histories
                                       where lm.UserId.Equals(obj.UserId) && lm.StatusId != 6
                                       select lm;
                    if (limit.ToList().Count >= userType.Limit)
                    {
                        TempData["Message"] = "ไม่สามารถยืมได้เกิน Limit";
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
                TempData["Message"] = ex.Message;
                return View(obj);
            }
			TempData["Message"] = "การแก้ไขผิดพลาด";
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
            var key = "E546C8DF278CD5931069B522E695D4F2";
            //Query หาว่ามี Login Password ที่ระบุหรือไม่
            var user = from u in _db.Users
                       where u.Email.Equals(userEmail)
                       select u;
            var userinfo = _db.Users.SingleOrDefault(u => u.Email == userEmail);
            if (userinfo == null)
            {
                //ถ้าใช้ RedirectToAction ไม่สามารถใช้ ViewBag ได้ ต้องใช้ TempData
                TempData["Message"] = "ไม่พบผู้ใช้";
                //ViewBag.ErrorMessage = "ระบุผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
                return RedirectToAction("Login");
            }
            string decryptPassword = DecryptString(userinfo.Password,key);
            bool isValidPassword = String.Equals(decryptPassword,userPass);
            // var cus = _db.Customers.Find(userName);
            //ถ้าข้อมูลเท่ากับ 0 ให้บอกว่าหาข้อมูลไม่พบ
            if (!isValidPassword)
            {
                TempData["Message"] = "รหัสผ่านไม่ถูกต้อง";
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
                //theRecord.CreatedAt = DateTime.Now.Date;
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
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            ViewBag.UserId = "User-" + myuuidAsString;
            ViewData["Agency"] = new SelectList(_db.Agencies, "AgencyId", "AgencyName");
            ViewData["UserType"] = new SelectList(_db.UserTypes, "UserTypeId", "UserTypeName");
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
                    var key = "E546C8DF278CD5931069B522E695D4F2";
                    obj.CreatedAt = DateTime.Now.Date;
                    obj.UpdatedAt = DateTime.Now.Date;
                    obj.Role = 3;
                    obj.Password = EncryptString(obj.Password,key);
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
            TempData["Message"] = "การบันทึกผิดพลาด";
            return View(obj);
        }
        public static string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
        public IActionResult Logout()
        {
            //ล้างทุก Session และย้ายกลับหน้า Index
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}
