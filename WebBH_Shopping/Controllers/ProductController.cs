using Microsoft.AspNetCore.Mvc;
using WebBH_Shopping.Data;

namespace WebBH_Shopping.Controllers
{
	public class ProductController : Controller
	{
		private readonly WebBH_ShoppingContext _context;

		public ProductController(WebBH_ShoppingContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Details(int Id)
		{
			if (Id == null) return RedirectToAction("Index");
			var productsById = _context.ProductModel.Where(p => p.Id == Id).FirstOrDefault();
			return View(productsById);
		}
	}
}
