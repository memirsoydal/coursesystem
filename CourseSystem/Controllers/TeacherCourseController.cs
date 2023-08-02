using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseSystem.Data;
using CourseSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CourseSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeacherCourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeacherCourseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TeacherCourse
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TeacherCourses.Include(t => t.CourseGrade).Include(t => t.Teacher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TeacherCourse/Details/5
        public async Task<IActionResult> Details(string? teacherId, Guid? courseGradeId)
        {
            if (teacherId == null || courseGradeId == null  || _context.TeacherCourses == null)
            {
                return NotFound();
            }

            var teacherTeach = await _context.TeacherCourses.FindAsync(teacherId, courseGradeId);
            if (teacherTeach == null)
            {
                return NotFound();
            }

            return View(teacherTeach);
        }

        // GET: TeacherCourse/Create
        public IActionResult Create()
        {
            ViewData["CourseGradeId"] = new SelectList(_context.CourseGrades, "Id", "CourseGradeName");
            ViewData["TeacherId"] = new SelectList( _userManager.GetUsersInRoleAsync("Teacher").Result, "Id", "FullName");
            return View();
        }

        // POST: TeacherCourse/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,CourseGradeId")] TeacherCourse teacherCourse)
        {
            if (ModelState.IsValid)
            {
                if (!TeacherTeachExists(teacherCourse.TeacherId, teacherCourse.CourseGradeId))
                {
                    _context.Add(teacherCourse);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "A duplicate entry already exists.");
            }
            ViewData["CourseGradeId"] = new SelectList(_context.CourseGrades, "Id", "Id", teacherCourse.CourseGradeId);
            ViewData["TeacherId"] = new SelectList(_context.Users, "Id", "FullName", teacherCourse.TeacherId);
            return View(teacherCourse);
        }

        // GET: TeacherCourse/Edit/5
        public async Task<IActionResult> Edit(string teacherId, Guid courseGradeId)
        {
            if (teacherId == null || _context.TeacherCourses == null)
            {
                return NotFound();
            }

            var teacherTeach = await _context.TeacherCourses.FindAsync(teacherId, courseGradeId);
            if (teacherTeach == null)
            {
                return NotFound();
            }
            ViewData["CourseGradeId"] = new SelectList(_context.CourseGrades, "Id", "CourseGradeName", teacherTeach.CourseGradeId);
            ViewData["TeacherId"] = new SelectList(_context.Users, "Id", "FullName", teacherTeach.TeacherId);
            return View(teacherTeach);
        }

        // POST: TeacherCourse/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string teacherId,Guid courseGradeId, [Bind("TeacherId,CourseGradeId")] TeacherCourse teacherCourse)
        {
            if (teacherId != teacherCourse.TeacherId || courseGradeId != teacherCourse.CourseGradeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!TeacherTeachExists(teacherCourse.TeacherId, teacherCourse.CourseGradeId))
                {
                    try
                    {
                        _context.Update(teacherCourse);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TeacherTeachExists(teacherCourse.TeacherId, teacherCourse.CourseGradeId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "A duplicate entry already exists.");
            }
            ViewData["CourseGradeId"] = new SelectList(_context.CourseGrades, "Id", "CourseGradeName", teacherCourse.CourseGradeId);
            ViewData["TeacherId"] = new SelectList(_context.Users, "Id", "FullName", teacherCourse.TeacherId);
            return View(teacherCourse);
        }

        // GET: TeacherCourse/Delete/5
        public async Task<IActionResult> Delete(string teacherId, Guid courseGradeId)
        {
            if (teacherId == null || _context.TeacherCourses == null)
            {
                return NotFound();
            }

            var teacherTeach = await _context.TeacherCourses.FindAsync(teacherId, courseGradeId);
            if (teacherTeach == null)
            {
                return NotFound();
            }

            return View(teacherTeach);
        }

        // POST: TeacherCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string teacherId, Guid courseGradeId)
        {
            if (_context.TeacherCourses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TeacherCourses'  is null.");
            }
            var teacherTeach = await _context.TeacherCourses.FindAsync(teacherId, courseGradeId);
            if (teacherTeach != null)
            {
                _context.TeacherCourses.Remove(teacherTeach);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherTeachExists(string teacherId, Guid courseGradeId)
        {
          return (_context.TeacherCourses?.Any(e => e.TeacherId == teacherId && e.CourseGradeId == courseGradeId)).GetValueOrDefault();
        }
    }
}
