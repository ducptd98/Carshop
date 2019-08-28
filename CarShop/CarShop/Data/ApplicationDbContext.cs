using System;
using System.Collections.Generic;
using System.Text;
using CarShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ApplicationRoles> ApplicationRoleses { get; set; }
        public DbSet<ApplicationUsers> ApplicationUserses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLike> ProductLikes { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }  

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new ProductLikeMap(builder.Entity<ProductLike>());
        }
    }
}
