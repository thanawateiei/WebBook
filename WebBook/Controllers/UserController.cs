using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;
using WebBook.Models;

namespace WebBook.Controllers
{

    public class UserController : Controller
    {
        private readonly webContext _db;
        public UserController(webContext db)
        { _db = db; }
        // GET: UserController
        public ActionResult Index()
        {
            var key = "E546C8DF278CD5931069B522E695D4F2";
            var uu = from u in _db.Users
                    
                    join ur in _db.Roles on u.Role equals ur.RoleId into join_u_ur
                    from uu_ur in join_u_ur

                    join ua in _db.Agencies on u.AgencyId equals ua.AgencyId into join_u_ua
                    from uu_ua in join_u_ua

                    join ut in _db.UserTypes on u.UserType equals ut.UserTypeId into join_u_ut
                    from uu_ut in join_u_ut

                     select new ViewModels.UserViewModel
                     {
                         user_id = u.UserId,
                         email = u.Email,
                         password = DecryptString(u.Password, key),
                         roleName = uu_ur.RoleName,
                         student_id = u.StudentId,
                         agencyName = uu_ua.AgencyName,
                         Firstname = u.Firstname,
                         Lastname = u.Lastname,
                         telephone = u.Telephone,
                         user_typeName = uu_ut.UserTypeName
                     };
            //List <ViewModels.UserViewModel> Uview = new List<ViewModels.UserViewModel>();
            //List<User> Uview = new List<User>();
            //Uview.AddRange(uu);
            if (uu == null) return NotFound();
            return View(uu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string? stext)
        {
            var key = "E546C8DF278CD5931069B522E695D4F2";
            var uu = from u in _db.Users

                     join ur in _db.Roles on u.Role equals ur.RoleId into join_u_ur
                     from uu_ur in join_u_ur

                     join ua in _db.Agencies on u.AgencyId equals ua.AgencyId into join_u_ua
                     from uu_ua in join_u_ua

                     join ut in _db.UserTypes on u.UserType equals ut.UserTypeId into join_u_ut
                     from uu_ut in join_u_ut

                     where u.Firstname.Contains(stext) ||
                           u.Lastname.Contains(stext) ||
                           u.StudentId.Contains(stext) ||
                           u.Email.Contains(stext)

                     select new ViewModels.UserViewModel
                     {
                         user_id = u.UserId,
                         email = u.Email,
                         password = DecryptString(u.Password,key),
                         roleName = uu_ur.RoleName,
                         student_id = u.StudentId,
                         agencyName = uu_ua.AgencyName,
                         Firstname = u.Firstname,
                         Lastname = u.Lastname,
                         telephone = u.Telephone,
                         user_typeName = uu_ut.UserTypeName
                     };
            //List <ViewModels.UserViewModel> Uview = new List<ViewModels.UserViewModel>();
            //List<User> Uview = new List<User>();
            //Uview.AddRange(uu);
            if (uu == null) return NotFound();
            return View(uu);
        }

        // GET: UserController/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            ViewBag.newidUser = "User-"+myuuidAsString;

            ViewData["Ur"] = new SelectList(_db.Roles, "RoleId", "RoleName");
            ViewData["Ua"] = new SelectList(_db.Agencies, "AgencyId", "AgencyName");
            ViewData["Ut"] = new SelectList(_db.UserTypes, "UserTypeId", "UserTypeName");
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    obj.UpdatedAt = DateTime.Now;
                    obj.CreatedAt = DateTime.Now;
                    _db.Users.Add(obj);
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
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            var obj = _db.Users.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMessage = "ไม่พบรายการนี้";
                return RedirectToAction("Index");

            }
            var key = "E546C8DF278CD5931069B522E695D4F2";
            TempData["Pass"] = DecryptString(obj.Password, key);
            ViewData["Ur"] = new SelectList(_db.Roles, "RoleId", "RoleName");
            ViewData["Ua"] = new SelectList(_db.Agencies, "AgencyId", "AgencyName");
            ViewData["Ut"] = new SelectList(_db.UserTypes, "UserTypeId", "UserTypeName");
            return View(obj);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var key = "E546C8DF278CD5931069B522E695D4F2";
                    obj.CreatedAt = _db.Users.AsNoTracking().FirstOrDefault(ue => ue.UserId == obj.UserId).CreatedAt;
                    obj.Password = EncryptString(obj.Password, key);
                    obj.UpdatedAt = DateTime.Now;
                    _db.Users.Update(obj);
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
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                ViewBag.ErrorMassage = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Users.Find(id);
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
        public ActionResult DeletePost(string UserId)
        {
            try
            {
                // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
                var obj = _db.Users.Find(UserId);
                if (obj == null)
                {
                    ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                    return RedirectToAction("Index");
                }
                _db.Users.Remove(obj); //ส่งคำสั่ง Remove ผ่าน DBContext
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
    }
}
