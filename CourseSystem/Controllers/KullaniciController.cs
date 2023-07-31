using CourseSystem.Data;
using CourseSystem.Models;
using CourseSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseSystem.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<Kullanici> _userManager;
    private readonly IUserStore<Kullanici> _userStore;
    private readonly IUserEmailStore<Kullanici> _emailStore;

    public UserController(
        UserManager<Kullanici> userManager,
        IUserStore<Kullanici> userStore,
        RoleManager<IdentityRole> roleManager, 
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _roleManager = roleManager;
        _context = context;
    }

    // list all users
    public IActionResult Index()
    {
        var users = _userManager.Users;
        return View(users);
    }
    public IActionResult CreateAdmin()
    {
        return View();
    }
    public IActionResult CreateTeacher()
    {
        return View();
    }
    public IActionResult CreateStudent()
    {
        ViewData["SinifId"] = new SelectList(_context.Sinifs, "Id", "Name");
        return View();
    }

    // POST: Courses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAdmin([Bind("FirstName,LastName, Username, Email, Password, SinifId")] KullaniciViewModel kullaniciViewModel)
    {
        if (ModelState.IsValid)
        {
            await CreateTempUser(kullaniciViewModel, "Admin");
            return RedirectToAction(nameof(Index));
        }
        return View(kullaniciViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTeacher([Bind("FirstName,LastName, Username, Email, Password, SinifId")] KullaniciViewModel kullaniciViewModel)
    {
        if (ModelState.IsValid)
        {
            await CreateTempUser(kullaniciViewModel, "Teacher");
            return RedirectToAction(nameof(Index));
        }
        return View(kullaniciViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStudent([Bind("FirstName,LastName, Username, Email, Password, SinifId")] KullaniciViewModel kullaniciViewModel)
    {
        if (ModelState.IsValid)
        {
            await CreateTempUser(kullaniciViewModel, "Student");
            return RedirectToAction(nameof(Index));
        }
        return View(kullaniciViewModel);
    }

    private async Task CreateTempUser(KullaniciViewModel kullaniciViewModel, string role)
    {
        var user = CreateUser();

        await _userStore.SetUserNameAsync(user, kullaniciViewModel.Username, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, kullaniciViewModel.Email, CancellationToken.None);

        user.FirstName = kullaniciViewModel.FirstName;
        user.LastName = kullaniciViewModel.LastName;
        user.FullName = $"{kullaniciViewModel.FirstName} {kullaniciViewModel.LastName}";
        user.SinifId = kullaniciViewModel.SinifId;

        await _userManager.CreateAsync(user, kullaniciViewModel.Password);
        await _userManager.AddToRoleAsync(user, role);
    }

    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null || _context.Users == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var userViewModel = new KullaniciViewModel();
        userViewModel.FirstName = user.FirstName;
        userViewModel.LastName = user.LastName;
        userViewModel.Username = user.UserName;
        userViewModel.Email = user.Email;
        userViewModel.Password = string.Empty;
        if(await _userManager.IsInRoleAsync(user, "Student"))
        {
            ViewData["SinifId"] = new SelectList(_context.Sinifs, "Id", "Name", user.SinifId);
        }
        return View(userViewModel);
    }

    // POST: Courses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("FirstName, LastName, Username, Email, Password, SinifId")] KullaniciViewModel kullaniciViewModel)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (!ModelState.IsValid) return View(kullaniciViewModel);
        user.FirstName = kullaniciViewModel.FirstName;
        user.LastName = kullaniciViewModel.LastName;
        user.FullName = kullaniciViewModel.FirstName + " " + kullaniciViewModel.LastName;
        user.UserName = kullaniciViewModel.Username;
        user.Email = kullaniciViewModel.Email;
        user.SinifId = kullaniciViewModel.SinifId;
        if (kullaniciViewModel.Password != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, kullaniciViewModel.Password);
        }
        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(Index));
    }
    // GET: Courses/Delete/5
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null || _context.Users == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
    // POST: Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirm(string id)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        return RedirectToAction(nameof(Index));
    }
    private Kullanici CreateUser()
    {
        try
        {
            return Activator.CreateInstance<Kullanici>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(Kullanici)}'. " +
                                                $"Ensure that '{nameof(Kullanici)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
    private IUserEmailStore<Kullanici> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<Kullanici>)_userStore;
    }
}