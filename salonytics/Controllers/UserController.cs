using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class UserController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public UserController(AppDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var stylists = _dbContext.Stylists.Include(s => s.Ratings).ToList();
            return View(stylists);
        }

        public IActionResult MoreFaqs()
        {
            return View();
        }

        public IActionResult UserLocation()
        {
            return View();
        }

        public async Task<IActionResult> Appointments()
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
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == user.Id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                // Get appointments for the customer
                var appointments = await _dbContext.Appointments
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

        public async Task<IActionResult> History()
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
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == user.Id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                // Get appointments for the customer from previous dates
                var pastAppointments = await _dbContext.Appointments
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


        public ActionResult StylistList() {
            var stylists = _dbContext.Stylists.Include(s => s.Ratings).ToList();
            return View(stylists);
        }

        public ActionResult StylistDetails(Guid? id)
        {
            if (id == null)
            {
                return BadRequest(); // For 400 Bad Request
            }

            Stylist stylist = _dbContext.Stylists
                .Include(s => s.Ratings)
                .FirstOrDefault(s => s.StylistId == id);

            if (stylist == null)
            {
                return NotFound();
            }

            return View(stylist);
        }

        public ActionResult RateStylist(Guid? id)
        {
            Stylist stylist = _dbContext.Stylists.Find(id);
            if (stylist == null)
            {
                return NotFound();
            }

            ViewBag.StylistId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RateStylist([Bind("StylistId,Score,Comment")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                rating.Date = DateTime.Now;
                _dbContext.Ratings.Add(rating);
                _dbContext.SaveChanges();
                return RedirectToAction("StylistDetails", new { id = rating.StylistId });
            }

            ViewBag.StylistId = rating.StylistId;
            return View(rating);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var customer = await _dbContext.Customers
                .Include(c => c.Appointments)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            var viewModel = new CustomerDetailsViewModel
            {
                Customer = customer,
                Appointments = customer.Appointments.ToList()
            };

            return View(viewModel);
        }

        //SERVICES AVAILED 
        /*
        public IActionResult ServicesAvailed(Guid customerId)
        {
            var servicesAvailed = _dbContext.ServicesAvailed
                .Where(s => s.CustomerId == customerId)
                .ToList();

            var viewModel = new CustomerServiceAvailedViewModel
            {
                CustomerId = customerId,
                ServicesAvailed = servicesAvailed,
            };

            return View(viewModel);
        } */
    }
}
