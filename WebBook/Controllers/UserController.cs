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

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
