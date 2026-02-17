using ContactManager.Data;
using ContactManager.Models;
using ContactManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;
        private readonly AppDbContext _context;

        public ContactsController(IContactService contactService, AppDbContext context)
        {
            _contactService = contactService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _context.Contacts.ToListAsync();
            return View(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "File is empty.");
                return View("Index", await _context.Contacts.ToListAsync());
            }

            if (!file.FileName.EndsWith(".csv", System.StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "Invalid file extension.");
                return View("Index", await _context.Contacts.ToListAsync());
            }

            try
            {
                await _contactService.ProcessCsvFileAsync(file);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error processing file.");
                return View("Index", await _context.Contacts.ToListAsync());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}