using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace salonytics.Models
{
    public class EditAppointmentViewModel
    {
        public Guid AppointmentId { get; set; }

        [Display(Name = "Service")]
        [Required(ErrorMessage = "Please select a service")]
        public Guid ServiceId { get; set; }

        public IEnumerable<SelectListItem> Services { get; set; }
        public Guid BranchId { get; set; }

        public IEnumerable<SelectListItem> Branches { get; set; }
        public Guid StylistId { get; set; }

        public IEnumerable<SelectListItem> Stylists { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Please select a date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }

        public float TotalPrice { get; set; }

        [Display(Name = "Appointment Status")]
        [Required(ErrorMessage = "Please select an appointment status")]
        public string AppointmentStatus { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }
    }
}
