using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
using CarShop.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        
        private readonly RoleManager<ApplicationRoles> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(
            RoleManager<ApplicationRoles> roleManager,
            IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create()
        {
            return PartialView("CreateRole");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationRoles role)
        {
            if (!ModelState.IsValid) return View("CreateRole", role);
            role.NormalizedName = role.Name.ToLower();
            await _roleManager.CreateAsync(role);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (String.IsNullOrEmpty(id)) return NotFound();
            var roleFromDb = await _roleManager.FindByIdAsync(id);
            return PartialView("EditRole",roleFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,ApplicationRoles role)
        {
            if (id != role.Id || !ModelState.IsValid) return View("EditRole", role);

            var roleFromDb =  await _roleManager.FindByIdAsync(id);
            if (roleFromDb == null) return NotFound("No role item");
            roleFromDb.Name = role .Name;
            roleFromDb.Description = role.Description;
            roleFromDb.NormalizedName = role.Name.ToLower();
            await _roleManager.UpdateAsync(roleFromDb);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (String.IsNullOrEmpty(id)) return NotFound();
            ApplicationRoles role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = _roleManager.DeleteAsync(role).Result;
            }
            return RedirectToAction("Index");
        }
    }
}