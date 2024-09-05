using System.ComponentModel.DataAnnotations;

namespace salonytics.Models.Entities
{
    public class HomeServiceAvailability
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid BranchId { get; set; }
        public Guid StylistId { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual Stylist Stylist { get; set; }
    }
}
