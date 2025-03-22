using System.ComponentModel.DataAnnotations;

namespace Assignment_3_CRUD___Model.Models
{
    public class Book
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; } 

        [Required]
        public DateOnly PublishedDate { get; set; }

        [Required]
        public string Genre { get; set; }

        public bool Availability { get; set; }
    }
}
