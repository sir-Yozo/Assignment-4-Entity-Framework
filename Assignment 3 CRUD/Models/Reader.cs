using System.ComponentModel.DataAnnotations;

namespace Assignment_3_CRUD___Model.Models
{
    public class Reader
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime MembershipDate { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
