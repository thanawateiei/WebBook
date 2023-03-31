using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using WebBook.Models;
using WebBook.ViewModels;

namespace WebBook.Controllers
{
    public class SettingController : Controller
    {
        // GET: BookController
        private readonly webContext _db;
        public SettingController(webContext db)
        { _db = db; }


        [Route("Admin/Setting")]
        public IActionResult Index()
        {
            var stDetail = (  from s in _db.Settings
                         where s.Detail != null
                         select new Setting
                         {
                            SettingId = s.SettingId,
                            Detail = s.Detail,
                            DetailStatus = s.DetailStatus
                         }).OrderByDescending(c => c.SettingId).ThenBy(c => c.SettingId);

            var stImg = (from s in _db.Settings
                       where s.BannerImg != null
                       select new Setting
                       {
                           SettingId = s.SettingId,
                           BannerImg = s.BannerImg,
                           BannerStatus = s.BannerStatus
                       }).OrderByDescending(c => c.SettingId).ThenBy(c => c.SettingId);

            List<Setting> SetDetail = new List<Setting>();
            SetDetail.AddRange(stDetail);

            List<Setting> Setimg = new List<Setting>();
            Setimg.AddRange(stImg);
            ViewBag.SetDetail = SetDetail;
            ViewBag.Setimg = Setimg;

            return View();

        }


        [Route("Admin/Setting/Create")]
        public ActionResult Create()
        {
            
            return View();
        }
        // POST: SettingController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("Admin/Setting/Create")]
        public ActionResult Create(SettingViewModel obj)
        {
            try
            {
                //CheckBookType cbt = new CheckBookType();
                var Setting = from s in _db.Settings
                              select s.SettingId;
                var newidSetting = (Setting.Max())+1;
                Setting Set = new Setting();
                if (ModelState.IsValid)
                {
                    if (obj.Settingimg != null)
                    {
                        //save
                        var LocalfileName = Path.GetFileName(obj.Settingimg.FileName);
                        var NewFileName = "Set"+ newidSetting;
                        var FileExtension = Path.GetExtension(LocalfileName);
                        var UpFileName = NewFileName + FileExtension;
                        var savedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\setting");
                        var Filepath = Path.Combine(savedir, UpFileName);

                        using (FileStream fs = System.IO.File.Create(Filepath))
                        {
                            obj.Settingimg.CopyTo(fs);
                            fs.Flush();
                        }
                        Set.BannerImg = UpFileName;

                    }
                   
                    Set.SettingId = newidSetting;
                    Set.BannerStatus = obj.BannerStatus;
                    Set.Detail = obj.Detail;
                    Set.DetailStatus = obj.DetailStatus;


                    _db.Settings.Add(Set);
                    _db.SaveChanges();

                    return RedirectToAction("index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            ViewBag.ErrorMessage = "การบันทึกผิดพลาด";

            return RedirectToAction("index");
        }

        // GET: SettingController/Edit/5
        [Route("Admin/Setting/Edit")]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");

            }
            var obj = _db.Settings.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMessage = "ไม่พบรายการนี้";
                return RedirectToAction("Index");

            }
            SettingViewModel Set  = new SettingViewModel();
            Set.SettingId = obj.SettingId;
            Set.BannerImg = obj.BannerImg;
            Set.BannerStatus = obj.BannerStatus;
            Set.Detail = obj.Detail;
            Set.DetailStatus = obj.DetailStatus;
            ViewBag.BannerImg = obj.BannerImg;
            ViewBag.BannerStatus = obj.BannerStatus;
            ViewBag.DetailStatus = obj.DetailStatus;
            return View(Set);
        }

        // POST: SettingController/Edit/5
        [HttpPost]
        [Route("Admin/Setting/Edit")]
        public ActionResult Edit(SettingViewModel obj)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    Setting Set = new Setting();
                    Set.SettingId = obj.SettingId;
                    Set.BannerStatus = obj.BannerStatus;
                    Set.Detail = obj.Detail;
                    Set.DetailStatus = obj.DetailStatus;

                    if (obj.Settingimg != null)
                    {
                        //delete
                        var fileName = obj.BannerImg;
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\setting");
                        if (fileName != null)
                        {
                            var filePath = Path.Combine(imagePath, fileName);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }


                        //save
                        var LocalfileName = Path.GetFileName(obj.Settingimg.FileName);
                        var NewFileName = "Set" + obj.SettingId;
                        var FileExtension = Path.GetExtension(LocalfileName);
                        var UpFileName = NewFileName + FileExtension;
                        var savedir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\setting");
                        var Filepath = Path.Combine(savedir, UpFileName);

                        using (FileStream fs = System.IO.File.Create(Filepath))
                        {
                            obj.Settingimg.CopyTo(fs);
                            fs.Flush();
                        }
                        Set.BannerImg = UpFileName;

                    }
                    else
                    {
                        Set.BannerImg = obj.BannerImg;
                    }

                    _db.Settings.Update(Set);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = "การบันทึกผิดพลาด";
            return RedirectToAction("Index");
        }

        // GET: SettingController/Delete/5
        public ActionResult Delete(int id)
        {
            //ตรวจสอบว่ามีการส่ง id มาหรือไม่
            if (id == 0)
            {
                ViewBag.ErrorMassage = "ต้องระบุค่า ID";
                return RedirectToAction("Index");
            }
            // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Settings.Find(id);
            if (obj == null)
            {
                ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                return RedirectToAction("Index");
            }
            ViewBag.img = obj.BannerImg;
            return View(obj);
        }

        // POST: SettingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int SettingId)
        {
            try
            {
                // ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
                var obj = _db.Settings.Find(SettingId);
                if (obj == null)
                {
                    ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
                    return RedirectToAction("Index");
                }
                if (obj.BannerImg != null){
                    var fileName = obj.BannerImg;
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\setting");
                    var filePath = Path.Combine(imagePath, fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                }
               
                _db.Settings.Remove(obj); //ส่งคำสั่ง Remove ผ่าน DBContext
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


