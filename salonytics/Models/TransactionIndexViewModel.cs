using Microsoft.AspNetCore.Mvc.Rendering;
using salonytics.Models.Entities;

namespace salonytics.Models
{
    public class TransactionIndexViewModel
    {
        public Guid? AppointmentId { get; set; }
        public string TransactionId { get; set; }
        public string PayerId { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }

        public List<Transaction> Transactions { get; set; }

        public List<SelectListItem> Appointments { get; set; }

        public TransactionIndexViewModel()
        {
            Transactions = new List<Transaction>();
            Appointments = new List<SelectListItem>();
        }
    }
}
