using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Data;
using CarShop.Models;
using CarShop.Models.ViewModels;
using CarShop.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty] public CategoryViewModel CategoryVm { get; set; }

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            CategoryVm = new CategoryViewModel
            {
                Categories = _unitOfWork.Repository<Category>().GetAll(),
                Category = new Category()
            };
        }
        public IActionResult Index()
        {
            var lstCategory = _unitOfWork.Repository<Category>().GetAllInclude(c => c.CategoryParent);
            return View(lstCategory);
        }

        public IActionResult Create()
        {
            CategoryVm.Categories = _unitOfWork.Repository<Category>().GetAll();
            return PartialView("Create",CategoryVm);
        }
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            if (!ModelState.IsValid) return View("Create", CategoryVm);
            if (_unitOfWork.Repository<Category>().Exist(c => c.Name == CategoryVm.Category.Name))
            {
                return BadRequest("Category exists");
            }

            await _unitOfWork.Repository<Category>().InsertAsync(CategoryVm.Category);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var categoryFromDb = _unitOfWork.Repository<Category>().GetAllInclude(c => c.CategoryParent)
                .FirstOrDefault(c => c.Id == id);
            CategoryVm.Category = categoryFromDb;
            CategoryVm.Categories = _unitOfWork.Repository<Category>().GetAll();
            return PartialView("Edit",CategoryVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != CategoryVm.Category.Id || !ModelState.IsValid) return View("Edit",CategoryVm);

            var categoryFromDb = _unitOfWork.Repository<Category>().GetAllInclude(c => c.CategoryParent)
                .FirstOrDefault(c => c.Id == id);
            if (categoryFromDb == null) return NotFound();
            categoryFromDb.Name = CategoryVm.Category.Name;
            categoryFromDb.ModifiedDate = CategoryVm.Category.ModifiedDate;
            categoryFromDb.Description = CategoryVm.Category.Description;
            categoryFromDb.CategoryId = CategoryVm.Category.CategoryId;

            await _unitOfWork.Repository<Category>().UpdateAsync(categoryFromDb);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Category category = _unitOfWork.Repository<Category>().GetById(id);
            if(category!=null)
                _unitOfWork.Repository<Category>().Delete(category);
            return RedirectToAction("Index");
        }
    }
}