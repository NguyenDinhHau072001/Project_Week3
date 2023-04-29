﻿using Microsoft.AspNetCore.Mvc;
using ProjectDATN.Data.EF;
using ProjectDATN.Data.Entities;
using X.PagedList;

namespace ProjectDATN.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BrandController : Controller
	{
		private readonly ApplicationDBContext _db;

		public BrandController(ApplicationDBContext db)
		{
			_db = db;
		}

		public IActionResult Index(int? page)
		{
			var listOfCategories = _db.Brands.ToList();

			int pageSize = 5;
			int pageNumber = (page ?? 1);

			return View(listOfCategories.OrderByDescending(x => x.Id).ToPagedList(pageNumber, pageSize));
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Brand a)
		{
			if (ModelState.IsValid)
			{
				_db.Brands.Add(a);
				_db.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(_db.Brands);
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			if (id == 0 || id == null)
				return NotFound();
			var brandID = _db.Brands.FirstOrDefault(u => u.Id == id);
			if (brandID == null)
				return NotFound();
			return View(brandID);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Brand a)
		{
			if (ModelState.IsValid)
			{
				_db.Brands.Update(a);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(a);
		}

		/*public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var categoryID = _db.Brands.FirstOrDefault(u => u.Id == id);
            if (categoryID == null)
                return NotFound();
            return View(categoryID);
        }*/

		public IActionResult Delete(int? id)
		{
			if (id == 0 || id == null)
				return NotFound();
			var brand = _db.Brands.FirstOrDefault(u => u.Id == id);
			if (brand == null)
				return NotFound();
			return PartialView("DeleteCategory", brand);
		}

		[HttpPost]
		public IActionResult Delete(Brand a)
		{
			_db.Brands.Remove(a);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}