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

namespace CourseSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseGradeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseGradeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseGrades
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CourseGrades.Include(c => c.Course).Include(c => c.Grade);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CourseGrades/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.CourseGrades == null)
            {
                return NotFound();
            }

            var courseGrade = await _context.CourseGrades
                .Include(c => c.Course)
                .Include(c => c.Grade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseGrade == null)
            {
                return NotFound();
            }

            return View(courseGrade);
        }

        // GET: CourseGrades/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "Name");
            return View();
        }

        // POST: CourseGrades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,GradeId")] CourseGrade courseGrade)
        {
            if (ModelState.IsValid)
            {
                if (!CourseSinifExists(courseGrade.CourseId, courseGrade.GradeId))
                {
                    courseGrade.Id = Guid.NewGuid();
                    var courseName = await _context.Courses.FindAsync(courseGrade.CourseId);
                    var gradeName = await _context.Grades.FindAsync(courseGrade.GradeId);
                    courseGrade.CourseGradeName = courseName.Name + " | " + gradeName.Name;
                    _context.Add(courseGrade);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "A duplicate entry already exists.");
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseGrade.CourseId);
            ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "Name", courseGrade.GradeId);
            return View(courseGrade);
        }

        // GET: CourseGrades/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.CourseGrades == null)
            {
                return NotFound();
            }

            var courseGrade = await _context.CourseGrades.FindAsync(id);
            if (courseGrade == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseGrade.CourseId);
            ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "Name", courseGrade.GradeId);
            return View(courseGrade);
        }

        // POST: CourseGrades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CourseId,GradeId")] CourseGrade courseGrade)
        {
            if (id != courseGrade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!CourseSinifExists(courseGrade.CourseId, courseGrade.GradeId))
                {
                    try
                    {
                        var courseName = _context.Courses.FindAsync(courseGrade.CourseId);
                        var gradeName = _context.Grades.FindAsync(courseGrade.GradeId);
                        courseGrade.CourseGradeName = courseName + " " + gradeName;
                        _context.Add(courseGrade);
                        _context.Update(courseGrade);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CourseSinifExists(courseGrade.CourseId, courseGrade.GradeId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseGrade.CourseId);
            ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "Name", courseGrade.GradeId);
            return View(courseGrade);
        }

        // GET: CourseGrades/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.CourseGrades == null)
            {
                return NotFound();
            }

            var courseGrade = await _context.CourseGrades
                .Include(c => c.Course)
                .Include(c => c.Grade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseGrade == null)
            {
                return NotFound();
            }

            return View(courseGrade);
        }

        // POST: CourseGrades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.CourseGrades == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CourseGrade'  is null.");
            }
            var courseGrade = await _context.CourseGrades.FindAsync(id);
            if (courseGrade != null)
            {
                _context.CourseGrades.Remove(courseGrade);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseSinifExists(Guid cid, Guid sid)
        {
          return (_context.CourseGrades?.Any(e => e.CourseId == cid && e.GradeId == sid)).GetValueOrDefault();
        }
    }
}
