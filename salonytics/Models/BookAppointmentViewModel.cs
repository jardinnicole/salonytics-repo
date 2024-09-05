using System.ComponentModel.DataAnnotations.Schema;
using Salonytics.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace salonytics.Models
{
    public class BookAppointmentViewModel
    {
        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Home Location is required.")]
        public Guid BranchId { get; set; }
        public IEnumerable<SelectListItem> Branches { get; set; }

        [Required(ErrorMessage = "Stylist is required.")]
        public Guid? StylistId { get; set; }
        public IEnumerable<SelectListItem> Stylists { get; set; }

        [Required(ErrorMessage = "Service is required.")]
        public Guid ServiceId { get; set; }
        public IEnumerable<SelectListItem> Services { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        public TimeSpan StartTime { get; set; }
        public IEnumerable<SelectListItem> AvailableStartTimes { get; set; }

        [Required(ErrorMessage = "Number of Heads is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of heads must be greater than zero.")]
        public int NoOfHeads { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Contact number must be exactly 11 digits.")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public float TotalPrice { get; set; }
        public float? TipAmount { get; set; }
        public int? Rating { get; set; }
        public string? Remarks { get; set; }

        public IEnumerable<DateTime> FullyBookedDates { get; set; }
    }
}
