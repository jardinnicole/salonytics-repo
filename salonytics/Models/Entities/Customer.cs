using Salonytics.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salonytics.Models.Entities
{
    public class Customer
    {
        public string CustomerId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }


        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Contact number must be exactly 11 digits.")]
        public string ContactNumber { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }


    }
}
