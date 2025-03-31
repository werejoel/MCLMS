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
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Customers.Include(c => c.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            // Use a more descriptive field for the display text (UserName instead of Id)
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "UserName");

            // Initialize a new customer with a default CreditScore
            var customer = new Customer
            {
                CreditScore = new CreditScore
                {
                    Score = 0,
                    Rating = "Default",
                    LastUpdated = DateTime.Now
                }
            };

            return View(customer);
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ContactNo,Email,Address,WorkPlace,ApplicationUserId")] Customer customer)
        {
            try
            {
                // Check ModelState errors
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid. Validation errors:");

                    // Add model validation errors to TempData for display in view
                    ViewData["ValidationErrors"] = new List<string>();

                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"- {state.Key}: {error.ErrorMessage}");
                            ((List<string>)ViewData["ValidationErrors"]).Add($"{state.Key}: {error.ErrorMessage}");
                        }
                    }

                    // Add a summary error message
                    ModelState.AddModelError(string.Empty, "There were validation errors. Please check the form and try again.");
                }
                else
                {
                    Console.WriteLine($"Attempting to add customer: {customer.Name}, Email: {customer.Email}");
                    Console.WriteLine($"ApplicationUserId: {customer.ApplicationUserId ?? "null"}");

                    // Create and associate a new CreditScore with required fields
                    customer.CreditScore = new CreditScore
                    {
                        Score = 0,
                        Rating = "Default",  // Set a default rating
                        LastUpdated = DateTime.Now
                        // Add any other required properties
                    };

                    _context.Add(customer);
                    var result = await _context.SaveChangesAsync();

                    Console.WriteLine($"SaveChangesAsync result: {result} records affected");

                    // Add success message to TempData
                    TempData["SuccessMessage"] = "Customer created successfully!";

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                // Log the complete exception details
                Console.WriteLine("Exception during customer creation:");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    // Add inner exception message to view for display
                    ModelState.AddModelError(string.Empty, $"Inner exception: {ex.InnerException.Message}");
                }

                // Add the exception message to ModelState for display in view
                ModelState.AddModelError(string.Empty, $"Unable to save: {ex.Message}");

                // Add detailed error information to ViewData
                ViewData["ErrorDetails"] = new Dictionary<string, string>
        {
            { "Message", ex.Message },
            { "StackTrace", ex.StackTrace },
            { "InnerException", ex.InnerException?.Message ?? "None" }
        };
            }

            // Use UserName for display instead of Id when returning the view on error
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "UserName", customer.ApplicationUserId);
            return View(customer);
        }
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.CreditScore) // Include CreditScore
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            // If CreditScore is still null, create one
            if (customer.CreditScore == null)
            {
                customer.CreditScore = new CreditScore
                {
                    Score = 0,
                    LastUpdated = DateTime.Now
                };
            }

            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "UserName", customer.ApplicationUserId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ContactNo,Email,Address,WorkPlace,ApplicationUserId")] Customer customer)
        {
            if (id != customer.Id)
            {
                Console.WriteLine($"ID mismatch: URL ID = {id}, Customer ID = {customer.Id}");
                ModelState.AddModelError(string.Empty, $"ID mismatch: URL ID = {id}, Customer ID = {customer.Id}");
                return NotFound();
            }

            try
            {
                // Check ModelState errors
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid. Validation errors:");

                    // Add model validation errors to ViewData for display in view
                    ViewData["ValidationErrors"] = new List<string>();

                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"- {state.Key}: {error.ErrorMessage}");
                            ((List<string>)ViewData["ValidationErrors"]).Add($"{state.Key}: {error.ErrorMessage}");
                        }
                    }

                    // Add a summary error message
                    ModelState.AddModelError(string.Empty, "There were validation errors. Please check the form and try again.");
                }
                else
                {
                    Console.WriteLine($"Attempting to update customer ID {id}: {customer.Name}");
                    Console.WriteLine($"ApplicationUserId: {customer.ApplicationUserId ?? "null"}");

                    // Get existing customer with credit score
                    var existingCustomer = await _context.Customers
                        .Include(c => c.CreditScore)
                        .FirstOrDefaultAsync(c => c.Id == id);

                    if (existingCustomer != null)
                    {
                        // Update fields from form
                        existingCustomer.Name = customer.Name;
                        existingCustomer.ContactNo = customer.ContactNo;
                        existingCustomer.Email = customer.Email;
                        existingCustomer.Address = customer.Address;
                        existingCustomer.WorkPlace = customer.WorkPlace;
                        existingCustomer.ApplicationUserId = customer.ApplicationUserId;

                        // Make sure CreditScore exists
                        if (existingCustomer.CreditScore == null)
                        {
                            existingCustomer.CreditScore = new CreditScore
                            {
                                Score = 0,
                                LastUpdated = DateTime.Now
                            };
                        }

                        _context.Update(existingCustomer);
                    }
                    else
                    {
                        // If somehow the customer doesn't exist, create a new CreditScore
                        customer.CreditScore = new CreditScore
                        {
                            Score = 0,
                            LastUpdated = DateTime.Now
                        };

                        _context.Update(customer);
                    }

                    var result = await _context.SaveChangesAsync();

                    Console.WriteLine($"SaveChangesAsync result: {result} records affected");

                    // Add success message to TempData
                    TempData["SuccessMessage"] = "Customer updated successfully!";

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CustomerExists(customer.Id))
                {
                    Console.WriteLine($"Customer with ID {customer.Id} does not exist.");
                    ModelState.AddModelError(string.Empty, $"Customer with ID {customer.Id} does not exist.");
                    return NotFound();
                }
                else
                {
                    Console.WriteLine($"Concurrency exception: {ex.Message}");
                    ModelState.AddModelError(string.Empty, $"Concurrency exception: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Log the complete exception details
                Console.WriteLine("Exception during customer update:");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    // Add inner exception message to view for display
                    ModelState.AddModelError(string.Empty, $"Inner exception: {ex.InnerException.Message}");
                }

                // Add the exception message to ModelState for display in view
                ModelState.AddModelError(string.Empty, $"Unable to save: {ex.Message}");

                // Add detailed error information to ViewData
                ViewData["ErrorDetails"] = new Dictionary<string, string>
                {
                    { "Message", ex.Message },
                    { "StackTrace", ex.StackTrace },
                    { "InnerException", ex.InnerException?.Message ?? "None" }
                };
            }

            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "UserName", customer.ApplicationUserId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.CreditScore) // Include CreditScore for deletion
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer != null)
            {
                // Remove CreditScore if it exists
                if (customer.CreditScore != null)
                {
                    _context.Remove(customer.CreditScore);
                }

                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}