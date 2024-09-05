using salonytics.Models.Entities;
using Salonytics.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace salonytics.Models
{
    public class ManageStylistsModel
    {
        public Guid StylistId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid BranchId { get; set; }
        public Guid ManagerId { get; set; }
        public double AverageRating { get; set; }

        // Navigation properties
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

        public List<Service>? Services { get; set; }
        public List<Rating>? Ratings { get; set; }
    }
}
