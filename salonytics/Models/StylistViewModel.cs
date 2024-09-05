namespace salonytics.Models
{

    public class StylistViewModel
    {

        public Guid StylistId { get; set; }
        public string StylistCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } // New property to hold the branch name
        public string Position { get; set; }
        public double AverageRating { get; set; }
        public decimal TotalSales { get; set; }
    }
}
