using Assignment_3_CRUD.Data;
using Assignment_3_CRUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment_3_CRUD.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ReaderController : Controller
    {
        private readonly LMA_DBcontext _context;

        public ReaderController(LMA_DBcontext context)
        {
            _context = context;
        }

        // Display All Readers
        [HttpGet]
        public async Task<IActionResult> ReaderListPage()
        {
            var readers = await _context.Readers.ToListAsync();
            return View(readers);
        }

        // Get Reader Details
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> ReaderDetails(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            return reader != null ? View(reader) : NotFound();
        }

        // Show create form
        [HttpGet("AddReader")]
        public IActionResult AddReader()
        {
            return View();
        }

        // Add a new Reader
        [HttpPost("AddReader")]
        public async Task<IActionResult> AddReader(Reader newReader)
        {
            if (ModelState.IsValid)
            {
                await _context.Readers.AddAsync(newReader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ReaderListPage));
            }

            return View(newReader);
        }

        // Show edit form
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> EditReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            return reader != null ? View(reader) : NotFound();
        }

        // Update Reader details
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> EditReader(int id, Reader reader)
        {
            if (id != reader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(reader).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ReaderListPage));
            }

            return View(reader);
        }

        // Show delete confirmation
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            return reader != null ? View(reader) : NotFound();
        }

        // Confirm and delete Reader
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null) return NotFound();

            _context.Readers.Remove(reader);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ReaderListPage));
        }

        // Search Readers by Name
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string query)
        {
            var readers = await _context.Readers
                .Where(r => r.FullName.Contains(query))
                .ToListAsync();

            return View("SearchResults", readers);
        }


    }
}
