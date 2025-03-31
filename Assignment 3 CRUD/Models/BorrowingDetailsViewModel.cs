using Assignment_3_CRUD.Models;
using System.ComponentModel.DataAnnotations;

namespace Assignment_3_CRUD.Models
{
    public class BorrowingDetailsViewModel
    {
        public Borrowing Borrowing { get; set; }
        public Book Book { get; set; }
        public Reader Reader { get; set; }
    }
}
