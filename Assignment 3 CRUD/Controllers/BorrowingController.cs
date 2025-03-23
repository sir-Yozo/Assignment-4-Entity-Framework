using Assignment_3_CRUD___Model.Models;
using Assignment_3_CRUD___Model.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Assignment_3_CRUD___Model.Controllers
{
    [Route("[controller]")]
    public class BorrowingController : Controller
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IReaderRepository _readerRepository;

        public BorrowingController(IBorrowingRepository borrowingRepository, IBookRepository bookRepository, IReaderRepository readerRepository)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _readerRepository = readerRepository;
        }

        // Display all borrowings
        [HttpGet]
        public IActionResult BorrowingList()
        {
            var borrowings = _borrowingRepository.GetAllBorrowings();

            var borrowingViewModels = borrowings.Select(borrowing => new BorrowingDetailsViewModel
            {
                Borrowing = borrowing,
                BookName = _borrowingRepository.GetBookName(borrowing.BookId),
                ReaderName = _borrowingRepository.GetReaderName(borrowing.ReaderId)
            }).ToList();

            return View(borrowingViewModels);
        }

        // Display Borrowing details
        [HttpGet("Details/{id}")]
        public IActionResult BorrowingDetails(int id)
        {
            var borrowing = _borrowingRepository.GetBorrowingById(id);
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
        //Show create form
        [HttpGet("AddBorrow")]
        public IActionResult AddBorrow()
        {
            var books = _bookRepository.GetAllBooks()
                .Where(b => b.Availability)
                .Select(b => new { b.Id, b.Title })
                .ToList();
            ViewBag.Books = new SelectList(books, "Id", "Title");

            var readers = _readerRepository.GetAllReaders().Select(r => new { r.Id, r.FullName }).ToList();
            ViewBag.Readers = new SelectList(readers, "Id", "FullName");




            return View();
        }
        //Add a new Reader
        [HttpPost("AddBorrow")]
        public IActionResult AddBorrow(Borrowing newBorrowing)
        {


            if (ModelState.IsValid)
            {
                newBorrowing.Status = StatusEnum.Borrowed; // Default selection
                _borrowingRepository.AddBorrowing(newBorrowing);
                // Toggle book availability (mark it as borrowed)
                _bookRepository.SetToNotAvailable(newBorrowing.BookId);
                return RedirectToAction(nameof(BorrowingList)); //Redirect to Reader List page
            }
            var books = _bookRepository.GetAllBooks().Select(b => new { b.Id, b.Title }).ToList();
            ViewBag.Books = new SelectList(books, "Id", "Title");
            

            var readers = _readerRepository.GetAllReaders().Select(r => new { r.Id, r.FullName }).ToList();
            ViewBag.Readers = new SelectList(readers, "Id", "FullName");
            return View(newBorrowing);
        }

        //Show create form
        [HttpGet("EditBorrow")]
        public IActionResult EditBorrow(int Id)
        {
            var borrowing = _borrowingRepository.GetBorrowingById(Id);
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

            var borrowing = _borrowingRepository.GetBorrowingById(borrowingView.Borrowing.Id);
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

            _borrowingRepository.UpdateBorrowing(borrowing);

            // If the status is changed to "Returned", mark the book as available
            if (previousStatus == StatusEnum.Borrowed && borrowing.Status == StatusEnum.Returned)
            {
                _bookRepository.SetToAvailable(borrowing.BookId);
            }

            // If the status is changed from "Returned" to "Borrowed", mark the book as not available
            if (previousStatus == StatusEnum.Returned && borrowing.Status == StatusEnum.Borrowed)
            {
                _bookRepository.SetToNotAvailable(borrowing.BookId);
            }

            return RedirectToAction(nameof(BorrowingList));
        }



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

    }
}
