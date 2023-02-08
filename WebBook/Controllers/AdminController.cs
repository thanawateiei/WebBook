using Microsoft.AspNetCore.Mvc;
using WebBook.Models;
using WebBook.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebBook.Controllers
{
    public class AdminController : Controller
    {
        private readonly webContext _db;
        public AdminController(webContext db)
        { _db = db; }
        public IActionResult Index()
        {
            var rq = from r in _db.Histories
                     join ue in _db.Users on r.UserId equals ue.UserId into join_r_ue
                     from r_ue in join_r_ue.DefaultIfEmpty()
                     join s in _db.Statuses on r.StatusId equals s.StatusId into join_r_s
                     from r_s in join_r_s.DefaultIfEmpty()
                     select new Detail
                     {
                         UserEmail = r_ue.Email,
                         BookTitle = r.BookName,
                         ReceiveDate = r.ReceiveDate,
                         CallNumber = r.CallNumber,
                         Status = r_s.StatusName
                     };
            if (rq == null) return NotFound();
            return View(rq);
        }
        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
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
                            r.CallNumber.Contains(stext)
                     select new Detail
                     {
                         UserEmail = r_ue.Email,
                         BookTitle = r.BookName,
                         ReceiveDate = r.ReceiveDate,
                         CallNumber = r.CallNumber,
                         Status = r_s.StatusName
                     };
            if (rq == null) return NotFound();

            ViewBag.stext = stext;
            return View(rq);
        }
    }
}
