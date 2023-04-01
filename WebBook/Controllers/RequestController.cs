using Microsoft.AspNetCore.Mvc;
using WebBook.Models;
using WebBook.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

            var st = from b in _db.Statuses
                     select b;
            

            if (rqq == null) return NotFound();
            ViewBag.allData = rqq.Count();
            ViewBag.CountStatus = CStatus;
            ViewBag.Status = st;
            ViewBag.filterDate = null;
            ViewBag.pagenow = 1;
            ViewBag.ListNumber = 2;
            var list = 2;
            if ((rqq.Count() % list) == 0)
            {
                ViewBag.allListNum = rqq.Count() / list;
            }
            else
            {
                ViewBag.allListNum = (rqq.Count() / list) + 1;
            }
           
            rqq = rqq.GetRange(0, list);
            //ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName");
            return View(rqq);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/request")]
        public IActionResult Index(string stext, string filterDate, string filterStatus, DateTime dateStart, DateTime dateEnd , string T_btn , int ListNumber, int pageNow)
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
                        var dateS = dateStart;
                        string setY = DateTime.Now.Year.ToString();
                        int SY = (Int32.Parse(setY) + 543);
                        try
                        {
                            //TH
                            dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + SY);
                            var RQ = rqq.Where(r => (r.CreatedAt.Date >= dateS)).ToList();
                            if (RQ.Count() == 0)
                            {
                                try
                                {
                                    //US 
                                    dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + setY);
                                    rqq = rqq.Where(r => (r.CreatedAt.Date >= dateS)).ToList();
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                rqq = RQ;

                            }

                        }
                        catch (Exception ex)
                        {
                            //Uk
                            dateS = Convert.ToDateTime(dateStart.Month + "/" + dateStart.Day + "/" + setY);
                            rqq = rqq.Where(r => (r.CreatedAt.Date >= dateS)).ToList();

                        }
                        //rqq.OrderByDescending(c => c.CreatedAt.Date).ThenBy(c => c.CreatedAt.TimeOfDay);
                    }
                    else if (dateStart == DateTime.MinValue && dateEnd != DateTime.MinValue)
                    {

                        var dateE = dateEnd;
                        string setY = DateTime.Now.Year.ToString();
                        int SY = (Int32.Parse(setY) + 543);
                        try
                        {
                            //TH
                            dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + SY);
                            var RQ = rqq.Where(r => (r.CreatedAt.Date <= dateE)).ToList();
                            if (RQ.Count() == 0)
                            {
                                try
                                {
                                    //US 
                                    dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + setY);
                                    rqq = rqq.Where(r => (r.CreatedAt.Date <= dateE)).ToList();
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                rqq = RQ;

                            }

                        }
                        catch (Exception ex)
                        {
                            //Uk
                            dateE = Convert.ToDateTime(dateEnd.Month + "/" + dateEnd.Day + "/" + setY);
                            rqq = rqq.Where(r => (r.CreatedAt.Date <= dateE)).ToList();

                        }
                    }
                    else if (dateStart != DateTime.MinValue && dateEnd != DateTime.MinValue)
                    {
                        var dateS = dateStart;
                        var dateE = dateEnd;
                        string setY = DateTime.Now.Year.ToString();
                        int SY = (Int32.Parse(setY) + 543);
                        try
                        {
                            //TH
                            dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + SY);
                            dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + SY);
                            var RQ = rqq.Where(r => (r.CreatedAt.Date >= dateS) && (r.CreatedAt.Date <= dateE)).ToList();
                            if (RQ.Count() == 0)
                            {
                                try
                                {
                                    //US 
                                    dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + setY);
                                    dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + setY);
                                    rqq = rqq.Where(r => (r.CreatedAt.Date >= dateS) && (r.CreatedAt.Date <= dateE)).ToList();
                                }
                                catch (Exception ex)
                                {
                                }
							}
							else
							{
                                rqq = RQ;

                            }

                        }
                        catch (Exception ex)
                        {
                            //Uk
                            dateS = Convert.ToDateTime(dateStart.Month + "/" + dateStart.Day + "/" + setY);
                            dateE = Convert.ToDateTime(dateEnd.Month + "/" + dateEnd.Day + "/" + setY);
                            rqq = rqq.Where(r => (r.CreatedAt.Date >= dateS) && (r.CreatedAt.Date <= dateE)).ToList();

                        }



                    }
                  
                }
                else if (filterDate == "ReceiveDate")
                {
                    if (dateStart != DateTime.MinValue && dateEnd == DateTime.MinValue)
                    {
                        var dateS = dateStart;
                        var dateE = dateEnd;
                        string setY = DateTime.Now.Year.ToString();
                        int SY = (Int32.Parse(setY) + 543);
                        try
                        {
                            //TH
                            dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + SY);
                            var RQ = rqq.Where(r => (r.ReceiveDate.Date >= dateS)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();
                            if (RQ.Count() == 0)
                            {
                                try
                                {
                                    //US 
                                    dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + setY);
                                    rqq = rqq.Where(r => (r.ReceiveDate.Date >= dateS)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                rqq = RQ;

                            }

                        }
                        catch (Exception ex)
                        {
                            //Uk
                            dateS = Convert.ToDateTime(dateStart.Month + "/" + dateStart.Day + "/" + setY);
                            rqq = rqq.Where(r => (r.ReceiveDate.Date >= dateS)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();

                        }
                        //rqq.OrderByDescending(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay);
                    }
                    else if (dateStart == DateTime.MinValue && dateEnd != DateTime.MinValue)
                    {

                        var dateE = dateEnd;
                        string setY = DateTime.Now.Year.ToString();
                        int SY = (Int32.Parse(setY) + 543);
                        try
                        {
                            //TH
                            dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + SY);
                            var RQ = rqq.Where(r => (r.ReceiveDate.Date <= dateE)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();
                            if (RQ.Count() == 0)
                            {
                                try
                                {
                                    //US 
                                    dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + setY);
                                    rqq = rqq.Where(r => (r.ReceiveDate.Date <= dateE)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                rqq = RQ;

                            }

                        }
                        catch (Exception ex)
                        {
                            //Uk
                            dateE = Convert.ToDateTime(dateEnd.Month + "/" + dateEnd.Day + "/" + setY);
                            rqq = rqq.Where(r => (r.ReceiveDate.Date <= dateE)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();

                        }
                    }
                    else if (dateStart != DateTime.MinValue && dateEnd != DateTime.MinValue)
                    {
                        var dateS = dateStart;
                        var dateE = dateEnd;
                        string setY = DateTime.Now.Year.ToString();
                        int SY = (Int32.Parse(setY) + 543);
                        try
                        {
                            //TH
                            dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + SY);
                            dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + SY);
                            var RQ = rqq.Where(r => (r.ReceiveDate.Date >= dateS) && (r.ReceiveDate.Date <= dateE)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();
                            if (RQ.Count() == 0)
                            {
                                try
                                {
                                    //US 
                                    dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + setY);
                                    dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + setY);
                                    rqq = rqq.Where(r => (r.ReceiveDate.Date >= dateS) && (r.ReceiveDate.Date <= dateE)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                rqq = RQ;

                            }

                        }
                        catch (Exception ex)
                        {
                            //Uk
                            dateS = Convert.ToDateTime(dateStart.Month + "/" + dateStart.Day + "/" + setY);
                            dateE = Convert.ToDateTime(dateEnd.Month + "/" + dateEnd.Day + "/" + setY);
                            rqq = rqq.Where(r => (r.ReceiveDate.Date >= dateS) && (r.ReceiveDate.Date <= dateE)).OrderBy(c => c.ReceiveDate.Date).ThenBy(c => c.ReceiveDate.TimeOfDay).ToList();

                        }


                        
                    }
                }
               


            }
            else if(dateStart != DateTime.MinValue || dateEnd != DateTime.MinValue)
            {
                if (dateStart != DateTime.MinValue && dateEnd == DateTime.MinValue)
                {
                    var dateS = dateStart;
                    string setY = DateTime.Now.Year.ToString();
                    int SY = (Int32.Parse(setY) + 543);
                    try
                    {
                        //TH
                        dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + SY);
                        var RQ = rqq.Where(r => ((r.ReceiveDate.Date >= dateS) || (r.CreatedAt.Date >= dateS))).ToList();
                        if (RQ.Count() == 0)
                        {
                            try
                            {
                                //US 
                                dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + setY);
                                rqq = rqq.Where(r => ((r.ReceiveDate.Date >= dateS) || (r.CreatedAt.Date >= dateS))).ToList();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            rqq = RQ;

                        }

                    }
                    catch (Exception ex)
                    {
                        //Uk
                        dateS = Convert.ToDateTime(dateStart.Month + "/" + dateStart.Day + "/" + setY);
                        rqq = rqq.Where(r => ((r.ReceiveDate.Date >= dateS) || (r.CreatedAt.Date >= dateS))).ToList();

                    }
                    
                }
                else if (dateStart == DateTime.MinValue && dateEnd != DateTime.MinValue)
                {
                    var dateE = dateEnd;
                    string setY = DateTime.Now.Year.ToString();
                    int SY = (Int32.Parse(setY) + 543);
                    try
                    {
                        //TH
                        dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + SY);
                        var RQ = rqq.Where(r => ((r.ReceiveDate.Date <= dateE) || (r.CreatedAt.Date <= dateE))).ToList();

                        if (RQ.Count() == 0)
                        {
                            try
                            {
                                //US 
                                dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + setY);
                                rqq = rqq.Where(r => ((r.ReceiveDate.Date <= dateE) || (r.CreatedAt.Date <= dateE))).ToList();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            rqq = RQ;

                        }

                    }
                    catch (Exception ex)
                    {
                        //Uk
                        dateE = Convert.ToDateTime(dateEnd.Month + "/" + dateEnd.Day + "/" + setY);
                        rqq = rqq.Where(r => ((r.ReceiveDate.Date <= dateE) || (r.CreatedAt.Date <= dateE))).ToList();

                    }
                }
                else
                {
                    var dateS = dateStart;
                    var dateE = dateEnd;
                    string setY = DateTime.Now.Year.ToString();
                    int SY = (Int32.Parse(setY) + 543);
                    try
                    {
                        //TH
                        dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + SY);
                        dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + SY);
                        var RQ = rqq.Where(r => ((r.ReceiveDate.Date >= dateS) && (r.ReceiveDate.Date <= dateE)) || ((r.CreatedAt.Date >= dateS) && (r.CreatedAt.Date <= dateE))).ToList();

                        if (RQ.Count() == 0)
                        {
                            try
                            {
                                //US 
                                dateS = Convert.ToDateTime(dateStart.Day + "/" + dateStart.Month + "/" + setY);
                                dateE = Convert.ToDateTime(dateEnd.Day + "/" + dateEnd.Month + "/" + setY);
                                rqq = rqq.Where(r => ((r.ReceiveDate.Date >= dateS) && (r.ReceiveDate.Date <= dateE)) || ((r.CreatedAt.Date >= dateS) && (r.CreatedAt.Date <= dateE))).ToList();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            rqq = RQ;

                        }

                    }
                    catch (Exception ex)
                    {
                        //Uk
                        dateS = Convert.ToDateTime(dateStart.Month + "/" + dateStart.Day + "/" + setY);
                        dateE = Convert.ToDateTime(dateEnd.Month + "/" + dateEnd.Day + "/" + setY);
                        rqq = rqq.Where(r => ((r.ReceiveDate.Date >= dateS) && (r.ReceiveDate.Date <= dateE)) || ((r.CreatedAt.Date >= dateS) && (r.CreatedAt.Date <= dateE))).ToList();

                    }
                }
            }

            if (filterStatus != null)
            {
                rqq = rqq.Where(r => (r.Status.Contains(filterStatus))).OrderByDescending(c => c.UpdatedAt.Date).OrderBy(c => c.UpdatedAt.TimeOfDay).ToList();
            }
            //set Filter
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
            ViewBag.allData = rqq.Count();
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
            // Filter End

           

            //Page Number
            if (pageNow == 0)
            {
                pageNow = 1;
            }

            if (ListNumber == 0)
            {
                ListNumber = 2;
            }
            var pageNumber = pageNow;
            var allList = 0;  
            // set allList
            if ((rqq.Count() % ListNumber) == 0 )
            {
                 allList = rqq.Count() / ListNumber;
            }
            else
            {
                 allList = (rqq.Count() / ListNumber)+1;
            }
            
            var listS = (pageNumber * ListNumber) - ListNumber;
            var EListNum = 0;
            var listData = new List<RequestViewModel>();
            //ถ้า ข้อมูลน้อยกว่าจำนวนที่ต้องการ
            //GetRange(เริ่ม, จำนวน);
            if ((ListNumber*pageNumber) > rqq.Count())
            {
                listData = rqq.GetRange(listS, ((rqq.Count())-listS   ));
                EListNum = (rqq.Count())-listS;
            }
            else
            {
                listData = rqq.GetRange(listS, ListNumber);
                EListNum = ListNumber;

            }
            ViewBag.allListNum = allList;
            ViewBag.ListNumber = ListNumber;
            ViewBag.pagenow = pageNow; 
            //Page Number END

            // Print
            if (T_btn == "print")
            {
                return print(listData, stext, filterDate, filterStatus, dateStart, dateEnd);
            }
            // Print END


           
            return View(listData);

        }


        public IActionResult print(List<RequestViewModel> rqq, string stext, string filterDate, string filterStatus, DateTime dateStart, DateTime dateEnd)
        {
            try
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
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
           
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

                    obj.CreatedAt = _db.Histories.AsNoTracking().FirstOrDefault(ue => ue.HistoryId == obj.HistoryId).CreatedAt;
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