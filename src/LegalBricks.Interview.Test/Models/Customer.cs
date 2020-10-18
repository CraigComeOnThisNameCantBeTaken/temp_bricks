using System.ComponentModel.DataAnnotations;

namespace LegalBricks.Interview.Test.Models
{ 
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
