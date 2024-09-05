namespace salonytics.Models.Entities
{
    public class StylistSlot
    {
        public Guid StylistSlotId { get; set; }
        public Guid StylistId { get; set; }
        public Stylist Stylist { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
