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
           
            return View();
        }
        
    }
}
