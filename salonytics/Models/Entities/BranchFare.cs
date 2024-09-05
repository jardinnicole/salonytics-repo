using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace salonytics.Models.Entities
{
    public class BranchFare
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FromBranchId { get; set; }
        public Guid ToBranchId { get; set; }
        public decimal Fare { get; set; }

    }
}
