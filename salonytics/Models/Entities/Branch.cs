namespace salonytics.Models.Entities
{
    public class Branch
    {
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public string Location { get; set; }
        public int Sales { get; set; }
    }
}