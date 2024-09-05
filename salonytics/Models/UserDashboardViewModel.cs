using salonytics.Models.Entities;
using Salonytics.Models.Entities;

namespace salonytics.Models
{
    public class UserDashboardViewModel
    {
        public IEnumerable<Appointment> Appointments { get; set; }
        public IEnumerable<HomeServiceAvailability> AvailableDates { get; set; }
    }
}
