using Salonytics.Models.Entities;

namespace salonytics.Models
{
    public class StylistTransactionViewModel
    {
        public Guid StylistId { get; set; }
        public string StylistName { get; set; }
        public List<Appointment> DoneAppointments { get; set; }
        public float PaymentAmount { get; set; }
    }
}
