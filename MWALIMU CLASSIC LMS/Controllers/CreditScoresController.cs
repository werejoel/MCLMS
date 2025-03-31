using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MWALIMU_CLASSIC_LMS.Data;
using MWALIMU_CLASSIC_LMS.Models;

namespace MWALIMU_CLASSIC_LMS.Controllers
{
    public class CreditScoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CreditScoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CreditScores
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CreditScores.Include(c => c.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CreditScores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditScore = await _context.CreditScores
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (creditScore == null)
            {
                return NotFound();
            }

            return View(creditScore);
        }

        // GET: CreditScores/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "ContactNo");
            return View();
        }

        // POST: CreditScores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Score,Rating,LastUpdated,CustomerId")] CreditScore creditScore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(creditScore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "ContactNo", creditScore.CustomerId);
            return View(creditScore);
        }

        // GET: CreditScores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditScore = await _context.CreditScores.FindAsync(id);
            if (creditScore == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "ContactNo", creditScore.CustomerId);
            return View(creditScore);
        }

        // POST: CreditScores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Score,Rating,LastUpdated,CustomerId")] CreditScore creditScore)
        {
            if (id != creditScore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(creditScore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditScoreExists(creditScore.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "ContactNo", creditScore.CustomerId);
            return View(creditScore);
        }

        // GET: CreditScores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditScore = await _context.CreditScores
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (creditScore == null)
            {
                return NotFound();
            }

            return View(creditScore);
        }

        // POST: CreditScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var creditScore = await _context.CreditScores.FindAsync(id);
            if (creditScore != null)
            {
                _context.CreditScores.Remove(creditScore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditScoreExists(int id)
        {
            return _context.CreditScores.Any(e => e.Id == id);
        }
    }
}
