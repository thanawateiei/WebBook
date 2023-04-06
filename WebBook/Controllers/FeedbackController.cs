using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBook.Models;

namespace WebBook.Controllers
{

    public class FeedbackController : Controller
    {
        private readonly webContext _db;
        public FeedbackController(webContext db)
        { _db = db; }

        [Route("Admin/Feedback")]
        public ActionResult Index()
        {
            var fbs = from fb in _db.Feedbacks
                      join ue in _db.Users on fb.UserId equals ue.UserId into join_fb_ue
                      from fb_ue in join_fb_ue.DefaultIfEmpty()
                      select new Feedback
                      {
                          FeedbackId = fb.FeedbackId,
                          UserId = fb_ue.Email,
                          FeedbackLike = fb.FeedbackLike,
                          FeedbackScore = fb.FeedbackScore,
                          FeedbackDetail= fb.FeedbackDetail,
                          FeedbackHeading = fb.FeedbackHeading
                      };


            if (fbs == null) return NotFound();
            return View(fbs);
        }
        public IActionResult Detail(int id)
		{
			if (id == 0)
			{
				return RedirectToAction("Index");

			}
			var obj = _db.Feedbacks.Find(id);
			if (obj == null)
			{
				ViewBag.ErrorMessage = "ไม่พบรายการนี้";
				return RedirectToAction("Index");

			}
			var UserEmail = _db.Users.FirstOrDefault(ue => ue.UserId == obj.UserId).Email;
            obj.UserId = UserEmail;
			return PartialView(obj);
		}

	}
}
