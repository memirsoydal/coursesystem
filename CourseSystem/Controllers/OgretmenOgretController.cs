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
    public class OgretmenOgretController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Kullanici> _userManager;

        public OgretmenOgretController(ApplicationDbContext context, UserManager<Kullanici> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: OgretmenOgret
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TeacherTeaches.Include(t => t.CourseSinif).Include(t => t.Teacher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OgretmenOgret/Details/5
        public async Task<IActionResult> Details(string? teacherId, Guid? courseSinifId)
        {
            if (teacherId == null || courseSinifId == null  || _context.TeacherTeaches == null)
            {
                return NotFound();
            }

            var teacherTeach = await _context.TeacherTeaches.FindAsync(teacherId, courseSinifId);
            if (teacherTeach == null)
            {
                return NotFound();
            }

            return View(teacherTeach);
        }

        // GET: OgretmenOgret/Create
        public IActionResult Create()
        {
            ViewData["CourseSinifId"] = new SelectList(_context.CourseSinif, "Id", "Id");
            ViewData["TeacherId"] = new SelectList( _userManager.GetUsersInRoleAsync("Teacher").Result, "Id", "FullName");
            return View();
        }

        // POST: OgretmenOgret/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,CourseSinifId")] OgretmenOgret ogretmenOgret)
        {
            if (ModelState.IsValid)
            {
                if (!TeacherTeachExists(ogretmenOgret.TeacherId, ogretmenOgret.CourseSinifId))
                {
                    _context.Add(ogretmenOgret);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "A duplicate entry already exists.");
            }
            ViewData["CourseSinifId"] = new SelectList(_context.CourseSinif, "Id", "Id", ogretmenOgret.CourseSinifId);
            ViewData["TeacherId"] = new SelectList(_context.Users, "Id", "FullName", ogretmenOgret.TeacherId);
            return View(ogretmenOgret);
        }

        // GET: OgretmenOgret/Edit/5
        public async Task<IActionResult> Edit(string teacherId, Guid courseSinifId)
        {
            if (teacherId == null || _context.TeacherTeaches == null)
            {
                return NotFound();
            }

            var teacherTeach = await _context.TeacherTeaches.FindAsync(teacherId, courseSinifId);
            if (teacherTeach == null)
            {
                return NotFound();
            }
            ViewData["CourseSinifId"] = new SelectList(_context.CourseSinif, "Id", "Id", teacherTeach.CourseSinifId);
            ViewData["TeacherId"] = new SelectList(_context.Users, "Id", "FullName", teacherTeach.TeacherId);
            return View(teacherTeach);
        }

        // POST: OgretmenOgret/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string teacherId,Guid courseSinifId, [Bind("TeacherId,CourseSinifId")] OgretmenOgret ogretmenOgret)
        {
            if (teacherId != ogretmenOgret.TeacherId || courseSinifId != ogretmenOgret.CourseSinifId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!TeacherTeachExists(ogretmenOgret.TeacherId, ogretmenOgret.CourseSinifId))
                {
                    try
                    {
                        _context.Update(ogretmenOgret);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TeacherTeachExists(ogretmenOgret.TeacherId, ogretmenOgret.CourseSinifId))
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
            ViewData["CourseSinifId"] = new SelectList(_context.CourseSinif, "Id", "Id", ogretmenOgret.CourseSinifId);
            ViewData["TeacherId"] = new SelectList(_context.Users, "Id", "FullName", ogretmenOgret.TeacherId);
            return View(ogretmenOgret);
        }

        // GET: OgretmenOgret/Delete/5
        public async Task<IActionResult> Delete(string teacherId, Guid courseSinifId)
        {
            if (teacherId == null || _context.TeacherTeaches == null)
            {
                return NotFound();
            }

            var teacherTeach = await _context.TeacherTeaches.FindAsync(teacherId, courseSinifId);
            if (teacherTeach == null)
            {
                return NotFound();
            }

            return View(teacherTeach);
        }

        // POST: OgretmenOgret/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string teacherId, Guid courseSinifId)
        {
            if (_context.TeacherTeaches == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TeacherTeaches'  is null.");
            }
            var teacherTeach = await _context.TeacherTeaches.FindAsync(teacherId, courseSinifId);
            if (teacherTeach != null)
            {
                _context.TeacherTeaches.Remove(teacherTeach);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherTeachExists(string teacherId, Guid courseSinifId)
        {
          return (_context.TeacherTeaches?.Any(e => e.TeacherId == teacherId && e.CourseSinifId == courseSinifId)).GetValueOrDefault();
        }
    }
}
