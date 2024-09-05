using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models;
using salonytics.Models.Entities;
using System;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Salonytics.Models.Entities;
using salonytics.Services;

namespace salonytics.Controllers
{
    public class StylistController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly StylistCodeService _stylistCodeService;


        public StylistController(StylistCodeService stylistCodeService, AppDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _stylistCodeService = stylistCodeService;

        }



        public async Task<IActionResult> Index(string branchId, string lastName, string position, double? minRating)
        {
            await _stylistCodeService.UpdateStylistCodesAsync();

            var stylists = _dbContext.Stylists
                .Include(s => s.Branch)
                .Include(s => s.Ratings)
                .AsQueryable();

        
            foreach (var stylist in stylists)
            {
                stylist.TotalSales = _dbContext.Appointments
                .Count(a => a.StylistId == stylist.StylistId && a.AppointmentStatus == "Done");

                await _dbContext.SaveChangesAsync();
            }

            // Filter by branchId
            if (!string.IsNullOrEmpty(branchId))
            {
                stylists = stylists.Where(s => s.BranchId.ToString() == branchId);
            }

            // Filter by lastName
            if (!string.IsNullOrEmpty(lastName))
            {
                stylists = stylists.Where(s => s.LastName.Contains(lastName));
            }

            // Filter by position
            if (!string.IsNullOrEmpty(position))
            {
                stylists = stylists.Where(s => s.Position.Contains(position));
            }

            // Filter by minRating
            if (minRating.HasValue)
            {
                stylists = stylists.Where(s => s.AverageRating >= minRating.Value);
            }

            
            var viewModel = new StylistIndexViewModel
            {
                Stylists = stylists.Select(s => new StylistViewModel
                {
                    StylistId = s.StylistId,
                    StylistCode = s.StylistCode,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    EmailAddress = s.EmailAddress,
                    BranchId = s.BranchId,
                    BranchName = s.Branch.BranchName,
                    Position = s.Position,
                    AverageRating = s.AverageRating,
                    TotalSales = (int)s.TotalSales

                }).ToList(),
                Positions = await _dbContext.Stylists.Select(s => s.Position).Distinct().ToListAsync(),
                Branches = await _dbContext.Branches.Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.BranchName
                }).ToListAsync()
            };



            return View(viewModel);
        }

        // CRUD STYLIST
        // GET: Stylist/GetBranchNameById/{id}
        [HttpGet]
        public async Task<IActionResult> GetBranchNameById(Guid id)
        {
            var branch = await _dbContext.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }

            return Ok(branch.BranchName);
        }


        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stylist = await _dbContext.Stylists
                .Include(s => s.Branch)
                .Include(s => s.Ratings)
                .FirstOrDefaultAsync(m => m.StylistId == id);

            if (stylist == null)
            {
                return NotFound();
            }

            return View(stylist);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stylist = await _dbContext.Stylists.FindAsync(id);

            if (stylist == null)
            {
                return NotFound();
            }

            // Populate ViewData with Branches for the dropdown
            ViewData["Branches"] = await _dbContext.Branches.Select(b => new SelectListItem
            {
                Value = b.BranchId.ToString(),
                Text = b.BranchName
            }).ToListAsync();

            return View(stylist);
        }

        // POST: Stylist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("StylistId,FirstName,LastName,EmailAddress,BranchId,Position")] Stylist stylist)
        {
            if (id != stylist.StylistId)
            {
                return NotFound();
            }

            _dbContext.Update(stylist);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stylist = await _dbContext.Stylists
                .Include(s => s.Branch)
                .FirstOrDefaultAsync(m => m.StylistId == id);
            if (stylist == null)
            {
                return NotFound();
            }

            return View(stylist);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var stylist = await _dbContext.Stylists.FindAsync(id);
            _dbContext.Stylists.Remove(stylist);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StylistExists(Guid id)
        {
            return _dbContext.Stylists.Any(e => e.StylistId == id);
        }


        public IActionResult AddStylist()
        {
            ViewBag.Branches = _dbContext.Branches.ToList();

            // Fetch services and convert to SelectList
            var services = _dbContext.Services
                .Select(s => new SelectListItem { Value = s.ServiceId.ToString(), Text = s.Name })
                .ToList();

            ViewBag.Services = services;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStylist(Stylist stylist)
        {
            if (ModelState.IsValid)
            {
                stylist.StylistId = Guid.NewGuid();
                _dbContext.Stylists.Add(stylist);
                _dbContext.SaveChanges();
                return RedirectToAction("ViewStylist", "Stylist"); // Replace with your desired action
            }

            ViewBag.Branches = _dbContext.Branches.ToList();
            var services = _dbContext.Services
                .Select(s => new SelectListItem { Value = s.ServiceId.ToString(), Text = s.Name })
                .ToList();

            ViewBag.Services = services;

            return View(stylist);
        }


        public async Task<IActionResult> GetServiceName(Guid serviceId)
        {
            var serviceName = await _dbContext.Services
                .Where(s => s.ServiceId == serviceId)
                .Select(s => s.Name)
                .FirstOrDefaultAsync();

            return Json(new { serviceName });
        }

        public async Task<IActionResult> ViewStylists()
        {
            var stylists = await _dbContext.Stylists
                .Include(s => s.Branch)
                .Select(s => new StylistViewModel
                {
                    StylistId = s.StylistId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    BranchName = s.Branch.BranchName,
                    AverageRating = s.AverageRating
                })
                .ToListAsync();

            var branches = await _dbContext.Branches.ToListAsync();
            ViewBag.Branches = branches.Select(b => new SelectListItem { Value = b.BranchId.ToString(), Text = b.BranchName }).ToList();

            return View(stylists);
        }

        [HttpGet]
        public ActionResult ManageStylists(Guid? id)
        {
            Stylist model;
            if (id.HasValue)
            {
                model = _dbContext.Stylists.FirstOrDefault(s => s.StylistId == id.Value);
                if (model == null)
                {
                    return NotFound();
                }
            }
            else
            {
                model = new Stylist();
            }

            // Populate branches
            ViewBag.Branches = _dbContext.Branches.ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStylist(Stylist model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageStylists", model);
            }

            if (model.StylistId == Guid.Empty)
            {
                // Add new stylist
                model.StylistId = Guid.NewGuid();
                _dbContext.Stylists.Add(model);
            }
            else
            {
                // Update existing stylist
                var existingStylist = _dbContext.Stylists.FirstOrDefault(s => s.StylistId == model.StylistId);
                if (existingStylist != null)
                {
                    existingStylist.FirstName = model.FirstName;
                    existingStylist.LastName = model.LastName;
                    existingStylist.BranchId = model.BranchId;
                    // Do not update AverageRating here
                }
            }

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetManagersByBranch(Guid branchId)
        {
            var managers = _dbContext.Managers.Where(m => m.BranchId == branchId)
                                   .Select(m => new { Value = m.ManagerId, Text = m.ManagerName })
                                   .ToList();
            return Json(managers);
        }

        // STYLIST BRANCH ASSIGNMENT

        [HttpGet]
        public IActionResult AssignBranch(Guid id)
        {
            var stylist = _dbContext.Stylists
        .SingleOrDefault(s => s.StylistId == id);

            if (stylist == null)
            {
                return NotFound();
            }

            var fromBranch = _dbContext.Branches
                .SingleOrDefault(b => b.BranchId == stylist.BranchId);

            var viewModel = new StylistBranchAssignmentViewModel
            {
                StylistId = stylist.StylistId,
                StylistName = stylist.FirstName + " " + stylist.LastName,
                FromBranchId = stylist.BranchId,
                FromBranchName = fromBranch?.BranchName ?? "Unknown",
                Branches = _dbContext.Branches.Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.BranchName
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AssignBranch(StylistBranchAssignmentViewModel viewModel)
        {
            
            if (viewModel.FromDate > viewModel.ToDate)
            {
                ModelState.AddModelError("FromDate", "From Date cannot be later than To Date.");
            }

            if (ModelState.IsValid)
            {
                viewModel.Branches = _dbContext.Branches.Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.BranchName
                }).ToList();



                return View(viewModel);
            }

            // Retrieve and update the stylist details
            var stylist = _dbContext.Stylists.Find(viewModel.StylistId);
            if (stylist == null)
            {
                return NotFound();
            }

            stylist.BranchId = viewModel.ToBranchId; // Update the current branch to the new branch
            stylist.ToBranchId = viewModel.ToBranchId;
            stylist.Fare = _dbContext.BranchFares
                .Where(f => f.FromBranchId == viewModel.FromBranchId && f.ToBranchId == viewModel.ToBranchId)
                .Select(f => f.Fare)
                .FirstOrDefault();
            stylist.FromDate = viewModel.FromDate;
            stylist.ToDate = viewModel.ToDate;

            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult GetFare(Guid fromBranchId, Guid toBranchId)
        {
            try
            {
                // Fetch the fare from the database
                var fare = _dbContext.BranchFares
                    .Where(f => f.FromBranchId == fromBranchId && f.ToBranchId == toBranchId)
                    .Select(f => f.Fare)
                    .FirstOrDefault();

                // Return the fare, or a 404 if not found
                if (fare == default)
                {
                    return NotFound("Fare not found for the selected branches.");
                }

                return Json(fare);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                // Consider using a logging framework like Serilog, NLog, etc.
                // _logger.LogError(ex, "Error fetching fare");
                return StatusCode(500, "Internal server error");
            }
        }


        // STYLIST VIEW - APPOINTMENTSm

        public async Task<IActionResult> StylistAppointment()
        {
            if (_signInManager.IsSignedIn(User))
            {
                // Get the current user's ID
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Get the stylist associated with the user
                var stylist = await _dbContext.Stylists.FirstOrDefaultAsync(s => s.EmailAddress == user.Email);
                if (stylist == null)
                {
                    return NotFound("Stylist not found.");
                }

                // Get appointments for the stylist
                var appointments = await _dbContext.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Branch)
                    .Where(a => a.StylistId == stylist.StylistId)
                    .OrderBy(a => a.Date)
                    .ThenBy(a => a.StartTime)
                    .ToListAsync();

                // Pass appointments to the view
                return View(appointments);
            }

            return RedirectToAction("Login", "Account");
        }

        // MANAGE SCHEDULE

        [HttpGet]
        public IActionResult EditSchedule(Guid stylistId)
        {
            var stylist = _dbContext.Stylists
                .SingleOrDefault(s => s.StylistId == stylistId);

            if (stylist == null)
            {
                // Add logging to see the stylistId being passed and check if stylist is null
                Console.WriteLine($"Stylist not found for StylistId: {stylistId}");
                return NotFound();
            }

            var model = new WeeklyScheduleViewModel
            {
                StylistId = stylist.StylistId,
                StylistName = $"{stylist.FirstName} {stylist.LastName}",
                DailySchedules = Enum.GetValues(typeof(DayOfWeek))
                    .Cast<DayOfWeek>()
                    .Select(day => new DailyScheduleViewModel { DayOfWeek = day })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditSchedule(WeeklyScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var dailySchedule in model.DailySchedules)
                {
                    foreach (var schedule in dailySchedule.Schedules)
                    {
                        schedule.ScheduleId = Guid.NewGuid();
                        schedule.StylistId = model.StylistId;
                        _dbContext.StylistSchedules.Add(schedule);
                    }
                }

                _dbContext.SaveChanges();
                return RedirectToAction("Index", new { stylistId = model.StylistId });
            }

            return View(model);
        }

        public async Task<IActionResult> ViewSchedule(Guid stylistId)
        {
            var stylist = await _dbContext.Stylists
                .Include(s => s.Schedules)
                .FirstOrDefaultAsync(s => s.StylistId == stylistId);

            if (stylist == null)
            {
                return NotFound();
            }

            var model = new WeeklyScheduleViewModel
            {
                StylistId = stylist.StylistId,
                StylistName = $"{stylist.FirstName} {stylist.LastName}",
                DailySchedules = Enum.GetValues(typeof(DayOfWeek))
                    .Cast<DayOfWeek>()
                    .Select(day => new DailyScheduleViewModel
                    {
                        DayOfWeek = day,
                        Schedules = stylist.Schedules.Where(s => s.DayOfWeek == day).ToList()
                    })
                    .ToList()
            };

            return View(model);
        }

        // transaction

        public async Task<IActionResult> StylistTransaction(Guid stylistId)
        {
            // Get the current user's ID
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Get the stylist associated with the user
            var stylist = await _dbContext.Stylists
                .Include(s => s.Appointments)
                .ThenInclude(a => a.Service)
                .FirstOrDefaultAsync(s => s.EmailAddress == user.Email);

            if (stylist == null)
            {
                return NotFound("Stylist not found.");
            }

            // Filter done appointments for the stylist
            var doneAppointments = stylist.Appointments
                .Where(a => a.AppointmentStatus == "Done")
                .ToList();

            var today = DateTime.Today;
            var revenue = await _dbContext.Revenues
                .FirstOrDefaultAsync(r => r.BranchId == stylist.BranchId && r.Date == today);

            // Create the view model
            var model = new StylistTransactionViewModel
            {
                StylistId = stylist.StylistId,
                StylistName = $"{stylist.FirstName} {stylist.LastName}",
                DoneAppointments = doneAppointments,
                PaymentAmount = revenue != null ? revenue.TotalPayment / _dbContext.Stylists.Count(s => s.BranchId == stylist.BranchId) : 0

            };

            return View(model);
        }



    }
}
