using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                         password = u.Password,
                         roleName = uu_ur.RoleName,
                         student_id = u.StudentId,
                         agencyName = uu_ua.AgencyName,
                         name = u.Name,
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
            var uu = from u in _db.Users

                     join ur in _db.Roles on u.Role equals ur.RoleId into join_u_ur
                     from uu_ur in join_u_ur

                     join ua in _db.Agencies on u.AgencyId equals ua.AgencyId into join_u_ua
                     from uu_ua in join_u_ua

                     join ut in _db.UserTypes on u.UserType equals ut.UserTypeId into join_u_ut
                     from uu_ut in join_u_ut

                     where u.Name.Contains(stext) ||
                           u.StudentId.Contains(stext) ||
                           u.Email.Contains(stext)

                     select new ViewModels.UserViewModel
                     {
                         user_id = u.UserId,
                         email = u.Email,
                         password = u.Password,
                         roleName = uu_ur.RoleName,
                         student_id = u.StudentId,
                         agencyName = uu_ua.AgencyName,
                         name = u.Name,
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
    }
}
