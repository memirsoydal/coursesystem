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
    public class IcerikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IcerikController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contents
        public async Task<IActionResult> Index()
        {
              return _context.Contents != null ? 
                          View(await _context.Contents.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Contents'  is null.");
        }

        // GET: Contents/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Contents == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // GET: Contents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Slide_Url,Video_Url,Pdf_Url,Is_Homework")] Icerik icerik)
        {
            if (ModelState.IsValid)
            {
                icerik.Id = Guid.NewGuid();
                _context.Add(icerik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(icerik);
        }

        // GET: Contents/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Contents == null)
            {
                return NotFound();
            }

            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Slide_Url,Video_Url,Pdf_Url,Is_Homework")] Icerik icerik)
        {
            if (id != icerik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(icerik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(icerik.Id))
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
            return View(icerik);
        }

        // GET: Contents/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Contents == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Contents == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Contents'  is null.");
            }
            var content = await _context.Contents.FindAsync(id);
            if (content != null)
            {
                _context.Contents.Remove(content);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentExists(Guid id)
        {
          return (_context.Contents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
