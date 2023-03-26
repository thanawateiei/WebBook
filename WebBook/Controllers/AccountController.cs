using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBook.ViewModels;
using WebBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

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
                TempData["Message"] = "กรุณาเข้าสู่ระบบ";
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
                        TempData["Message"] = "ไม่สามารถยืมเนื่องจากคุณยืมหนังสือเกิน "+ userType.Limit + " เล่ม";
                        return RedirectToAction("BookReq", "Account");
                    }
                    else
                    {
                        Guid myuuid = Guid.NewGuid();
                        string myuuidAsString = "His-"+myuuid.ToString();
                        obj.HistoryId = myuuidAsString;
                        obj.UserId = HttpContext.Session.GetString("UserId");
                        obj.ReceiveDate = obj.ReceiveDate.Date;
                        if (obj.BookLang == "EN")
                        {
                            obj.ReturnDate = obj.ReceiveDate.AddDays((Double)userType.Enbook).Date;
                        }
                        else
                        {
                            obj.ReturnDate = obj.ReceiveDate.AddDays((Double)userType.Thbook).Date;
                        }
                        obj.CreatedAt = DateTime.Now;
                        obj.UpdatedAt = DateTime.Now;
                        obj.StatusId = 1;
                        _db.Histories.Add(obj);
                        _db.SaveChanges();
                        TempData["Message"] = "สำเร็จ";
                        return RedirectToAction("BookReq", "Account");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("BookReq", "Account");
            }
			TempData["Message"] = "การแก้ไขผิดพลาด";
            return View(obj);
        }
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                TempData["Message"] = "กรุณาเข้าสู่ระบบ";
                return RedirectToAction("Login");
            }
            var obj = _db.Users.Find(HttpContext.Session.GetString("UserId"));
            if (obj == null)
            {
                TempData["Message"] = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Profile", "Account");
            }
            ViewBag.UserId = obj.UserId;
            ViewBag.Agency = _db.Agencies.FirstOrDefault(ag => ag.AgencyId == obj.AgencyId).AgencyName;
            ViewBag.UserType = _db.UserTypes.FirstOrDefault(ut => ut.UserTypeId == obj.UserType).UserTypeName;
            return View(obj);
        }
        public IActionResult ProfileEdit(string id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                TempData["Message"] = "กรุณาเข้าสู่ระบบ";
                return RedirectToAction("Login");
            }
            if (id == null)
            {
                TempData["Message"] = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            var userinfo = _db.Users.Find(id);
            if (User == null)
            {
                TempData["Message"] = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }
            var key = "E546C8DF278CD5931069B522E695D4F2";
            TempData["Pass"] = DecryptString(userinfo.Password,key);
            ViewBag.Agency = _db.Agencies.FirstOrDefault(ag => ag.AgencyId == userinfo.AgencyId).AgencyName;
            ViewBag.UserType = _db.UserTypes.FirstOrDefault(ut => ut.UserTypeId == userinfo.UserType).UserTypeName;
            return View(userinfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProfileEdit(User obj)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                TempData["Message"] = "กรุณาเข้าสู่ระบบ";
                return RedirectToAction("Login");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var key = "E546C8DF278CD5931069B522E695D4F2";
                    obj.Password = EncryptString(obj.Password, key);
                    obj.UpdatedAt = DateTime.Now;
                    _db.Users.Update(obj);
                    _db.SaveChangesAsync();
                    return RedirectToAction("Profile","Account");
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
        public IActionResult History()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                TempData["Message"] = "กรุณาเข้าสู่ระบบ";
                return RedirectToAction("Login");
            }
            var htr = from h in _db.Histories
                     join ue in _db.Users on h.UserId equals ue.UserId into join_h_ue
                     from h_ue in join_h_ue.DefaultIfEmpty()
                     join s in _db.Statuses on h.StatusId equals s.StatusId into join_r_s
                     from h_s in join_r_s.DefaultIfEmpty()
                     join ln in _db.Locations on h.LocationId equals ln.LocationId into join_h_ln
                     from h_ln in join_h_ln.DefaultIfEmpty()
                     where h.UserId == HttpContext.Session.GetString("UserId")
                     orderby h.UpdatedAt descending
                     select new HistoryViewModel
                     {
                         HistoryId = h.HistoryId,
                         UserEmail = h_ue.Email,
                         BookTitle = h.BookName,
                         CallNumber = h.CallNumber,
                         Location = h_ln.LocationName,
                         //ReceiveDate = r.ReceiveDate,
                         ReceiveDate = Convert.ToDateTime(h.ReceiveDate).ToString("dd/MM/yyyy"),
                         ReturnDate = Convert.ToDateTime(h.ReturnDate).ToString("dd/MM/yyyy"),
                         Status = h_s.StatusName
                     };
            if (htr == null) return NotFound();
            return View(htr);
        }
		public IActionResult Feedback()
		{
			return PartialView();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Feedback(Feedback obj)
		{
			{
				try
				{
					if (ModelState.IsValid)
					{
                        var idFeedback = from f in _db.Feedbacks
                                         select f.FeedbackId;
                        obj.FeedbackId = (idFeedback.ToList().Count >= 1) ? idFeedback.Max() + 1 : 1;
                        obj.UserId = HttpContext.Session.GetString("UserId");
                        _db.Feedbacks.Add(obj);
						_db.SaveChanges();
                        return View();
					}
				}
				catch (Exception ex)
				{
					TempData["Message"] = ex.Message;
					return View(obj);
				}
				TempData["Message"] = "การบันทึกผิดพลาด";
				return View(obj);
			}
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
                //return PartialView();
                return View();
            }
            ///try?
            string decryptPassword = DecryptString(userinfo.Password,key);
            bool isValidPassword = String.Equals(decryptPassword,userPass);
            // var cus = _db.Customers.Find(userName);
            //ถ้าข้อมูลเท่ากับ 0 ให้บอกว่าหาข้อมูลไม่พบ
            if (!isValidPassword)
            {
                TempData["Message"] = "รหัสผ่านไม่ถูกต้อง";
                return View();
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
            return RedirectToAction("Index","Home");
        }
        public IActionResult Register()
        {
            ViewData["Agency"] = new SelectList(_db.Agencies, "AgencyId", "AgencyName");
            ViewData["UserType"] = new SelectList(_db.UserTypes, "UserTypeId", "UserTypeName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User obj)
        {
            ViewData["Agency"] = new SelectList(_db.Agencies, "AgencyId", "AgencyName");
            ViewData["UserType"] = new SelectList(_db.UserTypes, "UserTypeId", "UserTypeName");
            try
            {
                if (ModelState.IsValid)
                {
					Guid myuuid = Guid.NewGuid();
					string myuuidAsString = myuuid.ToString();
					var key = "E546C8DF278CD5931069B522E695D4F2";
					var userEmail = from u in _db.Users
									where u.Email.Equals(obj.Email)
									select u;
					if (userEmail.ToList().Count >= 1)
					{
						TempData["Message"] = "อีเมลถูกใช้แล้ว";
                        return View(obj);
                    }
                    obj.UserId = "User-" + myuuidAsString;
					obj.CreatedAt = DateTime.Now.Date;
                    obj.UpdatedAt = DateTime.Now.Date;
                    obj.Role = 3;
                    obj.Password = EncryptString(obj.Password,key);
                    _db.Users.Add(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View(obj);
            }
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
