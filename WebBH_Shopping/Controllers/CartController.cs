﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models;
using WebBH_Shopping.Models.ViewModels;

namespace WebBH_Shopping.Controllers
{
	public class CartController : Controller
	{
		private readonly WebBH_ShoppingContext _context;

		public CartController(WebBH_ShoppingContext context)
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
		public IActionResult Checkout()
		{
			return View("~/Views/Checkout/Index.cshtml");
		}
		public async Task<IActionResult> Add(int Id)
		{
			ProductModel product = await _context.ProductModel.FindAsync(Id);
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItems = cart.Where(c=>c.ProductId == Id).FirstOrDefault();
			if(cartItems == null)
			{
				cart.Add(new CartItemModel(product));
			}
			else
			{
				cartItems.Quantity += 1;
			}
            HttpContext.Session.SetJson("Cart", cart);

			TempData["success"] = "Add Item to cart Successfully";
            return Redirect(Request.Headers["Referer"].ToString());

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
		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("Cart");
            TempData["success"] = "Clear all Item of cart Successfully";
            return RedirectToAction("Index");
		}
		[Authorize]
		public IActionResult PaymentCallBack()
		{
			return View();
		}
	}
}