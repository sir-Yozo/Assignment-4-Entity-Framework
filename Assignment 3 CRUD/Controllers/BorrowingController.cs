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
            return View(_borrowingRepository.GetAllBorrowings());
        }

        // Display Borrowing details1
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


    }
}
