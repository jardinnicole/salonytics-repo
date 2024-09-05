using Salonytics.Models.Entities;

namespace salonytics.Models.Entities
{
    public class Rating
    {
        public Guid RatingId { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public Guid StylistId { get; set; }
        public int Score { get; set; } // e.g., 1 to 5
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public Stylist Stylist { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
