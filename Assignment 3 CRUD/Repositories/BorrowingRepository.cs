using Assignment_3_CRUD.Models;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace Assignment_3_CRUD.Repositories
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private static List<Borrowing> borrowings = new List<Borrowing> {
            new Borrowing
            {
                Id = 1,
                BookId = 3,
                ReaderId = 1,
                BorrowDate = new DateTime(2025, 2, 10),
                ReturnDate = new DateTime(2025, 4, 24),
                ReturnedDate = null,
                Notes = "",
                Status = StatusEnum.Borrowed
            }
        };

        private readonly IBookRepository _bookRepository;
        private readonly IReaderRepository _readerRepository;

        public BorrowingRepository(IBookRepository bookRepository, IReaderRepository readerRepository)
        {
            _bookRepository = bookRepository;
            _readerRepository = readerRepository;
        }

        public IEnumerable<Borrowing> GetAllBorrowings()
        {
            return borrowings;
        }

        public Borrowing GetBorrowingById(int id)
        {
            return borrowings.FirstOrDefault(b => b.Id == id);
        }
        public string GetBookName(int bookId)
        {
            var book = _bookRepository.GetBookById(bookId);
            return book?.Title;
        }

        public string GetReaderName(int readerId)
        {
            var reader = _readerRepository.GetReaderById(readerId);
            return reader?.FullName;
        }

        public void AddBorrowing(Borrowing borrowing)
        {
            borrowing.Id = borrowings.Count() + 1;
            borrowings.Add(borrowing);
        }

        public void UpdateBorrowing(Borrowing borrowing)
        {
            var existingBorrowing = borrowings.FirstOrDefault(b => b.Id == borrowing.Id);
            if (existingBorrowing != null)
            {
                existingBorrowing.BookId = borrowing.BookId;
                existingBorrowing.ReaderId = borrowing.ReaderId;
                existingBorrowing.BorrowDate = borrowing.BorrowDate;
                existingBorrowing.ReturnDate = borrowing.ReturnDate;
                existingBorrowing.ReturnedDate = borrowing.ReturnedDate;
                existingBorrowing.Notes = borrowing.Notes;
                existingBorrowing.Status = borrowing.Status;
            }
        }


        public void DeleteBorrowing(int id)
        {
            var borrowing = GetBorrowingById(id);
            if (borrowing != null)
            {
                borrowings.Remove(borrowing);
            }
        }
    }
}
