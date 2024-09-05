using Microsoft.AspNetCore.Mvc.Rendering;
using salonytics.Models.Entities;

namespace salonytics.Models
{
    public class StylistIndexViewModel
    {
        public List<StylistViewModel> Stylists { get; set; }
        public List<SelectListItem> Branches { get; set; }
        public List<string> Positions { get; set; }
        public Guid BranchId { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public double? MinRating { get; set; }

        public StylistIndexViewModel()
        {
            Branches = new List<SelectListItem>();
            Positions = new List<string>();
        }

    }
}
