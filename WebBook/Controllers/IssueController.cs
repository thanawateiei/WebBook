using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBook.Models;

namespace WebBook.Controllers
{
    public class IssueController : Controller
    {
        private readonly webContext _db;
        public IssueController(webContext db)
        { _db = db; }
        public ActionResult Index()
        {
            var iss = from issue in _db.Issues
                      join ue in _db.Users on issue.UserId equals ue.UserId into join_issue_ue
                      from issue_ue in join_issue_ue.DefaultIfEmpty()
                      select new Issue
                      {
                          IssueId = issue.IssueId,
                          UserId = issue_ue.Email,
                          IssueTitle = issue.IssueTitle,
                          IssueDetail = issue.IssueDetail,
                          IssueStatus = issue.IssueStatus
                      };
            if (iss == null) return NotFound();
            return View(iss);
        }
        public IActionResult Detail(int id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            var obj = _db.Issues.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMessage = "ไม่พบรายการนี้";
                return RedirectToAction("Index");

            }
            var UserEmail = _db.Users.FirstOrDefault(ue => ue.UserId == obj.UserId).Email;
            obj.UserId = UserEmail;
            return PartialView(obj);
        }
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                TempData["Message"] = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            var obj = _db.Issues.Find(id);
            if (obj == null)
            {
                TempData["Message"] = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }
			return PartialView(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Issue obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obj.UserId = obj.UserId;
                    _db.Issues.Update(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Index","Issue");
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
    }
}
