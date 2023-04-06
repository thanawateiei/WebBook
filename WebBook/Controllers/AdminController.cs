using Microsoft.AspNetCore.Mvc;
using WebBook.Models;
using WebBook.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace WebBook.Controllers
{
    public class AdminController : Controller
    {
        private readonly webContext _db;
        public AdminController(webContext db)
        { _db = db; }

   
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login");
            }
            else if (HttpContext.Session.GetInt32("UserRole") != 1)
            {
                TempData["Message"] = "คุณไม่มีสิทธิ์เข้าถึงหน้านี้";
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            var his = from h in _db.Histories
                      select h;

            List<History> dataHis = new List<History>();
            dataHis.AddRange(his);

            //set data status
            List<DataPoint> datastatus = new List<DataPoint>();
            var Status = from s in _db.Statuses
                      select s;
            foreach(var item in Status)
            {
                datastatus.Add(new DataPoint(item.StatusName, dataHis.Where(x => x.StatusId.Equals(item.StatusId)).Count() ));
            }
            //set data UserAgency
            List<User> dataUser= new List<User>();
            List<DataPoint> dataUserAgency = new List<DataPoint>();
            List<DataPoint> dataUserType = new List<DataPoint>();
            var User = from u in _db.Users
                         select u;
            dataUser.AddRange(User);
            var Agency = from a in _db.Agencies
                       select a;
            var UserType = from ut in _db.UserTypes
                         select ut;

            foreach (var item in Agency)
            {
                dataUserAgency.Add(new DataPoint(item.AgencyName, dataUser.Where(x => x.AgencyId.Equals(item.AgencyId)).Count()));
            }
            foreach (var item in UserType)
            {
                dataUserType.Add(new DataPoint(item.UserTypeName, dataUser.Where(x => x.UserType.Equals(item.UserTypeId)).Count()));
            }


            var user = from h in _db.Users
                       where h.Role == 3
                       select h;
            ViewBag.AllUser = user.Count();
            ViewBag.dataUserAgency = JsonConvert.SerializeObject(dataUserAgency);
            ViewBag.dataUserType = JsonConvert.SerializeObject(dataUserType);
            ViewBag.datastatus = datastatus;
            ViewBag.HisCount = dataHis.Count();
            return View();
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
                return PartialView();
            }
            ///try?
            string decryptPassword = DecryptString(userinfo.Password, key);
            bool isValidPassword = String.Equals(decryptPassword, userPass);
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
            return RedirectToAction("Index");
        }
        public IActionResult Logout()
        {
            //ล้างทุก Session และย้ายกลับหน้า Index
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
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
