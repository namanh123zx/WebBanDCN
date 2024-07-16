using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models;

namespace WebBH_Shopping.Controllers
{
	public class BrandController :Controller
	{
		private readonly WebBH_ShoppingContext _context;

		public BrandController(WebBH_ShoppingContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = _context.BrandModel.Where(c => c.Slug == Slug).FirstOrDefault();
			if (brand == null) return RedirectToAction("Index");
			var productsByBrand = _context.ProductModel.Where(c => c.BrandId == brand.Id);
			return View(await productsByBrand.OrderByDescending(p => p.Id).ToArrayAsync());

		}
	}
}
