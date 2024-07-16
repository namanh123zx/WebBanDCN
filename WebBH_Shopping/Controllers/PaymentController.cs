using Microsoft.AspNetCore.Mvc;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models.ViewModels;
using WebBH_Shopping.Models;
using System.Security.Claims;



namespace WebBH_Shopping.Controllers
{
	public class PaymentController : Controller
	{
		private readonly WebBH_ShoppingContext _context;

		public PaymentController(WebBH_ShoppingContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemViewModel cartVM = new()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);

		}
		public async Task<IActionResult> Decrease(int Id)
		{

			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if( cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p=>p.ProductId == Id);

			}
			if(cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");

			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);

			}
            TempData["success"] = "Decrease Item quantity to cart Successfully";
            return RedirectToAction("Index");

		}
		public async Task<IActionResult> Increase(int Id)
		{

			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantity >= 1)
			{
				++cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);

			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");

			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);

			}
            TempData["success"] = "Increase Item quantity to cart Successfully";
            return RedirectToAction("Index");

		}
		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			cart.RemoveAll(p=>p.ProductId == Id);
			if(cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);

			}
            TempData["success"] = "Remove Item of cart Successfully";
            return RedirectToAction("Index");
		}
		
		
	}
}
