namespace salonytics.Models
{
    public class EmployeePaymentViewModel
    {
        public Guid StylistId { get; set; }
        public string StylistName { get; set; }
        public float PaymentAmount { get; set; }
    }

    public class BranchDailyReportViewModel
    {
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public DateTime Date { get; set; }
        public float TotalPrice { get; set; }
        public float TotalPayment { get; set; }
        public List<EmployeePaymentViewModel> EmployeePayments { get; set; }
    }
}
