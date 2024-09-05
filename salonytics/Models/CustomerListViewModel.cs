using salonytics.Models.Entities;


namespace salonytics.Models
{
    public class CustomerListViewModel
    {
        public List<Customer> Customers { get; set; }
        public string SearchString { get; set; }
    }
}
