using Assignment_3_CRUD___Model.Models;
using Assignment_3_CRUD___Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_3_CRUD___Model.Controllers
{
    [Route("[controller]")]
    public class BorrowingController : Controller
    {
        private readonly IBorrowingRepository _borrowingRepository;

        public BorrowingController(IBorrowingRepository borrowingRepository)
        {
            _borrowingRepository = borrowingRepository;
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
            return View();
        }
        //Add a new Reader
        [HttpPost("AddBorrow")]
        public IActionResult AddBorrow(Borrowing newBorrowing)
        {

            if (ModelState.IsValid)
            {
                _borrowingRepository.AddBorrowing(newBorrowing);
                return RedirectToAction(nameof(BorrowingList)); //Redirect to Reader List page
            }

            return View(newBorrowing);
        }


    }
}
