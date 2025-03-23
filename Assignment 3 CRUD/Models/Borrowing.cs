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

        [Required]
        public DateTime ReturnDate { get; set; }

        public DateTime? ReturnedDate { get; set; }

        [Required]
        public String Notes { get; set; }

        [Required]
        public StatusEnum Status { get; set; } // Use enum for status

    }

    public enum StatusEnum
    {
        
        Borrowed,
        Returned,
        Cancelled
    }

}
