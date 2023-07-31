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
    public class SinifController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SinifController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sinifs
        public async Task<IActionResult> Index()
        {
              return _context.Sinifs != null ? 
                          View(await _context.Sinifs.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Sinifs'  is null.");
        }

        // GET: Sinifs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Sinifs == null)
            {
                return NotFound();
            }

            var sinif = await _context.Sinifs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sinif == null)
            {
                return NotFound();
            }

            return View(sinif);
        }

        // GET: Sinifs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sinifs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Sinif sinif)
        {
            if (ModelState.IsValid)
            {
                sinif.Id = Guid.NewGuid();
                _context.Add(sinif);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sinif);
        }

        // GET: Sinifs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Sinifs == null)
            {
                return NotFound();
            }

            var sinif = await _context.Sinifs.FindAsync(id);
            if (sinif == null)
            {
                return NotFound();
            }
            return View(sinif);
        }

        // POST: Sinifs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Sinif sinif)
        {
            if (id != sinif.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sinif);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinifExists(sinif.Id))
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
            return View(sinif);
        }

        // GET: Sinifs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Sinifs == null)
            {
                return NotFound();
            }

            var sinif = await _context.Sinifs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sinif == null)
            {
                return NotFound();
            }

            return View(sinif);
        }

        // POST: Sinifs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Sinifs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sinifs'  is null.");
            }
            var sinif = await _context.Sinifs.FindAsync(id);
            if (sinif != null)
            {
                _context.Sinifs.Remove(sinif);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SinifExists(Guid id)
        {
          return (_context.Sinifs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
