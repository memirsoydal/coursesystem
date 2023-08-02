using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.Controllers;

[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    //List all the roles created by users.
    public IActionResult Index()
    {
        var roles = _roleManager.Roles;
        return View(roles);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(IdentityRole model)
    {
        if(!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
        {
            await _roleManager.CreateAsync(new IdentityRole(model.Name));
        }

        return RedirectToAction("Index");
    }

    // GET: /Role/Delete/{id}
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    // POST: /Role/Delete/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Index));
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(role);
    }
}