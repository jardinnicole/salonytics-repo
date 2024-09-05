using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using salonytics.Models;
using salonytics.Models.Entities;
using Salonytics.Models.Entities;

namespace salonytics.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HomeServiceAvailability> HomeServiceAvailabilities { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Stylist> Stylists { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Revenue> Revenues { get; set; }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchFare> BranchFares { get; set; }

        public DbSet<Manager> Managers { get; set; }
        public DbSet<Queue> Queues { get; set; }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<StylistSlot> StylistSlots { get; set; }

        public DbSet<StylistSchedule> StylistSchedules { get; set; }
        public DbSet<StylistService> StylistServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }



    }

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      }
   
}
