using Salonytics.Models.Entities;

namespace salonytics.Models
{
    public class BillingViewModel
    {
        public Appointment Appointment { get; set; }
        public float TotalPrice { get; set; }
        public float ReservationFee { get; set; }

    }
}
