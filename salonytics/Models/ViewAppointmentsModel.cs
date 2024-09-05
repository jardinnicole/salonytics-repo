namespace salonytics.Models
{
    public class ViewAppointmentsModel
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
        public DateTime StartTime { get; set; }
        public int NoOfHeads { get; set; }
        public string Remarks { get; set; }
        public float TotalPrice { get; set; }
    }
}
