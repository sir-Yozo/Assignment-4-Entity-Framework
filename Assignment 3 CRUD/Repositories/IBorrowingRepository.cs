using Assignment_3_CRUD___Model.Models;
using System.Collections.Generic;

namespace Assignment_3_CRUD___Model.Repositories
{
    public interface IBorrowingRepository
    {
        IEnumerable<Borrowing> GetAllBorrowings();
        Borrowing GetBorrowingById(int id);
        void AddBorrowing(Borrowing borrowing);
        void UpdateBorrowing(Borrowing borrowing);
        void DeleteBorrowing(int id);

        string GetBookName(int id);

        string GetReaderName(int id);
    }
}