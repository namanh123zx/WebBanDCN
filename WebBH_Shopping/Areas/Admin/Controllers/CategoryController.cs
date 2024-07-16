using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models;

namespace WebBH_Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize]
	public class CategoryController : Controller
	{
		private readonly WebBH_ShoppingContext _context;
		public CategoryController(WebBH_ShoppingContext context)
		{
			_context = context;

		}

		public async Task<IActionResult> Index()
		{
		return View(await _context.CategoryModel.OrderByDescending(p=>p.Id).ToListAsync());

		}
        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _context.CategoryModel.FindAsync(Id);

            return View(category);

        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        { 

            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _context.CategoryModel.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(category);
                }
               
                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model co mot vai thu dang bi loi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);

            }
            return View(category);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
        {

            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _context.CategoryModel.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(category);
                }

                _context.Update(category);
                await _context.SaveChangesAsync();
                TempData["success"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model co mot vai thu dang bi loi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);

            }
            return View(category);

        }


        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await _context.CategoryModel.FindAsync(Id);
           
            _context.CategoryModel.Remove(category);
            await _context.SaveChangesAsync();
            TempData["success"] = "Danh mục đã xoá";
            return RedirectToAction("Index");
        }
    }
}
