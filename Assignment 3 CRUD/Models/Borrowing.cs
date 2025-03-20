using System.ComponentModel.DataAnnotations;

namespace Assignment_3_CRUD___Model.Models
{
    public class Borrowing
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int ReaderId { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public DateTime? ReturnedDate { get; set; }
    }

}
