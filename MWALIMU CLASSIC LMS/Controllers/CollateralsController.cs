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
    public class CollateralsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CollateralsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Collaterals
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Collaterals.Include(c => c.Loan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Collaterals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collateral = await _context.Collaterals
                .Include(c => c.Loan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collateral == null)
            {
                return NotFound();
            }

            return View(collateral);
        }

        // GET: Collaterals/Create
        public IActionResult Create()
        {
            ViewData["LoanId"] = new SelectList(_context.Loans, "Id", "Status");
            return View();
        }

        // POST: Collaterals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Value,Description,LoanId")] Collateral collateral)
        {
            if (ModelState.IsValid)
            {
                _context.Add(collateral);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoanId"] = new SelectList(_context.Loans, "Id", "Status", collateral.LoanId);
            return View(collateral);
        }

        // GET: Collaterals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collateral = await _context.Collaterals.FindAsync(id);
            if (collateral == null)
            {
                return NotFound();
            }
            ViewData["LoanId"] = new SelectList(_context.Loans, "Id", "Status", collateral.LoanId);
            return View(collateral);
        }

        // POST: Collaterals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Value,Description,LoanId")] Collateral collateral)
        {
            if (id != collateral.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collateral);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollateralExists(collateral.Id))
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
            ViewData["LoanId"] = new SelectList(_context.Loans, "Id", "Status", collateral.LoanId);
            return View(collateral);
        }

        // GET: Collaterals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collateral = await _context.Collaterals
                .Include(c => c.Loan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collateral == null)
            {
                return NotFound();
            }

            return View(collateral);
        }

        // POST: Collaterals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collateral = await _context.Collaterals.FindAsync(id);
            if (collateral != null)
            {
                _context.Collaterals.Remove(collateral);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollateralExists(int id)
        {
            return _context.Collaterals.Any(e => e.Id == id);
        }
    }
}
