using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using WebBH_Shopping.Data;
using WebBH_Shopping.Models;

namespace WebBH_Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/User")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

        }
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.OrderByDescending(p => p.Id).ToListAsync());
        }
        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(new AppUserModel());

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(AppUserModel user)
        {
            if (ModelState.IsValid)
            {
                var createUserResult = await _userManager.CreateAsync(user, user.PasswordHash);
                if (createUserResult.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(user);
                }
            }
            else
            {
                TempData["error"] = "dang bi loi";
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
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id, AppUserModel user)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.RoleId = user.RoleId;


                var updateUserResult = await _userManager.UpdateAsync(existingUser);
                if (updateUserResult.Succeeded)
                {
                    TempData["success"] = "User updated successfully";
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    AddIdentityErrors(updateUserResult);
                    return View(existingUser);
                }

            }
                // Load roles for the view
                var roles = await _roleManager.Roles.ToListAsync();
                ViewBag.Roles = new SelectList(roles, "Id", "Name");

                // Display model errors
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                string errorMessage = string.Join("\n", errors);
                TempData["error"] = errorMessage;
            
            return View(existingUser);
        }

        private void AddIdentityErrors(IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();

            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                return View("Error");

            }
            TempData["success"] = "User đã xoá thành công";
            return RedirectToAction("Index");
        }

    }
}