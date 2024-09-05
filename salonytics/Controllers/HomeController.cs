using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using salonytics.Data;
using salonytics.Models;
using salonytics.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace salonytics.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(AppDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Branches()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Action method to redirect to appropriate dashboard based on user role
        public IActionResult Dashboard()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("AdminDashboard");
            }
            else if (User.IsInRole("User"))
            {
                return RedirectToAction("UserDashboard");
            }
            else if (User.IsInRole("Stylist"))
            {
                return RedirectToAction("StylistDashboard");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        public async Task<IActionResult> UserDashboard()
        {
            if (_signInManager.IsSignedIn(User))
            {
                // Get the current user's ID
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Get the customer associated with the user
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == user.Id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                // Get appointments for the customer
                var appointments = await _context.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Branch)
                    .Include(a => a.Stylist)
                    .Where(a => a.CustomerId == customer.CustomerId)
                    .ToListAsync();

                // Pass the list of appointments directly to the view
                return View(appointments);
            }

            return RedirectToAction("Login", "Account"); // Redirect to login page if user is not signed in
        }

        public async Task<IActionResult> PastAppointments()
        {
            if (_signInManager.IsSignedIn(User))
            {
                // Get the current user's ID
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Get the customer associated with the user
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == user.Id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                // Get appointments for the customer from previous dates
                var pastAppointments = await _context.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Branch)
                    .Include(a => a.Stylist)
                    .Where(a => a.CustomerId == customer.CustomerId && a.Date < DateTime.Today)
                    .ToListAsync();

                // Pass the list of past appointments directly to the view
                return View(pastAppointments);
            }

            return RedirectToAction("Login", "Account"); // Redirect to login page if user is not signed in
        }
        // get queue number

        [HttpGet]
        public JsonResult GetQueuePosition(Guid appointmentId)
        {
            var appointment = _context.Appointments
                .Include(a => a.Branch)
                .Include(a => a.Service)
                .SingleOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return Json(new { success = false, message = "Appointment not found" });
            }

            var appointmentsAtSameBranchAndService = _context.Appointments
                .Where(a => a.BranchId == appointment.BranchId && a.ServiceId == appointment.ServiceId && a.AppointmentStatus != "Done")
                .OrderBy(a => a.StartTime)
                .ToList();

            int queuePosition = appointmentsAtSameBranchAndService
                .FindIndex(a => a.AppointmentId == appointmentId) + 1;

            return Json(new { success = true, queuePosition = queuePosition });
        }


        // Action to get service name
        public async Task<IActionResult> GetServiceName(Guid id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound("Service not found.");
            }
            return Json(service.Name);
        }

        // Action to get branch name
        public async Task<IActionResult> GetBranchName(Guid id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound("Branch not found.");
            }
            return Json(branch.BranchName);
        }

        // Action to get stylist name
        public async Task<IActionResult> GetStylistName(Guid id)
        {
            var stylist = await _context.Stylists.FindAsync(id);
            if (stylist == null)
            {
                return NotFound("Stylist not found.");
            }
            return Json(stylist.FirstName + " " + stylist.LastName);
        }

        //     SEND TIP AND RATE STYLIST FOR USER

        [HttpPost]
        public IActionResult SendRating(int appointmentId, int rating)
        {
            // Example: Save rating to database
            var appointment = _context.Appointments.Find(appointmentId);
            if (appointment != null)
            {
                appointment.Rating = rating;
                _context.SaveChanges();
            }
            return Ok(); // Optionally return JSON response
        }

        [HttpPost]
        public IActionResult SendTip(int appointmentId, bool leaveTip, float tipAmount)
        {
            // Example: Save tip information to database
            var appointment = _context.Appointments.Find(appointmentId);
            if (appointment != null && leaveTip)
            {
                appointment.TipAmount = tipAmount;
                _context.SaveChanges();
            }
            return Ok(); // Optionally return JSON response
        }


        [HttpPost]
        public IActionResult ConfirmTipPayment(int appointmentId, string transactionId, string payerId)
        {
            try
            {
                var appointment = _context.Transactions.Find(appointmentId);
                if (appointment == null)
                {
                    return NotFound();
                }

                // Update the appointment with transaction details
                appointment.TransactionId = transactionId;
                appointment.PayerId = payerId;

                _context.Update(appointment);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult RateStylist(int appointmentId, int rating)
        {
            try
            {
                var appointment = _context.Appointments.Find(appointmentId);
                if (appointment == null)
                {
                    return NotFound();
                }

                appointment.Rating = rating;
                _context.Update(appointment);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Stylist")]
        public IActionResult StylistDashboard()
        {
            return View();
        }

        // Action method to retrieve weekly sales data
        public JsonResult GetWeeklySalesData()
        {
            // Simulated data for demonstration purposes
            var data = new List<int> { 500, 600, 700, 550, 680, 750, 720 };
            return Json(data);
        }

        // Action method to retrieve monthly sales data
        public JsonResult GetMonthlySalesData()
        {
            // Simulated data for demonstration purposes
            var data = new List<int> { 1500, 1600, 1700, 1550, 1680, 1750, 1720, 1630, 1610, 1670, 1680, 1700 };
            return Json(data);
        }

        // Action method to retrieve annual sales data
        public JsonResult GetAnnualSalesData()
        {
            // Simulated data for demonstration purposes
            var data = new List<int> { 20000, 25000, 30000, 28000, 32000, 35000, 38000, 40000, 42000, 45000, 48000, 50000 };
            return Json(data);
        }

        // Action method to retrieve top branches data
        public JsonResult GetTopBranchesData()
        {
            // Simulated data for demonstration purposes
            var data = _context.Branches.OrderByDescending(b => b.Sales).Take(5).Select(b => b.BranchName).ToList();
            return Json(data);
        }

        // Action method to retrieve top stylists data
        public JsonResult GetTopStylistsData()
        {
            // Simulated data for demonstration purposes
            var data = _context.Stylists.Take(5).Select(s => s.LastName + "," + s.FirstName).ToList();
            return Json(data);
        }

        // Action method to retrieve top services data
        public JsonResult GetTopServicesData()
        {
            // Simulated data for demonstration purposes
            var data = _context.Services.OrderByDescending(s => s.TotalServices).Take(5).Select(s => s.Name).ToList();
            return Json(data);
        }

        // Action method to retrieve calendar events data
        public JsonResult GetCalendarEvents()
        {
            // Simulated data for demonstration purposes
            var events = new List<object>
            {
                new { title = "Event 1", start = new DateTime(2024, 5, 1).ToString("yyyy-MM-dd") },
                new { title = "Event 2", start = new DateTime(2024, 5, 15).ToString("yyyy-MM-dd") }
                // Add more events as needed
            };
            return Json(events);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
