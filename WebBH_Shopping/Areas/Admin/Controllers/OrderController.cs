using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models;

namespace WebBH_Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {

        private readonly WebBH_ShoppingContext _context;
        public OrderController(WebBH_ShoppingContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.OrderByDescending(p => p.Id).ToListAsync());

        }
        public async Task<IActionResult> ViewOrder(string ordercode)
            
        {
            var orderDetails = await _context.OrderDetails.Include(od => od.Product).Where(od => od.OrderCode==ordercode).ToListAsync();
            return View(orderDetails);

        }
        public async Task<IActionResult> Delete(int Id)
        {
            OrderModel order = await _context.Orders.FindAsync(Id);

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            TempData["success"] = "Đơn hàng đã xoá thành công";
            return RedirectToAction("Index");
        }
    }
}