using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models;
using salonytics.Models.Entities;
using salonytics.Services;
using Salonytics.Helpers;
using Salonytics.Models.Entities;

namespace salonytics.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppointmentService _appointmentService;


        public AppointmentController(AppointmentService appointmentService, AppDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _appointmentService = appointmentService;

        }
        // Create GET method
        public async Task<IActionResult> Create(Guid? selectedServiceId)
        {

            var viewModel = await GetScheduleViewModelAsync(selectedServiceId);

            // Pre-select the service if serviceId is provided
            if (selectedServiceId.HasValue)
            {
                viewModel.SelectedServiceId = selectedServiceId.Value;
            }
            else
            {
                // Handle case where no service is pre-selected
                viewModel.SelectedServiceId = null;
            }

            return View(viewModel);
        }

        // Create POST method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {

            if (ModelState.IsValid)
            {
                // Generate the AppointmentCode
                appointment.AppointmentCode = await _appointmentService.GenerateAppointmentCodeAsync(appointment.BranchId, appointment.Date);

                // Add the appointment to the database
                _dbContext.Appointments.Add(appointment);
                _dbContext.SaveChanges();
                return RedirectToAction("Billing", new { id = appointment.AppointmentId });
            }

            // If model is not valid, re-fetch the view model
            var viewModel = await GetScheduleViewModelAsync(appointment.ServiceId);
            return View("Create", viewModel);
        }

        // GET: Appointment/Confirmation/{id}
        public async Task<IActionResult> Confirmation(Guid appointmentId)
        {
            ViewData["SuccessMessage"] = "Appointment booked successfully!";
            var appointment = await _dbContext.Appointments
                .Include(a => a.Service)
                .Include(a => a.Branch)
                .Include(a => a.Stylist)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointment/GetStylists/{branchId}
        [HttpGet]
        public IActionResult GetStylists(Guid branchId)
        {
            var stylists = _dbContext.Stylists
                .Where(s => s.BranchId == branchId)
                .Select(s => new { StylistId = s.StylistId, Name = s.FirstName + " " + s.LastName })
                .ToList();

            return Json(stylists);
        }

        // GET: Appointment/GetServicePrice/{serviceId}
        [HttpGet]
        public async Task<IActionResult> GetServicePrice(Guid serviceId)
        {
            var service = await _dbContext.Services.FirstOrDefaultAsync(s => s.ServiceId == serviceId);
            if (service == null)
            {
                return NotFound();
            }
            return Json(service.Price);
        }

         // GET: Appointment/GetAvailableTimes/{stylistId}/{date}
        [HttpGet]
        public JsonResult GetAvailableTimes(Guid stylistId, DateTime date)
        {
            var appointments = _dbContext.Appointments
                .Where(a => a.StylistId == stylistId && a.Date.Date == date.Date)
                .Select(a => a.StartTime)
                .ToList();

            var availableTimes = new List<string>();
            var startTime = TimeSpan.FromHours(8); // Start time for appointments (e.g., 8:00 AM)
            var endTime = TimeSpan.FromHours(17); // End time for appointments (e.g., 5:00 PM)

            for (var time = startTime; time <= endTime; time = time.Add(TimeSpan.FromHours(1)))
            {
                if (!appointments.Any(a => a == time))
                {
                    availableTimes.Add(time.ToString(@"hh\:mm"));
                }
            }

            return Json(availableTimes);
        }

        // Helper method to get fully booked dates
        private async Task<List<DateTime>> GetFullyBookedDates()
        {
            var fullyBookedDates = await _dbContext.Appointments
                .GroupBy(a => a.Date)
                .Where(g => g.Sum(a => a.StylistId != null ? 1 : 0) >= 4) // Assuming StylistId is not null for each appointment
                .Select(g => g.Key)
                .ToListAsync();

            return fullyBookedDates;
        }

        // Helper method to get ScheduleViewModel
        private async Task<ScheduleViewModel> GetScheduleViewModelAsync(Guid? selectedServiceId)
        {
            // Fetch data from the database
            var services = await _dbContext.Services.ToListAsync();
            var branches = await _dbContext.Branches.ToListAsync();

            // Create the view model
            var viewModel = new ScheduleViewModel
            {
                FullyBookedDates = await GetFullyBookedDates(),
                Appointment = new Appointment(),
                Services = services.Select(s => new SelectListItem
                {
                    Value = s.ServiceId.ToString(),
                    Text = s.Name
                }).ToList(), // Convert to List<SelectListItem> for Services
                SelectedServiceId = selectedServiceId, // Set the selected service ID
                Branches = branches.Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.BranchName
                }).ToList(), // Convert to List<SelectListItem> for Branches
                Stylists = new List<SelectListItem>(), // Populate as needed
                StartTime = new List<SelectListItem>() // Populate as needed
            };

            return viewModel;
        }


        //VIEW APPOINTMENTS

        // GET: Appointments
        public async Task<IActionResult> Appointments()
        {
            return View(await _dbContext.Appointments.ToListAsync());
        }


        // GET: Appointments/Weekly
        public async Task<IActionResult> Weekly()
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(6);

            var appointments = await _dbContext.Appointments
                .Include(a => a.Service)
                .Include(a => a.Stylist)
                .Include(a => a.Branch)
                .Where(a => a.Date >= startOfWeek && a.Date <= endOfWeek)
                .ToListAsync();

            return View("Appointments", appointments);
        }

        // GET: Appointments/Monthly
        public async Task<IActionResult> Monthly()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var appointments = await _dbContext.Appointments
                .Include(a => a.Service)
                .Include(a => a.Stylist)
                .Include(a => a.Branch)
                .Where(a => a.Date >= startOfMonth && a.Date <= endOfMonth)
                .ToListAsync();

            return View("Appointments", appointments);
        }

        // GET: Appointments/Details?id=AppointmentId
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _dbContext.Appointments
                .Include(a => a.Service)
                .Include(a => a.Stylist)
                .Include(a => a.Branch)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return PartialView("_AppointmentDetails", appointment);
        }

        //Get Customer Info if logged in
        [HttpGet]
        public async Task<IActionResult> GetCustomerInfo(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest("Customer Id is required.");
            }

            // Fetch customer info based on customerId
            var customer = _dbContext.Customers.FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var user = await _userManager.FindByIdAsync(customerId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var customerInfo = new
            {
                fullName = customer.FirstName + " " + customer.LastName,
                contactNo = customer.ContactNumber,
                email = user.Email
            };

            return Json(customerInfo);
        }


        // GET: /Appointment/GetServiceName
        public async Task<IActionResult> GetServiceName(Guid id)
        {
            var service = await _dbContext.Services.FindAsync(id);
            return Ok(service?.Name ?? "Service not found");
        }

        // GET: /Appointment/GetStylistName
        public async Task<IActionResult> GetStylistName(Guid id)
        {
            var stylist = await _dbContext.Stylists.FindAsync(id);
            return Ok(stylist != null ? $"{stylist.LastName} {stylist.FirstName}" : "Stylist not found");
        }

        // GET: /Appointment/GetBranchName
        public async Task<IActionResult> GetBranchName(Guid id)
        {
            var branch = await _dbContext.Branches.FindAsync(id);
            return Ok(branch?.BranchName ?? "Branch not found");
        }

        public async Task<IActionResult> ByCustomer()
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

                return View(appointments);
            }

            return Unauthorized();
        }

        // BILLING 

        public IActionResult Billing(Guid id)
        {// Fetch the appointment details
            var appointment = _dbContext.Appointments.FirstOrDefault(a => a.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Fetch the service details to get the service price
            var service = _dbContext.Services.FirstOrDefault(s => s.ServiceId == appointment.ServiceId);
            if (service == null)
            {
                return NotFound();
            }

            // Calculate the reservation fee as 3% of the service price
            var reservationFee = service.Price * 0.03;

            
            int reservationFeeInt = (int)reservationFee;

            // Create the view model with the appointment and reservation fee
            var billingViewModel = new BillingViewModel
            {
                Appointment = appointment,
                ReservationFee = reservationFeeInt,
                TotalPrice = appointment.TotalPrice
            };

            return View(billingViewModel);

        }

        [HttpPost]
        public IActionResult ConfirmPayment(Guid appointmentId, string transactionId, string payerId)
        {
            var appointment = _dbContext.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            // Save the transaction details
            var transaction = new Transaction
            {
                AppointmentId = appointmentId,
                TransactionId = transactionId,
                PayerId = payerId,
                Status = "Completed"
            };
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            // Redirect to confirmation page
            return RedirectToAction("Confirmation", new { appointmentId = appointmentId });
        }
        // GET: Appointment/Edit/5
        public IActionResult Edit(Guid id)
        {
            var appointment = _dbContext.Appointments
                .Include(a => a.Service)
                .Include(a => a.Branch)
                .Include(a => a.Stylist)
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }


            return View(appointment);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(appointment);
                    _dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine(ex.Message);
                    if (!AppointmentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(appointment);
        }

       
        private bool AppointmentExists(Guid id)
        {
            return _dbContext.Appointments.Any(e => e.AppointmentId == id);
        }

        //MANAGE APPOINTMENTS
        public IActionResult Index(Guid? branchId, string status, Guid? serviceId, Guid? stylistId)
        {
            var appointmentsQuery = _dbContext.Appointments.AsQueryable();

            // Apply filters
            if (branchId != null && branchId != Guid.Empty)
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.BranchId == branchId);
            }

            if (!string.IsNullOrEmpty(status))
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.AppointmentStatus == status);
            }

            if (serviceId != null && serviceId != Guid.Empty)
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.ServiceId == serviceId);
            }

            if (stylistId != null && stylistId != Guid.Empty)
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.StylistId == stylistId);
            }

            var appointments = appointmentsQuery.ToList();

            var viewModel = new AppointmentIndexViewModel
            {
                Appointments = appointments,
                Services = _dbContext.Services
                    .Select(s => new SelectListItem
                    {
                        Value = s.ServiceId.ToString(),
                        Text = s.Name
                    }),
                Branches = _dbContext.Branches
                    .Select(b => new SelectListItem
                    {
                        Value = b.BranchId.ToString(),
                        Text = b.BranchName
                    }),
                Stylists = _dbContext.Stylists
                    .Select(s => new SelectListItem
                    {
                        Value = s.StylistId.ToString(),
                        Text = $"{s.FirstName} {s.LastName}"
                    })
            };

            return View(viewModel);
        }
        // GET: Shop/Details/5
        public async Task<IActionResult> Read(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _dbContext.Appointments
                .Include(a => a.Service)
                .Include(a => a.Branch)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }
        // GET: Shop/Edit/5
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _dbContext.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.Services = await _dbContext.Services.ToListAsync();
            ViewBag.Branches = await _dbContext.Branches.ToListAsync();
            ViewBag.Stylists = await _dbContext.Stylists.ToListAsync();

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, [Bind("AppointmentId,AppointmentCode,AppointmentStatus,FullName,ContactNo,Email,ServiceId,StylistId,BranchId,Date,StartTime,NoOfHeads,Remarks")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAppointment = await _dbContext.Appointments.FindAsync(id);

                    if (existingAppointment == null)
                    {
                        return NotFound();
                    }

                    appointment.AppointmentCode = existingAppointment.AppointmentCode;


                    // Preserve the AppointmentCode

                    _dbContext.Entry(existingAppointment).CurrentValues.SetValues(appointment);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Reload necessary data if model state is invalid
            ViewBag.Services = await _dbContext.Services.ToListAsync();
            ViewBag.Branches = await _dbContext.Branches.ToListAsync();
            ViewBag.Stylists = await _dbContext.Stylists.ToListAsync();

            return View(appointment);
        }


        public async Task<IActionResult> Cancel(Guid? id)
        {
            var appointment = _dbContext.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.AppointmentStatus = "Canceled";

                var user = await _userManager.GetUserAsync(User);
                var userRoles = await _userManager.GetRolesAsync(user);
                var userRole = userRoles.FirstOrDefault();

                if (userRole == "User" && (appointment.Date - DateTime.UtcNow).TotalHours < 24)
                {
                    // Add logic to handle the case when cancellation is not allowed
                    ModelState.AddModelError("", "Cancellation is only allowed up to 24 hours before the appointment.");
                    return RedirectToAction("Index"); // Redirect to a suitable view or action
                }
                _dbContext.Appointments.Update(appointment);

                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Appointment cancelled and deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error cancelling appointment.";
            }
            return Redirect(Request.Headers["Referer"].ToString() ?? "/");
        }

        // HOME SERVICE AVAILABILIY

        public IActionResult HomeServiceAvailability(Guid branchId, Guid stylistId)
        {
            var bookedDates = _dbContext.Appointments
                .Where(a => a.BranchId == branchId && a.StylistId == stylistId)
                .Select(a => a.Date.Date)
                .Distinct()
                .ToList();

            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(30);

            var availableDates = new List<DateTime>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!bookedDates.Contains(date))
                {
                    availableDates.Add(date);
                }
            }

            var model = new HomeServiceAvailabilityViewModel
            {
                Branches = GetBranches().ToList(),
                Stylists = new List<SelectListItem>(), // This will be populated via AJAX
                AvailableDates = availableDates,
                SelectedDates = new List<string>()
            };

            return View(model);
        }

        [HttpGet]
        public JsonResult GetStylistsList(Guid branchId)
        {
            var stylists = _dbContext.Stylists
                .Where(s => s.BranchId == branchId)
                .Select(s => new SelectListItem
                {
                    Value = s.StylistId.ToString(),
                    Text = s.FirstName + " " + s.LastName
                })
                .ToList();

            return Json(stylists);
        }

        private IEnumerable<SelectListItem> GetBranches()
        {
            return _dbContext.Branches.Select(b => new SelectListItem
            {
                Value = b.BranchId.ToString(),
                Text = b.BranchName
            }).ToList();
        }

        public IActionResult SetHomeServiceAvailability(HomeServiceAvailabilityViewModel model)
        {
            if (model.SelectedDates != null && model.SelectedDates.Count > 0)
            {
                var selectedDates = model.SelectedDates.Select(date => DateTime.Parse(date)).ToList();

                foreach (var date in selectedDates)
                {
                    if (!_dbContext.HomeServiceAvailabilities.Any(h => h.Date == date && h.BranchId == model.BranchId && h.StylistId == model.StylistId))
                    {
                        _dbContext.HomeServiceAvailabilities.Add(new HomeServiceAvailability
                        {
                            Id = Guid.NewGuid(),
                            Date = date,
                            BranchId = model.BranchId,
                            StylistId = model.StylistId
                        });
                    }
                }

                _dbContext.SaveChanges();
            }
            return RedirectToAction("HomeServiceAvailability", new { branchId = model.BranchId, stylistId = model.StylistId });
        }

    }
}
