using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CarShop.Models;
using CarShop.Models.ViewModels;
using CarShop.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class LoginRegisterController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<ApplicationRoles> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUsers> _signInManager;

        [BindProperty] public LoginViewModel LoginVm { get; set; }

        public LoginRegisterController(UserManager<ApplicationUsers> userManager,
            RoleManager<ApplicationRoles> roleManager,
            IUnitOfWork unitOfWork,
            SignInManager<ApplicationUsers> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl=null)
        {
            return View();
        }
        [HttpPost,ActionName("Login")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> LoginPost(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (LoginVm.Email.IndexOf('@') > -1)
            {
                //Validate email format
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                string phoneRegex = @"(09|03[0-9])+([0-9]{8})\b";
                Regex EmailRe = new Regex(emailRegex);
                Regex PhoneRe = new Regex(phoneRegex);
                if (!EmailRe.IsMatch(LoginVm.Email))
                {
                    ModelState.AddModelError("Email", "Email is not valid");
                }
            }
            else
            {
                //validate Username format
                string usernameRegex = @"^[a-zA-Z0-9]*$";
                Regex re = new Regex(usernameRegex);
                if (!re.IsMatch(LoginVm.Email))
                {
                    ModelState.AddModelError("Email", "Username is not valid");
                }
            }

            if (ModelState.IsValid)
            {
                var username = LoginVm.Email;
                if (username.IndexOf('@') > -1)
                {
                    var user = await _userManager.FindByEmailAsync(LoginVm.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(LoginVm);
                    }

                    username = user.UserName;
                }

                var result = await _signInManager.PasswordSignInAsync(username, LoginVm.Password, LoginVm.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    ApplicationUsers user =
                        _userManager.Users.FirstOrDefault(x => x.UserName == username || x.Email == username);

                    if (user != null)
                    {
                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            if (returnUrl == null)
                            {
                                return RedirectToAction("Index", "HomeAdmin", new {area = "Admin"});
                            }

                            return LocalRedirect(returnUrl);
                        }
                        if (await _userManager.IsInRoleAsync(user, "User"))
                        {
                            if (returnUrl == null)
                            {
                                return RedirectToAction("Index", "Home",new {area = "Customer"});
                            }
                            return LocalRedirect(returnUrl);
                        }
                    }
                }
            }

            return View(LoginVm);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost, ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UsersViewModel usersViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Something failed");
                return View();
            }

            ApplicationUsers applicationUsers = new ApplicationUsers
            {
                Name = usersViewModel.Name,
                UserName = usersViewModel.Username,
                Email = usersViewModel.Email,
                Address = usersViewModel.Address,
                Country = usersViewModel.Country,
                DateOfBirth = usersViewModel.DateOfBirth,
                City = usersViewModel.City,
                Region = usersViewModel.Region,
                Gender = usersViewModel.Gender,
                Contact = usersViewModel.Contact,
                EmailConfirmed = false
            };
            IdentityResult result = await _userManager.CreateAsync(applicationUsers, usersViewModel.Password);
            if (result.Succeeded)
            {
                ApplicationRoles applicationRoles = await _roleManager.FindByNameAsync("User");
                if (applicationRoles != null)
                {
                    IdentityResult roleResult =
                        await _userManager.AddToRoleAsync(applicationUsers, applicationRoles.Name);
                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(usersViewModel);
        }



        public async Task<IActionResult> Logout(string returnUrl=null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index","Home",new {area = "Customer"});
            }
        }
    }
}