using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBH_Shopping.Models;

namespace WebBH_Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize]
	public class UsersController : Controller
	{

		private readonly UserManager<AppUserModel> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public UsersController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync();
			var userList = new List<UserViewModel>();
			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				userList.Add(new UserViewModel
				{
					Id = user.Id,
                    UserName= user.UserName,
                    Email = user.Email,
					Roles = string.Join(", ", roles)
				});
			}
			return View(userList);
		}

		// GET: /Users/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: /Users/Create
		[HttpPost]

		public async Task<IActionResult> Create(UserModel user)
		{
			if (ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel { UserName = user.UserName, Email = user.Email };
				IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
				if (result.Succeeded)
				{
					TempData["success"] = "Tạo user thành công";
                    return RedirectToAction("Index");
                }
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);

				}

			}
			return View(user);

		}
        // GET: /Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUserModel user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            EditModel userModel = new EditModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditModel user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel appUser = await _userManager.FindByIdAsync(user.Id);
                if (appUser == null)
                {
                    return NotFound();
                }

                appUser.UserName = user.UserName;
                appUser.Email = user.Email;

                IdentityResult result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    TempData["success"] = "Cập nhật user thành công";
                    return RedirectToAction("Index");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(user);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUserModel user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            EditModel userModel = new EditModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(userModel);
        }

        // GET: /Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: /Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

    }
}