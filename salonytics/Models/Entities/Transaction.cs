using Salonytics.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salonytics.Models.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        public Guid AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }

        [Required]
        public string TransactionId { get; set; }

        [Required]
        public string PayerId { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
