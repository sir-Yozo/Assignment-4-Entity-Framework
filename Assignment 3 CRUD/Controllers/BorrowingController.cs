using Assignment_3_CRUD.Data;
using Assignment_3_CRUD.Models;
using Assignment_3_CRUD.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assignment_3_CRUD.Controllers
{
    [Route("[controller]")]
    public class BorrowingController : Controller
    {

        //private readonly IBorrowingRepository _borrowingRepository;
        //private readonly IBookRepository _bookRepository;
        //private readonly IReaderRepository _readerRepository;


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

            var borrowingViewModels = borrowings.Select(borrowing => new BorrowingDetailsViewModel
            {
                Borrowing = borrowing,
                Book = borrowing.Book,
                Reader = borrowing.Reader
            }).ToList();


            return View(borrowingViewModels);
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

            var viewModel = new BorrowingDetailsViewModel
            {
                Borrowing = borrowing,
                Book = borrowing.Book,
                Reader = borrowing.Reader
            };

            return View(viewModel);
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


        //Show create form
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

            var viewModel = new BorrowingDetailsViewModel
            {
                Borrowing = borrowing,
                Book = borrowing.Book,
                Reader = borrowing.Reader
            };

            return View(viewModel);
        }

        [HttpPost("EditBorrow")]
        public IActionResult EditBorrow(BorrowingDetailsViewModel borrowingView)
        {
            if (borrowingView.Borrowing == null)
            {
                return BadRequest("Invalid borrowing data.");
            }

            // Validation: If Status is "Returned", ReturnedDate is required
            if (borrowingView.Borrowing.Status == StatusEnum.Returned && borrowingView.Borrowing.ReturnedDate == null)
            {
                ModelState.AddModelError("Borrowing.ReturnedDate", "Returned Date is required when the status is 'Returned'.");
            }

            if (!ModelState.IsValid)
            {
                return View(borrowingView); // Return view with validation errors
            }

            var borrowing = _context.Borrowings.Find(borrowingView.Borrowing.Id);
            if (borrowing == null)
            {
                return NotFound();
            }

            // Check if the status has changed
            var previousStatus = borrowing.Status;

            // Update borrowing details
            borrowing.BorrowDate = borrowingView.Borrowing.BorrowDate;
            borrowing.ReturnDate = borrowingView.Borrowing.ReturnDate;
            borrowing.Notes = borrowingView.Borrowing.Notes;
            borrowing.Status = borrowingView.Borrowing.Status;
            borrowing.ReturnedDate = borrowingView.Borrowing.ReturnedDate;

            _context.SaveChanges();

            // If the status is changed to "Returned", mark the book as available
            if (previousStatus == StatusEnum.Borrowed && borrowing.Status == StatusEnum.Returned)
            {
                var book = _context.Books.Find(borrowing.BookId);
                if (book != null)
                {
                    book.Availability = true;
                    _context.SaveChanges();
                }
            }

            // If the status is changed from "Returned" to "Borrowed", mark the book as not available
            if (previousStatus == StatusEnum.Returned && borrowing.Status == StatusEnum.Borrowed)
            {
                var book = _context.Books.Find(borrowing.BookId);
                if (book != null)
                {
                    book.Availability = false;
                    _context.SaveChanges();
                }
            }

            return RedirectToAction(nameof(BorrowingList));
        }


        /*
            //Show delete confirmation
            [HttpGet("Delete/{id}")]
            public IActionResult DeleteBorrow(int id)
            {
                var borrowing = FindOrFail(id);

                if (borrowing == null)
                {
                    return NotFound();
                }

                var bookName = _borrowingRepository.GetBookName(borrowing.BookId);
                var readerName = _borrowingRepository.GetReaderName(borrowing.ReaderId);

                var viewModel = new BorrowingDetailsViewModel
                {
                    Borrowing = borrowing,
                    BookName = bookName,
                    ReaderName = readerName
                };

                return View(viewModel);
            }

            //Confirm and delete Borrowing
            [HttpPost("Delete/{id}")]
            public IActionResult ConfirmDelete(int id)
            {
                var borrowing = FindOrFail(id);
                if (borrowing == null) return NotFound();
                _bookRepository.SetToAvailable(borrowing.BookId);

                _borrowingRepository.DeleteBorrowing(id);
                return RedirectToAction(nameof(BorrowingList));
            }

            //Helper Method: Find Borrowing or return null
            private Borrowing FindOrFail(int id) => _borrowingRepository.GetBorrowingById(id);
            */
    }
}
