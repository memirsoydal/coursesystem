using System.Security.Claims;
using CourseSystem.Data;
using CourseSystem.Models;
using CourseSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.Controllers;
[Authorize(Roles = "Admin, Teacher")]

public class TeacherCourses : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ApplicationDbContext _context;
    
    public TeacherCourses(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
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
        
        var courses = _context.TeacherCourses
            .Where(tt => tt.TeacherId == userId)
            .Join(_context.CourseGrades,
                tt => tt.CourseGradeId,
                cs => cs.Id,
                (tt, cs) => new { CourseSinif = cs, TeacherTeaches = tt })
            .Join(_context.Courses,
                cs => cs.CourseSinif.CourseId,
                c => c.Id,
                (cs, c) => new { CourseSinif = cs.CourseSinif, Course = c })
            .Join(_context.Grades,
                cc => cc.CourseSinif.GradeId,
                s => s.Id,
                (cc, s) => new { CourseSinif = cc.CourseSinif, Course = cc.Course, Sinif = s })
            .Select(ccs => new CourseViewModel {
                Course = ccs.Course,
                GradeName = ccs.Sinif.Name
            })
            .ToList();

        return View("Dashboard",courses);
    }
    
    public IActionResult Lectures(Guid id)
    {
        var lectures = _context.CourseContents.Where(c => c.CourseId == id)
            .Join(_context.Contents, cc => cc.ContentId,
                ct => ct.Id, (cc, ct) => ct).ToList();
        return View("Lectures",lectures);
    }
}