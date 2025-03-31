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
    public class LoanOfficersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoanOfficersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoanOfficers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LoanOfficers.Include(l => l.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LoanOfficers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanOfficer = await _context.LoanOfficers
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanOfficer == null)
            {
                return NotFound();
            }

            return View(loanOfficer);
        }

        // GET: LoanOfficers/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: LoanOfficers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,ApplicationUserId")] LoanOfficer loanOfficer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loanOfficer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", loanOfficer.ApplicationUserId);
            return View(loanOfficer);
        }

        // GET: LoanOfficers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanOfficer = await _context.LoanOfficers.FindAsync(id);
            if (loanOfficer == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", loanOfficer.ApplicationUserId);
            return View(loanOfficer);
        }

        // POST: LoanOfficers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,ApplicationUserId")] LoanOfficer loanOfficer)
        {
            if (id != loanOfficer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanOfficer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanOfficerExists(loanOfficer.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", loanOfficer.ApplicationUserId);
            return View(loanOfficer);
        }

        // GET: LoanOfficers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanOfficer = await _context.LoanOfficers
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanOfficer == null)
            {
                return NotFound();
            }

            return View(loanOfficer);
        }

        // POST: LoanOfficers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanOfficer = await _context.LoanOfficers.FindAsync(id);
            if (loanOfficer != null)
            {
                _context.LoanOfficers.Remove(loanOfficer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanOfficerExists(int id)
        {
            return _context.LoanOfficers.Any(e => e.Id == id);
        }
    }
}
