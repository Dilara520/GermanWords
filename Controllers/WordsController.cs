using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GermanWords.Data;
using GermanWords.Models;
using Microsoft.AspNetCore.Authorization;

namespace GermanWords.Controllers
{
    public class WordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Words
        public async Task<IActionResult> Index()
        {
              return _context.Word != null ? 
                          View(await _context.Word.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Word'  is null.");
        }
        
        // GET: Words/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: Words/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchTerm)
        {
            return View("Index", await _context.Word.Where(j => j.WordQuestion.Contains(SearchTerm)).ToListAsync());
        }

        // GET: Words/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Word == null)
            {
                return NotFound();
            }

            var word = await _context.Word
                .FirstOrDefaultAsync(m => m.Id == id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        // GET: Words/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Words/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WordQuestion,WordAnswer")] Word word)
        {
            if (ModelState.IsValid)
            {
                _context.Add(word);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(word);
        }

        // GET: Words/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Word == null)
            {
                return NotFound();
            }

            var word = await _context.Word.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }

        // POST: Words/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WordQuestion,WordAnswer")] Word word)
        {
            if (id != word.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(word);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WordExists(word.Id))
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
            return View(word);
        }

        // GET: Words/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Word == null)
            {
                return NotFound();
            }

            var word = await _context.Word
                .FirstOrDefaultAsync(m => m.Id == id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        // POST: Words/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Word == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Word'  is null.");
            }
            var word = await _context.Word.FindAsync(id);
            if (word != null)
            {
                _context.Word.Remove(word);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WordExists(int id)
        {
          return (_context.Word?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
