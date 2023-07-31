using System.Security.Claims;
using CourseSystem.Data;
using CourseSystem.Models;
using CourseSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.Controllers;
[Authorize(Roles = "Admin, Student")]

public class OgrenciDers : Controller
{
    private readonly UserManager<Kullanici> _userManager;
    private readonly ApplicationDbContext _context;

    public OgrenciDers(UserManager<Kullanici> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new ArgumentNullException(nameof(user));
        var isStudent = await _userManager.IsInRoleAsync(user, "Student");
        if (!isStudent)
        {
            return NotFound();
        }

        var courses = _context.CourseSinif
            .Where(cs => cs.SinifId == user.SinifId)
            .Join(_context.Courses,
                cs => cs.CourseId,
                c => c.Id,
                (cs, c) => new { CourseSinif = cs, Course = c })
            .Join(_context.Sinifs,
                ccs => ccs.CourseSinif.SinifId,
                s => s.Id,
                (ccs, s) => new { CourseSinif = ccs.CourseSinif, Course = ccs.Course, Sinif = s })
            .Select(ccs => new DersViewModel() {
                Ders = ccs.Course,
                SinifName = ccs.Sinif.Name
            })
            .ToList();
        
        // TODO Ogrenci sinifId ekleme, sinifa gore dersleri getirme.
        return View("Dashboard",courses);
    }

    
    // GET
    public IActionResult Lectures(Guid id)
    {
        var homeworks = _context.CourseContents.Where(c => c.CourseId == id)
            .Join(_context.Contents, cc => cc.ContentId,
                ct => ct.Id, (cc, ct) => ct).Where(ct => ct.Is_Homework == true).ToList();
        return View(homeworks);
    }
}