using salonytics.Models.Entities;
using Salonytics.Models.Entities;

namespace salonytics.Models
{
    internal class CustomerDetailsViewModel
    {
        public Customer Customer { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}