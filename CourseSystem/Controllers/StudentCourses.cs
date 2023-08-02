using System.Security.Claims;
using CourseSystem.Data;
using CourseSystem.Models;
using CourseSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.Controllers;
[Authorize(Roles = "Admin, Student")]

public class StudentCourses : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public StudentCourses(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
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

        var courses = _context.CourseGrades
            .Where(cs => cs.GradeId == user.GradeId)
            .Join(_context.Courses,
                cs => cs.CourseId,
                c => c.Id,
                (cs, c) => new { CourseSinif = cs, Course = c })
            .Join(_context.Grades,
                ccs => ccs.CourseSinif.GradeId,
                s => s.Id,
                (ccs, s) => new { CourseSinif = ccs.CourseSinif, Course = ccs.Course, Sinif = s })
            .Select(ccs => new CourseViewModel() {
                Course = ccs.Course,
                GradeName = ccs.Sinif.Name
            })
            .ToList();
        
        return View("Dashboard",courses);
    }

    
    // GET
    public IActionResult Lectures(Guid id)
    {
        var homeworks = _context.CourseContents.Where(c => c.CourseId == id)
            .Join(_context.Contents, cc => cc.ContentId,
                ct => ct.Id, (cc, ct) => ct).Where(ct => ct.IsHomework == true).ToList();
        return View("Lectures", homeworks);
    }
}