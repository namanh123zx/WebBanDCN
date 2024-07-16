using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models;

namespace WebBH_Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class AppRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
		
		
        public AppRolesController(RoleManager<IdentityRole> roleManager)
        { 
			
			_roleManager = roleManager;
        }
       
        public IActionResult Index()
		{
			var roles = _roleManager.Roles;
			return View(roles);
		}

		
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(IdentityRole model)
		{
			if (await _roleManager.RoleExistsAsync(model.Name))
			{
				ModelState.AddModelError("", "Role already exists.");
				return View(model);
			}

			var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
			if (result.Succeeded)
			{
				TempData["success"] = "Role created successfully";
				return RedirectToAction("Index");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return NotFound();
			}

			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				return NotFound();
			}

			return View(role);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, IdentityRole model)
		{
			if (string.IsNullOrEmpty(id))
			{
				return NotFound();
			}

			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				role.Name = model.Name;
				var result = await _roleManager.UpdateAsync(role);
				if (result.Succeeded)
				{
					TempData["success"] = "Role updated successfully";
					return RedirectToAction("Index");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

			return View(role);
		}

		[HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return NotFound();
			}

			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				return NotFound();
			}

			var result = await _roleManager.DeleteAsync(role);
			if (result.Succeeded)
			{
				TempData["success"] = "Role deleted successfully";
                return RedirectToAction("Index");
            }

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(role);
		}

	}
}
