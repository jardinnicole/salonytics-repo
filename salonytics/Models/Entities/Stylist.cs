using Salonytics.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace salonytics.Models.Entities
{
    public class Stylist
    {
        [Key]
        public Guid StylistId { get; set; }
        public string StylistCode { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        public Guid BranchId { get; set; }

        public string Position { get; set; }

        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

        public List<Service> Services { get; set; } = new List<Service>();

        public List<Rating> Ratings { get; set; } = new List<Rating>();

        public List<StylistSchedule> Schedules { get; set; } = new List<StylistSchedule>();
        public ICollection<StylistService> StylistServices { get; set; }

        [NotMapped]
        public double AverageRating
        {
            get
            {
                if (Ratings != null && Ratings.Count > 0)
                {
                    double totalRating = 0;
                    foreach (var rating in Ratings)
                    {
                        totalRating += rating.Score;
                    }
                    return totalRating / Ratings.Count;
                }
                else
                {
                    return 0; // or any default value if no ratings
                }
            }
        }

        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public int TotalSales { get; set; }

        public Guid FromBranchId { get; set; }
        [ForeignKey("FromBranchId")]

        public Guid ToBranchId { get; set; }
        [ForeignKey("ToBranchId")]

        public decimal Fare { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        
    }
}
