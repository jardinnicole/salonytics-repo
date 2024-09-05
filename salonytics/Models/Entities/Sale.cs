namespace salonytics.Models.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Service { get; set; }
        public decimal Cost { get; set; }
    }
}
