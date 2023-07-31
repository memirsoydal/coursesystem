using System.Security.Claims;
using CourseSystem.Data;
using CourseSystem.Models;
using CourseSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.Controllers;
[Authorize(Roles = "Admin, Teacher")]

public class OgretmenDers : Controller
{
    private readonly UserManager<Kullanici> _userManager;

    private readonly ApplicationDbContext _context;
    
    public OgretmenDers(UserManager<Kullanici> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        var isTeacher = await _userManager.IsInRoleAsync(user, "Teacher");
        if (!isTeacher)
        {
            return NotFound();
        }
        
        // var courses = _context.TeacherTeaches
        //     .Where(tt => tt.TeacherId == userId)
        //     .Join(_context.DersSinif,
        //         tt => tt.CourseSinifId,
        //         cs => cs.Id,
        //         (tt, cs) => cs)
        //     .Join(_context.Courses,
        //         cs => cs.CourseId,
        //         c => c.Id,
        //         (cs, c) => c)
        //     .ToList();
        var courses = _context.TeacherTeaches
            .Where(tt => tt.TeacherId == userId)
            .Join(_context.CourseSinif,
                tt => tt.CourseSinifId,
                cs => cs.Id,
                (tt, cs) => new { CourseSinif = cs, TeacherTeaches = tt })
            .Join(_context.Courses,
                cs => cs.CourseSinif.CourseId,
                c => c.Id,
                (cs, c) => new { CourseSinif = cs.CourseSinif, Course = c })
            .Join(_context.Sinifs,
                cc => cc.CourseSinif.SinifId,
                s => s.Id,
                (cc, s) => new { CourseSinif = cc.CourseSinif, Course = cc.Course, Sinif = s })
            .Select(ccs => new DersViewModel {
                Ders = ccs.Course,
                SinifName = ccs.Sinif.Name
            })
            .ToList();

        return View("Dashboard",courses);
    }
    
    public IActionResult Lectures(Guid id)
    {
        var lectures = _context.CourseContents.Where(c => c.CourseId == id)
            .Join(_context.Contents, cc => cc.ContentId,
                ct => ct.Id, (cc, ct) => ct).ToList();
        return View(lectures);
    }
}