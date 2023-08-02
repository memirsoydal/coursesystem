using CourseSystem.Models;
using CourseSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Controllers;

[Authorize(Roles = "Admin")]
public class UserRoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRoleViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRoleViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return NotFound();
            }
            ViewBag.UserName = user.UserName;

            var roles = await _roleManager.Roles.ToListAsync(); // Load roles into memory

            var model = new List<ManageUserRoleViewModel>();
            foreach (var role in roles)
            {
                var userRolesViewModel = new ManageUserRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                var hasRole = await _userManager.IsInRoleAsync(user, role.Name);
                userRolesViewModel.Selected = hasRole;
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRoleViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }