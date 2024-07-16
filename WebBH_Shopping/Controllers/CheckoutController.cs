using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models;

namespace WebBH_Shopping.Controllers
{
	public class CheckoutController :Controller
	{
		private readonly WebBH_ShoppingContext _context;

		public CheckoutController(WebBH_ShoppingContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail== null)
			{

				return RedirectToAction("Login", "Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.OrderCode = ordercode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreateDate = DateTime.Now;
				_context.Add(orderItem);
				_context.SaveChanges();
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach(var cart in cartItems)
				{
					var orderdetails = new OrderDetails();
					orderdetails.UserName = userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.ProductId = (int)cart.ProductId;
					orderdetails.Price = cart.Price;
					orderdetails.Quantity = cart.Quantity;
					_context.Add(orderdetails);
					_context.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");
				TempData["success"] = "Thanh Toán Thành Công, vui lòng chờ đơn hàng";
				return RedirectToAction("Index", "Cart");
			}
			return View();
		}
	}
}
