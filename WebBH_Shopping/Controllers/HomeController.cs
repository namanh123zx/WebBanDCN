using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models;
using X.PagedList;

namespace WebBH_Shopping.Controllers
{
	public class HomeController : Controller
	{
		private readonly WebBH_ShoppingContext _dataContext;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, WebBH_ShoppingContext context)
		{
			_logger = logger;
			_dataContext = context;
		}

		public IActionResult Index(int? page, string name )
		{
			var products = _dataContext.ProductModel.Include("Category").Include("Brand").ToPagedList(page ?? 1, 6, 20);
			return View(products);
		}
		public IActionResult Timkiem(int? page, string name)
		{
			if (Request.Method != "GET")
			{
				page = 1;
			}
			int pageSize = 6;
			int pageNumber = (page ?? 1);
			int TotalPages = 20;
			var products = _dataContext.ProductModel.Where(n => n.Name.Contains(name));
			ViewBag.name = name;
			return View(products.OrderBy(n => n.Name).ToPagedList(pageNumber, pageSize, TotalPages));
		}


		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		

	}
}