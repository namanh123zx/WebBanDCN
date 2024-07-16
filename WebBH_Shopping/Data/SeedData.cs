using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using WebBH_Shopping.Models;

namespace WebBH_Shopping.Data
{
	public class SeedData
	{
		public static void SeedingData(WebBH_ShoppingContext _context)
		{
            
			if (!_context.ProductModel.Any())
			{
				CategoryModel macbook = new CategoryModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is Large Brand in the world", Status = 1 };
				CategoryModel pc = new CategoryModel { Name = "Pc", Slug = "pc", Description = "Pc is Large Brand in the world", Status = 1 };
				BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple is Large Brand in the world", Status = 1 };
				BrandModel samsung = new BrandModel { Name = "Samsung", Slug = "samsung", Description = "Samsung is Large Brand in the world", Status = 1 };

				_context.ProductModel.AddRange(
					new ProductModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is Best", Image = "1.jpg", Category = macbook, Brand = samsung, Price = 1234 },
					new ProductModel { Name = "Pc", Slug = "pc", Description = "Pc is Best", Image = "1.jpg", Category = pc, Brand = samsung, Price = 1234 }
			);
				_context.SaveChanges();

			}

		}
	}
}
