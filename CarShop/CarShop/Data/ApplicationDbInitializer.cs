using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
using Microsoft.AspNetCore.Identity;

namespace CarShop.Data
{
    public class ApplicationDbInitializer
    {
        const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
        // any guid, but nothing is against to use the same one
        const string ROLE_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e576";
        public static void SeedUsers(UserManager<ApplicationUsers> userManager,RoleManager<ApplicationRoles> roleManager)
        {
            if (roleManager.FindByNameAsync("Admin").Result == null)
            {
                ApplicationRoles role = new ApplicationRoles
                {
                    Id = ROLE_ID,
                    Name = "Admin",
                    NormalizedName = "Admin".ToLower(),
                    CreatedDate = DateTime.Today,
                    Description = "Admin",
                };
                IdentityResult result = roleManager.CreateAsync(role).Result;
                if (result.Succeeded)
                {
                    Console.WriteLine("Created");
                }
            }
            if (userManager.FindByEmailAsync("admin@admin.com").Result == null)
            {
                ApplicationUsers user = new ApplicationUsers
                {
                    Id = ADMIN_ID,
                    UserName = "admin",
                    NormalizedUserName = "admin",
                    Email = "admin@admin.com",
                    Name = "Admin",
                    Contact = "0989751783",
                    DateOfBirth = new DateTime(1998, 3, 19),
                    Gender = "Male",
                    Country = "Viet Nam",
                    Region = "TP Ho Chi Minh",
                    City = "Thu Duc",
                    Address = "KTX khu A, Khu pho 6",
                    NormalizedEmail = "admin@admin.com",
                    EmailConfirmed = true,
                    SecurityStamp = string.Empty
                };

                IdentityResult result = userManager.CreateAsync(user, "Admin123!").Result;

                if (result.Succeeded)
                {
                    IdentityUserRole<string> userRole = new IdentityUserRole<string>
                    {
                        RoleId = ROLE_ID,
                        UserId = ADMIN_ID
                    };
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
