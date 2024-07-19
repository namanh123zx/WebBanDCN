using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebBH_Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly WebBH_ShoppingContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(WebBH_ShoppingContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5)
        {
            var products = _context.ProductModel
                .OrderByDescending(p => p.Id)
                .Include(p => p.Category)
                .Include(p => p.Brand);

            var totalCount = await products.CountAsync();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var paginatedProducts = await products
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.PageCount = pageCount;
            ViewBag.TotalCount = totalCount;

            return View(paginatedProducts);
        }
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.CategoryModel, "Id", "Name");

            ViewBag.Brands = new SelectList(_context.BrandModel, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_context.CategoryModel, "Id", "Name", product.CategoryId);

            ViewBag.Brands = new SelectList(_context.BrandModel, "Id", "Name", product.BrandId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await _context.ProductModel.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(product);
                }
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công";
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

        }
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _context.ProductModel.FindAsync(Id);
            ViewBag.Categories = new SelectList(_context.CategoryModel, "Id", "Name", product.CategoryId);

            ViewBag.Brands = new SelectList(_context.BrandModel, "Id", "Name", product.BrandId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, ProductModel product)
        {
            ViewBag.Categories = new SelectList(_context.CategoryModel, "Id", "Name", product.CategoryId);

            ViewBag.Brands = new SelectList(_context.BrandModel, "Id", "Name", product.BrandId);
            var existed_product = _context.ProductModel.Find(product.Id);
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");

                if (product.ImageUpload != null)
                {

                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    string oldfileImage = Path.Combine(uploadsDir, existed_product.Image);
                    try
                    {
                        if (System.IO.File.Exists(oldfileImage))
                        {
                            System.IO.File.Delete(oldfileImage);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while deleting the product image");

                    }

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_product.Image = imageName;

                }
                existed_product.Name = product.Name;
                existed_product.Description = product.Description;
                existed_product.Price = product.Price;
                existed_product.CategoryId = product.CategoryId;
                existed_product.BrandId = product.BrandId;

                _context.Update(existed_product);
                await _context.SaveChangesAsync();
                TempData["success"] = "Cập nhật sản phẩm thành công";
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

        }
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _context.ProductModel.FindAsync(Id);
            if (!string.Equals(product.Image, "noname.jpg"))

            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string oldfileImage = Path.Combine(uploadsDir, product.Image);
                try
                {
                    if (System.IO.File.Exists(oldfileImage))
                    {
                        System.IO.File.Delete(oldfileImage);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while deleting the product image");

                }
            }
            _context.ProductModel.Remove(product);
            await _context.SaveChangesAsync();
            TempData["error"] = "Sản phẩm đã xoá";
            return RedirectToAction("Index");
        }

    }
}
