namespace salonytics.Models.Entities
{

    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
