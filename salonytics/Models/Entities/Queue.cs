using System.ComponentModel.DataAnnotations.Schema;
using Salonytics.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace salonytics.Models.Entities
{
    public class Queue
    {
        public string QueueId { get; set; }

        [Required(ErrorMessage = "Appointment is required.")]
        public Guid AppointmentId { get; set; }

        [Required(ErrorMessage = "Queue position is required.")]
        public int Position { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        // Navigation properties
        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }
    }
}

