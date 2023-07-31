using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.Controllers;

[Authorize(Roles = "Admin")]
public class RolController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolController(RoleManager<IdentityRole> roleManager)
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

}