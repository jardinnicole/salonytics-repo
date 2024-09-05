using Microsoft.AspNetCore.Mvc.Rendering;
using Salonytics.Models.Entities;

namespace salonytics.Models
{
    public class AppointmentIndexViewModel
    {
        // Properties to hold appointments data
        public IEnumerable<Appointment> Appointments { get; set; }

        // Properties to hold filter options
        public IEnumerable<SelectListItem> Services { get; set; }
        public IEnumerable<SelectListItem> Branches { get; set; }
        public IEnumerable<SelectListItem> Stylists { get; set; }

        // Properties to hold filter values
        public Guid? ServiceId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? StylistId { get; set; }
        public string Status { get; set; }

        // Constructor to initialize collections
        public AppointmentIndexViewModel()
        {
            Services = new List<SelectListItem>();
            Branches = new List<SelectListItem>();
            Stylists = new List<SelectListItem>();
        }
    }
}
