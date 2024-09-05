using Microsoft.AspNetCore.Mvc;
using salonytics.Data;
using Salonytics.Models.Entities;
using Salonytics.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using salonytics.Models;
using System.Diagnostics;
using System.Drawing;

namespace salonytics.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AdminController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult ManageAppointments()
        {
            var appointments = _dbContext.Appointments
                         .ToList();

            return View(appointments);
        }

        public ActionResult DataReports()
        {
            string filePath = @"C:\Users\nicol\OneDrive\Desktop\project\salonytics\salonytics\data_analysis_report.html";
            string imageFolderUrl = "/images/"; // Relative URL path to your images folder

            if (System.IO.File.Exists(filePath))
            {
                string fileContent = System.IO.File.ReadAllText(filePath);

                // Replace image paths to include the image folder URL
                fileContent = fileContent.Replace("src=\"count_appointments_by_date.png\"", $"src=\"{imageFolderUrl}count_appointments_by_date.png\"");
                fileContent = fileContent.Replace("src=\"count_appointments_by_branch.png\"", $"src=\"{imageFolderUrl}count_appointments_by_branch.png\"");
                fileContent = fileContent.Replace("src=\"count_appointments_by_gender.png\"", $"src=\"{imageFolderUrl}count_appointments_by_gender.png\"");
                fileContent = fileContent.Replace("src=\"average_services_per_day.png\"", $"src=\"{imageFolderUrl}average_services_per_day.png\"");

                ViewBag.HtmlContent = fileContent;
            }
            else
            {
                ViewBag.HtmlContent = "<p>HTML file not found.</p>";
            }

            return View();
        }

        [HttpPost]
        public IActionResult ApproveAppointment(Guid appointmentId)
        {
            var appointment = _dbContext.Appointments.Find(appointmentId);
            if (appointment != null)
            {
                appointment.IsApproved = true;
                _dbContext.SaveChanges();
                // Redirect to the appointments page after approval
                return RedirectToAction("ManageAppointments");
            }
            // Handle error: Appointment not found
            TempData["ErrorMessage"] = "Appointment not found.";
            return RedirectToAction("ManageAppointments");
        }

        [HttpGet]
        public IActionResult GetAppointmentDetails(Guid appointmentId)
        {
            var appointment = _dbContext.Appointments
                .Include(a => a.Service)
                .Include(a => a.Branch)
                .Include(a => a.Stylist)
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                // Handle error: Appointment not found
                return NotFound();
            }

            return PartialView("_AppointmentDetails", appointment);
        }
        public async Task<IActionResult> CustomerInfo(string searchString)
        {
            // Use LINQ to get list of customers
            var customers = from c in _dbContext.Customers
                            select c;

            // Filter by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c =>
                    c.LastName.Contains(searchString) ||
                    c.FirstName.Contains(searchString) ||
                    c.CustomerId.Contains(searchString));
            }

            var viewModel = new CustomerListViewModel
            {
                Customers = await customers.ToListAsync(),
                SearchString = searchString
            };

            return View(viewModel);
        }

        // GET: Customer/Details/{id}
        public async Task<IActionResult> CustomerDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

    }
}
