using Microsoft.AspNetCore.Mvc;
using WebBook.Models;

namespace WebBook.Controllers
{
    public class DetailController : Controller
    {
        private readonly webContext _db;
        public DetailController(webContext db)
        { _db = db; }
        public IActionResult Index()
        {
            var pd = from p in _db.Statuses
                     select p;
            return View(pd);
        }
    }
}
