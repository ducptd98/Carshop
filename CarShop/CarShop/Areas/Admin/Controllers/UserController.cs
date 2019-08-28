using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
using CarShop.Models.ViewModels;
using CarShop.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty] public UsersViewModel UsersVm { get; set; }

        public UserController(UserManager<ApplicationUsers> userManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }

        public IActionResult Create()
        {
            return PartialView("CreateUser");
        }
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {

            if (!ModelState.IsValid) return PartialView("CreateUser", UsersVm);

            ApplicationUsers users = new ApplicationUsers
            {
                UserName = UsersVm.Username,
                Name = UsersVm.Name,
                DateOfBirth = UsersVm.DateOfBirth,
                Email = UsersVm.Email,
                Gender = UsersVm.Gender,
                Contact = UsersVm.Contact,
                Country = UsersVm.Country,
                Region = UsersVm.Region,
                City = UsersVm.City,
            };
            await _userManager.CreateAsync(users,UsersVm.Password);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (String.IsNullOrEmpty(id)) return NotFound();
            var userFromDb = await _userManager.FindByIdAsync(id);
            return PartialView("EditUser", userFromDb);
        }
        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id,ApplicationUsers user)
        {
            if (id != user.Id) return PartialView("EditUser", user);

            var userFromDb = await _userManager.FindByIdAsync(id);
            if (userFromDb == null) return NotFound("No user");
            userFromDb.Name = user.Name;
            userFromDb.Gender = user.Gender;
            userFromDb.Contact = user.Contact;
            userFromDb.DateOfBirth = user.DateOfBirth;
            userFromDb.Region = user.Region;
            userFromDb.Country = user.Country;
            userFromDb.City = user.City;
            userFromDb.Address = user.Address;

            await _userManager.UpdateAsync(userFromDb);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (String.IsNullOrEmpty(id)) return NotFound();
            ApplicationUsers role = await _userManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = _userManager.DeleteAsync(role).Result;
            }
            return RedirectToAction("Index");
        }
    }
}