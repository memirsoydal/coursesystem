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
    public class DersSinifController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DersSinifController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseSinifs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CourseSinif.Include(c => c.Course).Include(c => c.Sinif);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CourseSinifs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.CourseSinif == null)
            {
                return NotFound();
            }

            var courseSinif = await _context.CourseSinif
                .Include(c => c.Course)
                .Include(c => c.Sinif)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseSinif == null)
            {
                return NotFound();
            }

            return View(courseSinif);
        }

        // GET: CourseSinifs/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["SinifId"] = new SelectList(_context.Sinifs, "Id", "Name");
            return View();
        }

        // POST: CourseSinifs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,SinifId")] DersSinif dersSinif)
        {
            if (ModelState.IsValid)
            {
                if (!CourseSinifExists(dersSinif.CourseId, dersSinif.SinifId))
                {
                    dersSinif.Id = Guid.NewGuid();
                    _context.Add(dersSinif);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "A duplicate entry already exists.");
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", dersSinif.CourseId);
            ViewData["SinifId"] = new SelectList(_context.Sinifs, "Id", "Name", dersSinif.SinifId);
            return View(dersSinif);
        }

        // GET: CourseSinifs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.CourseSinif == null)
            {
                return NotFound();
            }

            var courseSinif = await _context.CourseSinif.FindAsync(id);
            if (courseSinif == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseSinif.CourseId);
            ViewData["SinifId"] = new SelectList(_context.Sinifs, "Id", "Name", courseSinif.SinifId);
            return View(courseSinif);
        }

        // POST: CourseSinifs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CourseId,SinifId")] DersSinif dersSinif)
        {
            if (id != dersSinif.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!CourseSinifExists(dersSinif.CourseId, dersSinif.SinifId))
                {
                    try
                {
                    _context.Update(dersSinif);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseSinifExists(dersSinif.CourseId, dersSinif.SinifId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", dersSinif.CourseId);
            ViewData["SinifId"] = new SelectList(_context.Sinifs, "Id", "Name", dersSinif.SinifId);
            return View(dersSinif);
        }

        // GET: CourseSinifs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.CourseSinif == null)
            {
                return NotFound();
            }

            var courseSinif = await _context.CourseSinif
                .Include(c => c.Course)
                .Include(c => c.Sinif)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseSinif == null)
            {
                return NotFound();
            }

            return View(courseSinif);
        }

        // POST: CourseSinifs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.CourseSinif == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DersSinif'  is null.");
            }
            var courseSinif = await _context.CourseSinif.FindAsync(id);
            if (courseSinif != null)
            {
                _context.CourseSinif.Remove(courseSinif);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseSinifExists(Guid cid, Guid sid)
        {
          return (_context.CourseSinif?.Any(e => e.CourseId == cid && e.SinifId == sid)).GetValueOrDefault();
        }
    }
}
