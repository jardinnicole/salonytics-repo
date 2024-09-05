using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace salonytics.Models
{
    public class StylistBranchAssignmentViewModel
    {
    
    public Guid StylistId { get; set; }

    [Display(Name = "Stylist Name")]
    public string StylistName { get; set; }

    [Display(Name = "From Branch")]
    public Guid FromBranchId { get; set; }

    public string FromBranchName { get; set; }

    [Display(Name = "To Branch")]
    [Required(ErrorMessage = "Please select a branch.")]
    public Guid ToBranchId { get; set; }

    public decimal? Fare { get; set; }

    [Display(Name = "From Date")]
    [Required(ErrorMessage = "Please select a from date.")]
    public DateTime FromDate { get; set; }

    [Display(Name = "To Date")]
    [Required(ErrorMessage = "Please select a to date.")]
    public DateTime ToDate { get; set; }

    public List<SelectListItem>? Branches { get; set; }

    }
}
