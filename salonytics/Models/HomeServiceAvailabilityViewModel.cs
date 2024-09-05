using Microsoft.AspNetCore.Mvc.Rendering;

namespace salonytics.Models
{
    public class HomeServiceAvailabilityViewModel
    {
        public Guid BranchId { get; set; }
        public Guid StylistId { get; set; }
        public List<DateTime> AvailableDates { get; set; }
        public List<string> SelectedDates { get; set; }
        public List<SelectListItem> Branches { get; set; }
        public List<SelectListItem> Stylists { get; set; }
    }
}
