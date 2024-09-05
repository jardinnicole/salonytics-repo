namespace salonytics.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string TransactionId { get; set; }
        public string PayerId { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

    }

}
