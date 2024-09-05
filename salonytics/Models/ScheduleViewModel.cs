using Microsoft.AspNetCore.Mvc.Rendering;
using Salonytics.Models.Entities;

namespace salonytics.Models
{
    public class ScheduleViewModel
    {
        public Appointment Appointment { get; set; }
        public string AppointmentCode { get; set; }

        public IEnumerable<SelectListItem> Services { get; set; }
        public Guid? SelectedServiceId { get; set; }

        public IEnumerable<SelectListItem> Branches { get; set; }
        public IEnumerable<SelectListItem> Stylists { get; set; }
        public IEnumerable<SelectListItem> StartTime { get; set; }
        public IEnumerable<DateTime> FullyBookedDates { get; set; }


    }

}