using salonytics.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salonytics.Models.Entities
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public string AppointmentCode { get; set; }

        [Required(ErrorMessage = "Appointment status is required.")]
        public string AppointmentStatus { get; set; }

        public bool IsApproved { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Contact number must be exactly 11 digits.")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        // Foreign key properties.
        [Required(ErrorMessage = "Service is required.")]
        public Guid ServiceId { get; set; }

        [Required(ErrorMessage = "Branch is required.")]
        public Guid BranchId { get; set; }

        public Guid? StylistId { get; set; }
        public string? CustomerId { get; set; }

        // Navigation properties
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }

        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }

        [ForeignKey("StylistId")]
        public Stylist? Stylist { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Number of heads is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of heads must be greater than zero.")]
        public int NoOfHeads { get; set; }
        public string? Remarks { get; set; }

        public float TotalPrice { get; set; }
        public float? TipAmount { get; set; }
        public int? Rating { get; set; }

        public TimeSpan? EndTime { get; set; }
        public bool NotificationSent { get; set; }

        [Required(ErrorMessage = "Terms and conditions acceptance is required.")]
        public bool TermsAndConditionsAccepted { get; set; }

        // Constructor
        public Appointment()
        {
            AppointmentCode = "";
            TermsAndConditionsAccepted = false;
            TotalPrice = 0;
            NotificationSent = false;
            IsApproved = false;
            NoOfHeads = 1;
            AppointmentStatus = "Pending";
        }

        // Dictionary to map branches to their codes
        private static readonly Dictionary<string, string> BranchCodes = new()
        {
            { "BOCAUE BRANCH", "BOCA" },
            { "SOUTH BRANCH MALOLOS", "SMLB" },
            { "APALIT BRANCH", "APLT" },
            { "PULILAN BRANCH", "PLLN" },
            { "PLARIDEL BRANCH", "PLRD" },
            { "SAN FERNANDO PAMPANGA BRANCH", "SFPB" },
            { "GUIGUINTO BRANCH", "GUIG" },
            { "MOJON BRANCH", "MOJN" },
            { "STA RITA GUIGUINTO BRANCH", "SRGB" },
            { "MALOLOS MAIN BRANCH", "MMLB" }
        };

        // Method to generate the AppointmentCode
        public void GenerateAppointmentCode(string branchName, int lastNumber)
        {
            if (!BranchCodes.TryGetValue(branchName, out var branchCode))
            {
                throw new ArgumentException("Invalid branch name");
            }

            var year = DateTime.Now.Year;
            var number = (lastNumber + 1).ToString("D4");
            AppointmentCode = $"{branchCode}-{year}-{number}";
        }
    }
}
