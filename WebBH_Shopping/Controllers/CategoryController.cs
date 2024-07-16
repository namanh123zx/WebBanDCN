using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models;

namespace WebBH_Shopping.Controllers
{
    public class CategoryController : Controller
    {
        private readonly WebBH_ShoppingContext _context;

        public CategoryController(WebBH_ShoppingContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string Slug = "")
        {
            CategoryModel category = _context.CategoryModel.Where(c => c.Slug == Slug).FirstOrDefault();
            if (category == null) return RedirectToAction("Index");
            var productsByCategory = _context.ProductModel.Where(c => c.CategoryId == category.Id);
            return View(await productsByCategory.OrderByDescending(p => p.Id).ToArrayAsync());

        }
    }
}
