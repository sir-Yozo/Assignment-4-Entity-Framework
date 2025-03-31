using System.ComponentModel.DataAnnotations;

namespace Assignment_3_CRUD.Models
{
    public class Borrowing
    {
        [Key]
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

        public String? Notes { get; set; }

        public StatusEnum Status { get; set; } // Use enum for status

        public Book? Book { get; set; }
        public Reader? Reader { get; set; }


    }

    public enum StatusEnum
    {
        Borrowed,
        Returned,
        Cancelled
    }

}
