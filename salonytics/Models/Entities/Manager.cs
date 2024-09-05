using Salonytics.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace salonytics.Models.Entities
{
    public class Manager
    {
        public Guid ManagerId { get; set; }
        public string ManagerName { get; set; }
        public Guid BranchId { get; set; }

        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }
        public List<Stylist> Stylists { get; set; } = new List<Stylist>();
    }
}
