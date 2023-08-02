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
    public class CourseContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseContents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CourseContents.Include(c => c.Content).Include(c => c.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CourseContents/Details/5
        public async Task<IActionResult> Details(Guid? courseId, Guid? contentId)
        {
            if (courseId == null || contentId == null || _context.CourseContents == null)
            {
                return NotFound();
            }

            var courseContent = await _context.CourseContents.FindAsync(courseId, contentId);
            if (courseContent == null)
            {
                return NotFound();
            }

            return View(courseContent);
        }

        // GET: CourseContents/Create
        public IActionResult Create()
        {
            ViewData["ContentId"] = new SelectList(_context.Contents, "Id", "Name");
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            return View();
        }

        // POST: CourseContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,ContentId,Sequence")] CourseContent courseContent)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate entry
                if (!CourseContentExists(courseContent.CourseId, courseContent.ContentId) &&
                    !ContentSequenceExists(courseContent.CourseId, courseContent.Sequence))
                {
                    _context.Add(courseContent);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "A duplicate entry already exists.");
            }

            ViewData["ContentId"] = new SelectList(_context.Contents, "Id", "Name", courseContent.ContentId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseContent.CourseId);
            return View(courseContent);
        }

        // GET: CourseContents/Edit/5
        public async Task<IActionResult> Edit(Guid? courseId, Guid? contentId)
        {
            if (courseId == null || _context.CourseContents == null)
            {
                return NotFound();
            }

            var courseContent = await _context.CourseContents.FindAsync(courseId, contentId);
            
            if (courseContent == null)
            {
                return NotFound();
            }
            ViewData["ContentId"] = new SelectList(_context.Contents, "Id", "Name", courseContent.ContentId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseContent.CourseId);
            return View(courseContent);
        }

        // POST: CourseContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid courseId, Guid contentId, [Bind("CourseId,ContentId,Sequence")] CourseContent courseContent)
        {
            if (courseId != courseContent.CourseId || contentId != courseContent.ContentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!ContentSequenceExists(courseContent.CourseId, courseContent.Sequence))
                {
                    try
                    {
                        _context.Update(courseContent);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CourseContentExists(courseContent.CourseId, courseContent.ContentId))
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
            ViewData["ContentId"] = new SelectList(_context.Contents, "Id", "Name", courseContent.ContentId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseContent.CourseId);
            return View(courseContent);
        }

        // GET: CourseContents/Delete/5
        public async Task<IActionResult> Delete(Guid? courseId, Guid? contentId)
        {
            if (courseId == null || _context.CourseContents == null)
            {
                return NotFound();
            }

            var courseContent = await _context.CourseContents.FindAsync(courseId, contentId);
            if (courseContent == null)
            {
                return NotFound();
            }

            return View(courseContent);
        }

        // POST: CourseContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid courseId, Guid contentId)
        {
            if (_context.CourseContents == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CourseContents'  is null.");
            }
            var courseContent = await _context.CourseContents.FindAsync(courseId, contentId);
            if (courseContent != null)
            {
                _context.CourseContents.Remove(courseContent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseContentExists(Guid courseId, Guid contentId)
        {
          return (_context.CourseContents?.Any(e => e.CourseId == courseId && e.ContentId == contentId)).GetValueOrDefault();
        }

        private bool ContentSequenceExists(Guid courseId, int sequence)
        {
            return (_context.CourseContents?.Any(e => e.CourseId == courseId && e.Sequence == sequence)).GetValueOrDefault();
        }
    }
}
