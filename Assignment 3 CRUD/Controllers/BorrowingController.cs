using Assignment_3_CRUD.Data;
using Assignment_3_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assignment_3_CRUD.Controllers
{
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
        public IActionResult BorrowingList()
        {
            var borrowings = _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .ToList();
            return View(borrowings);
        }

        //Display Borrowing details
        [HttpGet("Details/{id}")]
        public IActionResult BorrowingDetails(int id)
        {
            var borrowing = _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .FirstOrDefault(b => b.Id == id);


            if (borrowing == null)
            {
                return NotFound();
            }

            return View(borrowing);
        }

        //Show create form
        [HttpGet("AddBorrow")]
        public IActionResult AddBorrow()
        {
            var books = _context.Books
                .Where(b => b.Availability)
                .Select(b => new { b.Id, b.Title })
                .ToList();
            ViewBag.Books = new SelectList(books, "Id", "Title");

            var readers = _context.Readers
                .Select(r => new { r.Id, r.FullName })
                .ToList();
            ViewBag.Readers = new SelectList(readers, "Id", "FullName");

            return View();
        }

        // Add a new Borrowing
        [HttpPost("AddBorrow")]
        public IActionResult AddBorrow(Borrowing newBorrowing)
        {
            if (ModelState.IsValid)
            {

                var book = _context.Books.Find(newBorrowing.BookId);
                newBorrowing.Status = StatusEnum.Borrowed; // Default selection
                newBorrowing.Book = book;
                newBorrowing.Reader = _context.Readers.Find(newBorrowing.ReaderId);
                _context.Borrowings.Add(newBorrowing);
                _context.SaveChanges();

                // Toggle book availability (mark it as borrowed)

                if (book != null)
                {
                    book.Availability = false;
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(BorrowingList)); // Redirect to Borrowing List page
            }

            var books = _context.Books
                .Where(b => b.Availability)
                .Select(b => new { b.Id, b.Title })
                .ToList();
            ViewBag.Books = new SelectList(books, "Id", "Title");

            var readers = _context.Readers
                .Select(r => new { r.Id, r.FullName })
                .ToList();
            ViewBag.Readers = new SelectList(readers, "Id", "FullName");

            return View(newBorrowing);
        }


        //Show edit form
        [HttpGet("EditBorrow")]
        public IActionResult EditBorrow(int Id)
        {
            var borrowing = _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.Reader)
            .FirstOrDefault(b => b.Id == Id);

            if (borrowing == null)
            {
                return NotFound();
            }
            return View(borrowing);
        }

        [HttpPost("EditBorrow")]
        public IActionResult EditBorrow(Borrowing updatedBorrowing)
        {
            if (updatedBorrowing.Id == 0)
            {
                ModelState.AddModelError("", "Invalid borrowing data.");
                return View(updatedBorrowing);
            }

            // Validation: If Status is "Returned", ReturnedDate is required
            if (updatedBorrowing.Status == StatusEnum.Returned && updatedBorrowing.ReturnedDate == null)
            {
                ModelState.AddModelError("ReturnedDate", "Returned Date is required when the status is 'Returned'.");
            }

            if (!ModelState.IsValid)
            {
                updatedBorrowing.Book = _context.Books.Find(updatedBorrowing.BookId);
                updatedBorrowing.Reader = _context.Readers.Find(updatedBorrowing.ReaderId);
                return View(updatedBorrowing);
            }

            var borrowing = _context.Borrowings.Find(updatedBorrowing.Id);
            if (borrowing == null)
            {
                return NotFound("Borrowing record not found.");
            }

            // Store previous status
            var previousStatus = borrowing.Status;

            // Update borrowing details
            borrowing.BorrowDate = updatedBorrowing.BorrowDate;
            borrowing.ReturnDate = updatedBorrowing.ReturnDate;
            borrowing.Notes = updatedBorrowing.Notes;
            borrowing.Status = updatedBorrowing.Status;
            borrowing.ReturnedDate = updatedBorrowing.ReturnedDate;

            // Update book availability based on status change
            var book = _context.Books.Find(borrowing.BookId);
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

            _context.SaveChanges();
            return RedirectToAction(nameof(BorrowingList));
        }




        //Show delete confirmation
        [HttpGet("Delete/{id}")]
        public IActionResult DeleteBorrow(int id)
        {
            var borrowing = _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .FirstOrDefault(b => b.Id == id);

            if (borrowing == null)
            {
                return NotFound();
            }


            return View(borrowing);
        }

        //Confirm and delete Borrowing
        [HttpPost("Delete/{id}")]
        public IActionResult DeleteBorrow(Borrowing borrowing)
        {
            var existingBorrowing = _context.Borrowings.Find(borrowing.Id);

            if (existingBorrowing == null)
            {
                return NotFound();
            }

            // Fetch the associated book using BookId from the borrowing object
            var book = _context.Books.Find(existingBorrowing.BookId);
            if (book != null)
            {
                book.Availability = true; // Mark the book as available
                _context.SaveChanges(); // Save the changes to the book
            }

            // Remove the borrowing entry from the database
            _context.Borrowings.Remove(existingBorrowing);
            _context.SaveChanges();

            return RedirectToAction(nameof(BorrowingList));
        }

        // Search Books by Title, Author, or Genre
        [HttpGet("Search")]
        public IActionResult Search(string query)
        {
            var borrowings = _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .Where(b =>
                    (b.Book != null && b.Book.Title.Contains(query)) ||
                    (b.Reader != null && b.Reader.FullName.Contains(query)) ||
                    (b.Status.ToString().Contains(query)) || 
                    (!string.IsNullOrEmpty(b.Notes) && b.Notes.Contains(query))
                )
                .ToList();

            return View("SearchResults", borrowings);
        }

    }
}
