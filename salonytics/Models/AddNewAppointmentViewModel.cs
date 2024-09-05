namespace Salonytics.Models
{
    public class AddNewAppointmentViewModel
    {
        public Guid AppointmentId { get; set; }
        public bool IsApproved { get; set; }
        public string FullName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public float Price { get; set; }
        public Guid ServiceId { get; set; }
        public Guid BranchId { get; set; }
        public Guid? StylistId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public int NoOfHeads { get; set; }
        public string Remarks { get; set; }
        public float TotalPrice { get; set; }
         public IEnumerable<TimeSpan> AvailableSlots { get; set; }
         public IEnumerable<TimeSpan> BookedSlots { get; set; }

    }
}
