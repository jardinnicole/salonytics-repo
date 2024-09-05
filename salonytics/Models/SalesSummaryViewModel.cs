namespace salonytics.Models
{
    public class SalesSummaryViewModel
    {
        public string Service { get; set; }
        public decimal TotalSales { get; set; }
        public decimal AverageSales { get; set; }
        public decimal MaxSale { get; set; }
        public decimal MinSale { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }

    }
}
