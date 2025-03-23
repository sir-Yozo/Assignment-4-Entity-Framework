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


    }
}
