﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBook.Models;

namespace WebBook.Controllers
{
	public class BookTypeController : Controller
	{
		private readonly webContext _db;
		public BookTypeController(webContext db)
		{ _db = db; }
		// GET: BookTypeController

		public ActionResult Index()
		{
			var bt = from b in _db.BookTypes
					 select new ViewModels.BookTypeViewModel
					 {
						 BookTypeId = b.BookTypeId,
						 BookTypeName = b.BookTypeName
					 };

			
			if (bt == null) return NotFound();
			return View(bt);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(string? stext)
		{
            var bt = from b in _db.BookTypes
                     where b.BookTypeName.Contains(stext)
                     select new ViewModels.BookTypeViewModel
                     {
                         BookTypeId = b.BookTypeId,
                         BookTypeName = b.BookTypeName
                     };

            if (bt == null) return NotFound();
            ViewBag.stext = stext;
            return View(bt);
           
		}



		// GET: BookTypeController/Create
		public ActionResult Create()
		{
			var idBookTypes = from b in _db.BookTypes
						 select b.BookTypeId;
			var newidbookTypes = idBookTypes.Max();
			ViewBag.newidbookTypes = newidbookTypes + 1;

			return View();
		}

		// POST: BookTypeController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(BookType obj)
		{
			try
			{

				if (ModelState.IsValid)
				{
			
					_db.BookTypes.Add(obj);
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

		// GET: BookTypeController/Edit/5
		
		public ActionResult Edit(int id)
		{
			if (id == 0)
			{
				return RedirectToAction("Index");

			}
			var obj = _db.BookTypes.Find(id);
			if (obj == null)
			{
				ViewBag.ErrorMessage = "ไม่พบรายการนี้";
				return RedirectToAction("Index");

			}
			return View(obj);
		}

		// POST: BookTypeController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(BookType obj)
		{
			try
			{

				if (ModelState.IsValid)
				{
					_db.BookTypes.Update(obj);
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

		// GET: BookTypeController/Delete/5
		public ActionResult Delete(int id)
		{
			//ตรวจสอบว่ามีการส่ง id มาหรือไม่
			if (id == 0)
			{
				ViewBag.ErrorMassage = "ต้องระบุค่า ID";
				return RedirectToAction("Index");
			}
			// ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
			var obj = _db.BookTypes.Find(id);
			if (obj == null)
			{
				ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
				return RedirectToAction("Index");
			}

			//ViewBag.btName = _db.BookTypes.FirstOrDefault(bt => bt.BookTypeId == obj.BookType1).BookTypeId;
			return View(obj);
		}

		// POST: BookTypeController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeletePost(int BookTypeId)
		{
			try
			{
				// ทำการเขียน Query หา Record ของ Product.pdId จาก id ที่ส่งมา
				var books = from b in _db.Books
						   where (b.BookType1.Equals(BookTypeId)) || (b.BookType2.Equals(BookTypeId)) || (b.BookType3.Equals(BookTypeId)) || (b.BookType4.Equals(BookTypeId)) ||
								 (b.BookType5.Equals(BookTypeId)) || (b.BookType6.Equals(BookTypeId)) || (b.BookType7.Equals(BookTypeId)) || (b.BookType8.Equals(BookTypeId)) ||
								 (b.BookType9.Equals(BookTypeId)) || (b.BookType10.Equals(BookTypeId))
						   select b;
				List<Book> book = books.ToList();
				for (int i = 0; i < book.Count(); i++)
				{
					if (book[i].BookType1 == BookTypeId)
					{
						book[i].BookType1 = 0;
					}
					if (book[i].BookType2 == BookTypeId)
					{
						book[i].BookType2 = 0;
					}
					if (book[i].BookType3 == BookTypeId)
					{
						book[i].BookType3 = 0;
					}
					if (book[i].BookType4 == BookTypeId)
					{
						book[i].BookType4 = 0;
					}
					if (book[i].BookType5 == BookTypeId)
					{
						book[i].BookType5 = 0;
					}
					if (book[i].BookType6 == BookTypeId)
					{
						book[i].BookType6 = 0;
					}
					if (book[i].BookType7 == BookTypeId)
					{
						book[i].BookType7 = 0;
					}
					if (book[i].BookType8 == BookTypeId)
					{
						book[i].BookType8 = 0;
					}
					if (book[i].BookType9 == BookTypeId)
					{
						book[i].BookType9 = 0;
					}
					if (book[i].BookType10 == BookTypeId)
					{
						book[i].BookType10 = 0;
					}
					_db.Books.Update(book[i]);
					
				}_db.SaveChanges(); 

				var obj = _db.BookTypes.Find(BookTypeId);
				if (obj == null)
				{
					ViewBag.ErrorMassage = "ไม่พบข้อมูลที่ระบุ";
					return RedirectToAction("Index");
				}
	
				_db.BookTypes.Remove(obj); //ส่งคำสั่ง Remove ผ่าน DBContext
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
