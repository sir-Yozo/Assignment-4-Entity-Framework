using Assignment_3_CRUD___Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Assignment_3_CRUD___Model.Models
{
    public class BorrowingDetailsViewModel
    {
        public Borrowing Borrowing { get; set; }
        public string BookName { get; set; }
        public string ReaderName { get; set; }
    }
}
