using Microsoft.AspNetCore.Mvc;
using WebBook.Models;
using WebBook.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Collections.Generic;

namespace WebBook.Controllers
{
    public class RequestController : Controller
    {
        private readonly webContext _db;
        public RequestController(webContext db)
        { _db = db; }
        [Route("admin/request")]
        public IActionResult Index()
        {
            //DateTime dt = DateTime.ParseExact(yourObject.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            //string s = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            List<RequestViewModel> rqq = new List<RequestViewModel>();

            var rq = (from r in _db.Histories
                      join ue in _db.Users on r.UserId equals ue.UserId into join_r_ue
                      from r_ue in join_r_ue.DefaultIfEmpty()
                      join s in _db.Statuses on r.StatusId equals s.StatusId into join_r_s
                      from r_s in join_r_s.DefaultIfEmpty()
                      join l in _db.Locations on r.LocationId equals l.LocationId into join_r_l
                      from r_l in join_r_l.DefaultIfEmpty()
                      select new RequestViewModel
                      {
                          RequestId = r.HistoryId,
                          UserEmail = r_ue.Email,
                          BookTitle = r.BookName,
                          LocationName = r_l.LocationName,
                          CallNumber = r.CallNumber,
                          Status = r_s.StatusName,
                          CreatedAt = r.CreatedAt,
                          ReceiveDate = r.ReceiveDate,
                          UpdatedAt = r.UpdatedAt,
                          ReturnDate = r.ReturnDate,

                          S_ReceiveDate = Convert.ToDateTime(r.ReceiveDate).ToString("dd/MM/yyyy"),
                          S_ReturnDate = Convert.ToDateTime(r.ReturnDate).ToString("dd/MM/yyyy"),
                          S_CreatedAt = Convert.ToDateTime(r.CreatedAt).ToString("dd/MM/yyyy")
                      }).OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);

            
            List<CountStatusViewModel> CStatus = new List<CountStatusViewModel>();
            for(var s = 1; s <= _db.Statuses.Count(); s++)
            {
                CountStatusViewModel CS = new CountStatusViewModel();
                var sta = _db.Statuses.FirstOrDefault(ue => ue.StatusId == s);
                CS.StatusId = sta.StatusId;
                CS.StatusName = sta.StatusName;
                CS.Count = _db.Histories.Where(x => x.StatusId.Equals(s)).Count();
                CStatus.Add(CS);
            }
           

            rqq = rq.ToList();
            var i = 0;
            foreach (var r in rqq)
            {
                if (r.CreatedAt == r.UpdatedAt)
                {
                    rqq[i].state = "new";
                }
            }

            var st = from b in _db.Statuses
                     select b;
            

            if (rqq == null) return NotFound();
            ViewBag.CountStatus = CStatus;
            ViewBag.Status = st;
            ViewBag.filterDate = null;
            //ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName");
            return View(rqq);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/request")]
        public IActionResult Index(string stext, string filterDate, string filterStatus, DateTime dateStart, DateTime dateEnd , string T_btn)
        {

            List<RequestViewModel> rqq = new List<RequestViewModel>();

            var rq = (from r in _db.Histories
                      join ue in _db.Users on r.UserId equals ue.UserId into join_r_ue
                      from r_ue in join_r_ue.DefaultIfEmpty()
                      join s in _db.Statuses on r.StatusId equals s.StatusId into join_r_s
                      from r_s in join_r_s.DefaultIfEmpty()
                      join l in _db.Locations on r.LocationId equals l.LocationId into join_r_l
                      from r_l in join_r_l.DefaultIfEmpty()
                      select new RequestViewModel
                      {
                          RequestId = r.HistoryId,
                          UserEmail = r_ue.Email,
                          BookTitle = r.BookName,
                          LocationName = r_l.LocationName,
                          CallNumber = r.CallNumber,
                          Status = r_s.StatusName,
                          CreatedAt = r.CreatedAt,
                          ReceiveDate = r.ReceiveDate,
                          UpdatedAt = r.UpdatedAt,
                          ReturnDate = r.ReturnDate,

                          S_ReceiveDate = Convert.ToDateTime(r.ReceiveDate).ToString("dd/MM/yyyy"),
                          S_ReturnDate = Convert.ToDateTime(r.ReturnDate).ToString("dd/MM/yyyy"),
                          S_CreatedAt = Convert.ToDateTime(r.CreatedAt).ToString("dd/MM/yyyy")
                      }).OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);

            rqq = rq.ToList();
            var i = 0;
            foreach (var r in rqq)
            {
                if (r.CreatedAt == r.UpdatedAt)
                {
                    rqq[i].state = "new";
                }
            }

            // Filter
            if (stext != null)
            {
                rqq = rqq.Where(r => (r.BookTitle.Contains(stext) || r.CallNumber.Contains(stext) || r.UserEmail.Contains(stext) || r.RequestId.Contains(stext))).ToList();
            }

            if (filterDate != null)
            {
                if (filterDate == "CreatedAt")
                {// (r.CreatedAt.Date >= dateStart.Date && r.CreatedAt.Date <= dateEnd.Date)
                    if (dateStart != DateTime.MinValue && dateEnd == DateTime.MinValue)
                    {
                        rqq = rqq.Where(r => (r.CreatedAt.Date >= dateStart.Date)).ToList();
                    }
                    else if (dateStart == DateTime.MinValue && dateEnd != DateTime.MinValue)
                    {
                        rqq = rqq.Where(r => (r.CreatedAt.Date <= dateEnd.Date)).ToList();
                    }
                    else if(dateStart != DateTime.MinValue && dateEnd != DateTime.MinValue)
                    {
                        rqq = rqq.Where(r => (r.CreatedAt.Date >= dateStart.Date) && (r.CreatedAt.Date <= dateEnd.Date)).ToList();
                    }
                }
                else if (filterDate == "ReceiveDate")
                {
                    if (dateStart != DateTime.MinValue && dateEnd == DateTime.MinValue)
                    {
                        rqq = rqq.Where(r => (r.ReceiveDate.Date >= dateStart.Date)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();
                        //rqq.OrderByDescending(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay);
                    }
                    else if (dateStart == DateTime.MinValue && dateEnd != DateTime.MinValue)
                    {
                        rqq = rqq.Where(r => (r.ReceiveDate.Date <= dateEnd.Date)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();
                    }
                    else if (dateStart != DateTime.MinValue && dateEnd != DateTime.MinValue)
                    {
                        rqq = rqq.Where(r => (r.ReceiveDate.Date >= dateStart.Date) && (r.ReceiveDate.Date <= dateEnd.Date)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();
                    }
                }
               


            }
            else if(dateStart != DateTime.MinValue || dateEnd != DateTime.MinValue)
            {
                if (dateStart != DateTime.MinValue && dateEnd == DateTime.MinValue)
                {
                    rqq = rqq.Where(r => ((r.ReceiveDate.Date >= dateStart.Date) || (r.CreatedAt.Date >= dateStart.Date))).ToList();
                }
                else if (dateStart == DateTime.MinValue && dateEnd != DateTime.MinValue)
                {
                    rqq = rqq.Where(r => ((r.ReceiveDate.Date <= dateEnd.Date) || (r.CreatedAt.Date <= dateStart.Date))).ToList();
                }
                else
                {
                    rqq = rqq.Where(r => ((r.ReceiveDate.Date >= dateStart.Date) && (r.ReceiveDate.Date <= dateEnd.Date)) || ((r.CreatedAt.Date >= dateStart.Date) && (r.CreatedAt.Date <= dateEnd.Date))).ToList();
                }
            }

            if (filterStatus != null)
            {
                rqq = rqq.Where(r => (r.Status.Contains(filterStatus))).OrderByDescending(c => c.UpdatedAt.Date).OrderBy(c => c.UpdatedAt.TimeOfDay).ToList();
            }
            // Filter End
            List<CountStatusViewModel> CStatus = new List<CountStatusViewModel>();
            for (var s = 1; s <= _db.Statuses.Count(); s++)
            {
                CountStatusViewModel CS = new CountStatusViewModel();
                var sta = _db.Statuses.FirstOrDefault(ue => ue.StatusId == s);
                CS.StatusId = sta.StatusId;
                CS.StatusName = sta.StatusName;
                CS.Count = _db.Histories.Where(x => x.StatusId.Equals(s)).Count();
                CStatus.Add(CS);
            }

            ViewBag.stext = stext;
            var st = from b in _db.Statuses
                     where filterStatus != b.StatusName
                     select b;
            ViewBag.Status = st;
            ViewBag.filterDate = filterDate;
            ViewBag.filterStatus = ViewBag.filterBT = _db.Statuses.FirstOrDefault(ue => ue.StatusName == filterStatus);
            ViewBag.CountStatus = CStatus;
            if (dateStart != DateTime.MinValue.Date)
            {
                ViewBag.dateStart = dateStart.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.dateStart = null;
            }
            if (dateEnd != DateTime.MinValue.Date)
            {
                ViewBag.dateEnd = dateEnd.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.dateEnd = null;
            }
           

            if(T_btn == "print")
            {
                return print(rqq,stext, filterDate,  filterStatus,  dateStart,  dateEnd);
            }
            return View(rqq);

        }
        public IActionResult print(List<RequestViewModel> rqq, string stext, string filterDate, string filterStatus, DateTime dateStart, DateTime dateEnd)
        {
            ViewBag.stext = stext;
            ViewBag.filterDate = filterDate;
            ViewBag.filterStatus = filterStatus;
            var dateStart1 = "";
            var dateEnd1 = "";
            if (dateStart != DateTime.MinValue )
            {
                dateStart1 = Convert.ToDateTime(dateStart).ToString("dd/MM/yyyy");
            }
            else
            {
                dateStart1 = "ไม่มี";

            }
            if (dateEnd != DateTime.MinValue)
            {
                dateEnd1 = Convert.ToDateTime(dateEnd).ToString("dd/MM/yyyy");
            }
            else
            {
                dateEnd1 = "ไม่มี";

            }
            ViewBag.dateStart = dateStart1;
            ViewBag.dateEnd = dateEnd1;
            return PartialView("print", rqq);
        }
        [Route("admin/request/edit/{id}")]
        public IActionResult Edit(string id)
        {
            //ตรวจสอบว่ามีการส่ง id มาหรือไม่
            if (id == null)
            {
                ViewBag.ErrorMassage = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            //var obj = _db.Histories.FirstOrDefault(ue => ue.HistoryId == id);
            var obj = _db.Histories.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }

            //อ่านข้อมูลจากตารางลง SelectList แล้วใส่ข้อมูลลงตัว ViewData
            //และกำนหนดว่า Select ที่เลือก เป็น id ของ obj นั้นๆ

            ViewData["Location"] = new SelectList(_db.Locations, "LocationId", "LocationName");
            ViewBag.UserEmail = _db.Users.FirstOrDefault(ue => ue.UserId == obj.UserId).Email;
            ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName", obj.StatusId);
            return View(obj);
        }

        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken]
        [Route("admin/request/edit/{id}")]
        public IActionResult Edit(History obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obj.UpdatedAt = DateTime.Now;
                    obj.ReturnDate = DateTime.Now;
                    _db.Histories.Update(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            ViewBag.ErrorMessage = "การแก้ไขผิดพลาด";
            ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName", obj.StatusId);
            return View(obj);

        }
        [Route("admin/request/delete")]
        public IActionResult Delete(string id)
        {
            //ตรวจสอบว่ามีการส่ง id มาหรือไม่
            if (id == null)
            {
                ViewBag.ErrorMassage = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Histories.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }

            ViewBag.UserEmail = _db.Users.FirstOrDefault(ue => ue.UserId == obj.UserId).Email;
            ViewBag.StatusName = _db.Statuses.FirstOrDefault(sn => sn.StatusId == obj.StatusId).StatusName;
            return View(obj);
        }

        [HttpPost] //ระบุว่าเป็นการทำงานแบบ Post
        [ValidateAntiForgeryToken] // ป้องกันการโจมตี Cross_site Request Forgery
        //**** ค่าที่ส่งมาจาก Form เป็น string  ต้องรับค่าเป็น string
        // แต่ถ้ารับค่าเป็น string จะ Error เพราะเป็นการประกาศ method ซ้ำจึงต้องตั้งชื่อใหม่ เป็น DeletePost
        // และตัวแปรที่รับ จะต้องเหมือนกับชือที่ส่งมาจาก View ด้วย จากตัวอย่างคือ PdId
        public IActionResult DeletePost(string RequestId)
        {
            try
            {
                // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
                var obj = _db.Histories.Find(RequestId);
                if (obj == null)
                {
                    ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                    return RedirectToAction("Index");
                }
                _db.Histories.Remove(obj); //ส่งคำสั่ง Remove ผ่าน DBContext
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