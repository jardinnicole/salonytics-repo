using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace salonytics.Models.Entities
{
    public class Revenue
    {
        [Key]
        public Guid RevenueId { get; set; }

        public Guid BranchId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public float TotalPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public float TotalPayment { get; set; }
    }
}
