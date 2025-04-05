using Assignment_3_CRUD.Data;
using Assignment_3_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Assignment_3_CRUD.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class BorrowingController : Controller
    {
        private readonly LMA_DBcontext _context;

        public BorrowingController(LMA_DBcontext context)
        {
            _context = context;
        }

        // Display all borrowings
        [HttpGet]
        public async Task<IActionResult> BorrowingList()
        {
            var borrowings = await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .ToListAsync();

            return View(borrowings);
        }

        // Display Borrowing details
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> BorrowingDetails(int id)
        {
            var borrowing = await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .FirstOrDefaultAsync(b => b.Id == id);

            return borrowing != null ? View(borrowing) : NotFound();
        }

        // Show create form
        [HttpGet("AddBorrow")]
        public async Task<IActionResult> AddBorrow()
        {
            var books = await _context.Books
                .Where(b => b.Availability)
                .Select(b => new { b.Id, b.Title })
                .ToListAsync();
            ViewBag.Books = new SelectList(books, "Id", "Title");

            var readers = await _context.Readers
                .Select(r => new { r.Id, r.FullName })
                .ToListAsync();
            ViewBag.Readers = new SelectList(readers, "Id", "FullName");

            return View();
        }

        // Add a new Borrowing
        [HttpPost("AddBorrow")]
        public async Task<IActionResult> AddBorrow(Borrowing newBorrowing)
        {
            if (ModelState.IsValid)
            {
                var book = await _context.Books.FindAsync(newBorrowing.BookId);
                newBorrowing.Status = StatusEnum.Borrowed;
                newBorrowing.Book = book;
                newBorrowing.Reader = await _context.Readers.FindAsync(newBorrowing.ReaderId);

                _context.Borrowings.Add(newBorrowing);
                await _context.SaveChangesAsync();

                if (book != null)
                {
                    book.Availability = false;
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(BorrowingList));
            }

            var books = await _context.Books
                .Where(b => b.Availability)
                .Select(b => new { b.Id, b.Title })
                .ToListAsync();
            ViewBag.Books = new SelectList(books, "Id", "Title");

            var readers = await _context.Readers
                .Select(r => new { r.Id, r.FullName })
                .ToListAsync();
            ViewBag.Readers = new SelectList(readers, "Id", "FullName");

            return View(newBorrowing);
        }

        // Show edit form
        [HttpGet("EditBorrow")]
        public async Task<IActionResult> EditBorrow(int id)
        {
            var borrowing = await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .FirstOrDefaultAsync(b => b.Id == id);

            return borrowing != null ? View(borrowing) : NotFound();
        }

        [HttpPost("EditBorrow")]
        public async Task<IActionResult> EditBorrow(Borrowing updatedBorrowing)
        {
            if (updatedBorrowing.Id == 0)
            {
                ModelState.AddModelError("", "Invalid borrowing data.");
                return View(updatedBorrowing);
            }

            if (updatedBorrowing.Status == StatusEnum.Returned && updatedBorrowing.ReturnedDate == null)
            {
                ModelState.AddModelError("ReturnedDate", "Returned Date is required when the status is 'Returned'.");
            }

            if (!ModelState.IsValid)
            {
                updatedBorrowing.Book = await _context.Books.FindAsync(updatedBorrowing.BookId);
                updatedBorrowing.Reader = await _context.Readers.FindAsync(updatedBorrowing.ReaderId);
                return View(updatedBorrowing);
            }

            var borrowing = await _context.Borrowings.FindAsync(updatedBorrowing.Id);
            if (borrowing == null)
            {
                return NotFound("Borrowing record not found.");
            }

            var previousStatus = borrowing.Status;

            borrowing.BorrowDate = updatedBorrowing.BorrowDate;
            borrowing.ReturnDate = updatedBorrowing.ReturnDate;
            borrowing.Notes = updatedBorrowing.Notes;
            borrowing.Status = updatedBorrowing.Status;
            borrowing.ReturnedDate = updatedBorrowing.ReturnedDate;

            var book = await _context.Books.FindAsync(borrowing.BookId);
            if (book != null)
            {
                if (previousStatus == StatusEnum.Borrowed && borrowing.Status == StatusEnum.Returned)
                {
                    book.Availability = true;
                }
                else if (previousStatus == StatusEnum.Returned && borrowing.Status == StatusEnum.Borrowed)
                {
                    book.Availability = false;
                    borrowing.ReturnedDate = null;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BorrowingList));
        }

        // Show delete confirmation
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> DeleteBorrow(int id)
        {
            var borrowing = await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .FirstOrDefaultAsync(b => b.Id == id);

            return borrowing != null ? View(borrowing) : NotFound();
        }

        // Confirm and delete Borrowing
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteBorrow(Borrowing borrowing)
        {
            var existingBorrowing = await _context.Borrowings.FindAsync(borrowing.Id);

            if (existingBorrowing == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(existingBorrowing.BookId);
            if (book != null)
            {
                book.Availability = true;
                await _context.SaveChangesAsync();
            }

            _context.Borrowings.Remove(existingBorrowing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(BorrowingList));
        }

        // Search borrowings
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string query)
        {
            var borrowings = await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .Where(b =>
                    (b.Book != null && b.Book.Title.Contains(query)) ||
                    (b.Reader != null && b.Reader.FullName.Contains(query)) ||
                    (b.Status.ToString().Contains(query)) ||
                    (!string.IsNullOrEmpty(b.Notes) && b.Notes.Contains(query)))
                .ToListAsync();

            return View("SearchResults", borrowings);
        }
    }
}
