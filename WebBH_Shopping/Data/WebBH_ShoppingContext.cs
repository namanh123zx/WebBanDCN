using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBH_Shopping.Models;

namespace WebBH_Shopping.Data
{
    public class WebBH_ShoppingContext : IdentityDbContext<AppUserModel>
    {
        public WebBH_ShoppingContext (DbContextOptions<WebBH_ShoppingContext> options)
            : base(options)
        {
        }

        public DbSet<WebBH_Shopping.Models.CategoryModel> CategoryModel { get; set; } = default!;
		public DbSet<WebBH_Shopping.Models.BrandModel> BrandModel { get; set; }
		public DbSet<WebBH_Shopping.Models.ProductModel> ProductModel { get; set; }
		public DbSet<WebBH_Shopping.Models.OrderDetails> OrderDetails { get; set; }
		
        public DbSet<WebBH_Shopping.Models.OrderModel> Orders { get; set; }
        
       
        public DbSet<WebBH_Shopping.Models.UserModel> UserModel { get; set; }
        



    }
}
