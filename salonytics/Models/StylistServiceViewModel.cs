using Microsoft.AspNetCore.Mvc.Rendering;
using salonytics.Models.Entities;

namespace salonytics.Models
{
    public class StylistServiceViewModel
    {      
            public Stylist Stylist { get; set; }
            public List<Guid> SelectedServices { get; set; }
            public List<SelectListItem> Services { get; set; }
       
    }
}
