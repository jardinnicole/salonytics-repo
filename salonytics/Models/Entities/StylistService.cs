using Salonytics.Models.Entities;

namespace salonytics.Models.Entities
{
    public class StylistService
    {
        public Guid Id { get; set; }
        public Guid StylistId { get; set; }
        public Stylist Stylist { get; set; }
        public Guid ServiceId { get; set; } 
        public Service Service { get; set; }
    }
}
