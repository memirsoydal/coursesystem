using System.Security.Claims;
using CourseSystem.Data;
using CourseSystem.Models;
using CourseSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.Controllers;

public class DashboardController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ApplicationDbContext _context;
    
    public DashboardController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    private List<CourseViewModel>? GetTeacherCourses(string userId)
    {
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
        
        return courses;
    }
    private List<CourseViewModel>? GetStudentCourses(Guid? gradeId)
    {
        if (gradeId == null)
        {
            return null;
        }
        var courses = _context.CourseGrades
            .Where(cs => cs.GradeId == gradeId)
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
        
        return courses;
    }

    private List<Content> GetTeacherLectures(Guid id)
    {
        var lectures = _context.CourseContents.Where(c => c.CourseId == id)
            .Join(_context.Contents, cc => cc.ContentId,
                ct => ct.Id, (cc, ct) => ct).ToList();
        return lectures;
    }
    private List<Content> GetStudentLectures(Guid id)
    {
        var homeworks = _context.CourseContents.Where(c => c.CourseId == id)
            .Join(_context.Contents, cc => cc.ContentId,
                ct => ct.Id, (cc, ct) => ct).Where(ct => ct.IsHomework == true).ToList();
        return homeworks;
    }

    public async Task<IActionResult> Index()
    {
        var courses = new List<CourseViewModel>();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        
        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Contains("Teacher"))
        {
            courses = GetTeacherCourses(userId);
        }

        if (userRoles.Contains("Student"))
        {
            courses = GetStudentCourses(user.GradeId);
        }
        
        return View("Dashboard",courses);
    }
    
    public async Task<IActionResult> Lectures(Guid id)
    {
        var lectures = new List<Content>();
    
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
    
        var userRoles = await _userManager.GetRolesAsync(user);

        if (userRoles.Contains("Teacher"))
        {
            lectures = GetTeacherLectures(id);
        }

        if (userRoles.Contains("Student"))
        {
            lectures = GetStudentLectures(id);
        }

        return View("Lectures", lectures);
    }

}