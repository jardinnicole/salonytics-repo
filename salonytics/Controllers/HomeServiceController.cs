using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models.Entities;
using Salonytics.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace salonytics.Controllers
{
    public class HomeServiceController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;


        public HomeServiceController(AppDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;

        }

        // GET: HomeService/Book
        public async Task<IActionResult> Book()
        {
            var today = DateTime.Today;
            var availability = await _dbContext.HomeServiceAvailabilities
                .Where(hsa => hsa.Date >= today && hsa.Date <= today.AddDays(30))
                .ToListAsync(); // This should give you a List<HomeServiceAvailability>

            // Use LINQ Any method
            if (availability.Any())
            {
                // Availability exists
                return View(availability);
            }
            else
            {
                // No availability
                TempData["Message"] = "No home service availability found for the next 30 days.";
                return View(availability);
            }
        }

        // POST: HomeService/BookAppointment
        [HttpPost]
        public async Task<IActionResult> BookAppointment(
     DateTime date,
     Guid branchId,
     Guid? stylistId,
     Guid serviceId,
     TimeSpan startTime,
     int noOfHeads,
     string fullName,
     string contactNo,
     string email,
     string? remarks,
     float totalPrice,
     float? tipAmount,
     int? rating)
        {
            // Get the current user's ID
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == user.Id);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            // Check if the selected date and stylist are available
            var availability = await _dbContext.HomeServiceAvailabilities
                .FirstOrDefaultAsync(hsa => hsa.Date == date && hsa.BranchId == branchId && hsa.StylistId == stylistId);

            if (availability == null)
            {
                TempData["ErrorMessage"] = "The selected date or stylist is not available.";
                return RedirectToAction("Book");
            }

            // Create a new appointment
            var appointment = new Appointment
            {
                AppointmentId = Guid.NewGuid(),
                Date = date,
                BranchId = branchId,
                StylistId = stylistId,
                CustomerId = customer.CustomerId,
                ServiceId = serviceId,
                StartTime = startTime,
                NoOfHeads = noOfHeads,
                FullName = fullName,
                ContactNo = contactNo,
                Email = email,
                Remarks = remarks,
                TotalPrice = totalPrice,
                TipAmount = tipAmount,
                Rating = rating,
                AppointmentStatus = "Pending", // Default status
            };

            // Add the appointment to the database
            _dbContext.Appointments.Add(appointment);
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Appointment booked successfully!";
            return RedirectToAction("Index", "Home"); // Redirect to a confirmation or home page
        }
    }
}
