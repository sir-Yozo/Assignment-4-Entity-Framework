using Assignment_3_CRUD___Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_3_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingController : ControllerBase
    {
        private static List<Borrowing> borrowings = new List<Borrowing>
        {
            new Borrowing
            {
                Id = 1,
                BookId = 1,
                ReaderId = 1,
                BorrowDate = new DateTime(2025, 2, 10),
                ReturnDate = new DateTime(2025, 2, 24),
                ReturnedDate = null
            },
            new Borrowing
            {
                Id = 2,
                BookId = 2,
                ReaderId = 2,
                BorrowDate = new DateTime(2025, 1, 15),
                ReturnDate = new DateTime(2025, 1, 29),
                ReturnedDate = null
            },
            new Borrowing
            {
                Id = 3,
                BookId = 3,
                ReaderId = 3,
                BorrowDate = new DateTime(2025, 2, 1),
                ReturnDate = new DateTime(2025, 2, 15),
                ReturnedDate = null
            }
        };

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(borrowings);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var borrowing = borrowings.FirstOrDefault(b => b.Id == id);
            if (borrowing == null)
            {
                return NotFound();
            }
            return Ok(borrowing);
        }

        [HttpPost]
        public IActionResult Post(Borrowing newBorrowing)
        {
            borrowings.Add(newBorrowing);
            return Ok(newBorrowing);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Borrowing updatedBorrowing)
        {
            var borrowing = borrowings.FirstOrDefault(b => b.Id == id);
            if (borrowing == null)
            {
                return NotFound();
            }

            borrowing.BookId = updatedBorrowing.BookId;
            borrowing.ReaderId = updatedBorrowing.ReaderId;
            borrowing.BorrowDate = updatedBorrowing.BorrowDate;
            borrowing.ReturnDate = updatedBorrowing.ReturnDate;
            borrowing.ReturnedDate = updatedBorrowing.ReturnedDate;

            return Ok(borrowing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var borrowing = borrowings.FirstOrDefault(b => b.Id == id);
            if (borrowing == null)
            {
                return NotFound();
            }

            borrowings.Remove(borrowing);
            return Ok(borrowing);
        }

    }
}
