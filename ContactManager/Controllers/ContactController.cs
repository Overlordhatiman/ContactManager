using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ContactManager.Data;
using ContactManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using ContactManager.Validation;
using FluentValidation.Results;
using System.Globalization;
using System.Diagnostics;

namespace ContactManager.Controllers
{

    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display Contacts with sorting, filtering
        public async Task<IActionResult> Index(string search, string sortField)
        {
            var contacts = _context.Contacts.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                contacts = contacts.Where(c => c.Name.Contains(search) || c.Phone.Contains(search));
            }

            contacts = sortField switch
            {
                "Name" => contacts.OrderBy(c => c.Name),
                "Salary" => contacts.OrderBy(c => c.Salary),
                _ => contacts
            };

            return View(await contacts.ToListAsync());
        }

        // GET: Create Contact
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Contact
        [HttpPost]
        public async Task<IActionResult> Create(string input)
        {
            var data = input.Split(",");
            var contact = new Contact
            {
                Name = data[0],
                DateOfBirth = DateTime.Parse(data[1]),
                Married = bool.Parse(data[2]),
                Phone = data[3],
                Salary = Convert.ToDecimal(data[4], new CultureInfo("en-US"))
            };

            var validator = new ContactValidator();
            ValidationResult results = validator.Validate(contact);

            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return View("Error"); // Return error view with validation errors
            }

            // Save to database
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Edit a contact
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string input)
        {
            var contact = _context.Contacts.FirstOrDefault(p => p.Id == id);
            if (contact == null) return NotFound();

            var data = input.Split(",");
            contact.Name = data[0];
            contact.DateOfBirth = DateTime.Parse(data[1]);
            contact.Married = bool.Parse(data[2]);
            contact.Phone = data[3];
            contact.Salary = Convert.ToDecimal(data[4], new CultureInfo("en-US"));

            var validator = new ContactValidator();
            ValidationResult results = validator.Validate(contact);

            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return View("Error");
            }

            // Save changes
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Delete a contact
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(errorViewModel);
        }

    }
}
