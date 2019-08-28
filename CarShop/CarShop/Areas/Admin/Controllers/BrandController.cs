using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
using CarShop.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var lstBrand = _unitOfWork.Repository<Brand>().GetAll();
            return View(lstBrand);
        }

        public IActionResult Create()
        {
            return PartialView("CreateBrand");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid) return View("CreateBrand", brand);
            await _unitOfWork.Repository<Brand>().InsertAsync(brand);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var brandFromDb = _unitOfWork.Repository<Brand>().GetById(id);
            return PartialView("EditBrand",brandFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Brand brand)
        {
            if (id != brand.Id || !ModelState.IsValid) return View("EditBrand", brand);

            var brandFromDb = await _unitOfWork.Repository<Brand>().GetByIdAsync(id);
            if (brandFromDb == null) return NotFound("No brand item");
            brandFromDb.Name = brand.Name;
            brandFromDb.Description = brand.Description;
            brandFromDb.ModifiedDate = brand.ModifiedDate;
            await _unitOfWork.Repository<Brand>().UpdateAsync(brandFromDb);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Brand brand = _unitOfWork.Repository<Brand>().GetById(id);
            if (brand != null)
                _unitOfWork.Repository<Brand>().Delete(brand);
            return RedirectToAction("Index");
        }
    }
}