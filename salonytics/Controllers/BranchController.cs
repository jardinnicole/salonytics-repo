using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models;
using salonytics.Models.Entities;

namespace salonytics.Controllers
{
    public class BranchController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public BranchController(AppDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var branches = await _dbContext.Branches.ToListAsync();
            var model = new BranchIndexViewModel
            {
                Branches = branches
            };

            return View(model);
        }
        public async Task<IActionResult> BranchDailyReport(Guid branchId, DateTime date)
        {
            // Get appointments for the specified branch and date
            var appointments = await _dbContext.Appointments
                .Where(a => a.BranchId == branchId && a.Date.Date == date.Date && a.AppointmentStatus == "Done")
                .ToListAsync();

            // Get the branch details
            var branch = await _dbContext.Branches.FindAsync(branchId);
            if (branch == null)
            {
                return NotFound("Branch not found.");
            }

            // Get the employees (stylists) for the branch
            var employees = await _dbContext.Stylists
                .Where(s => s.BranchId == branchId)
                .ToListAsync();

            // Calculate total price and total payment
            var totalPrice = appointments.Sum(a => a.TotalPrice);
            var totalPayment = totalPrice / 5; // 20% of total price

            // Calculate payment per employee
            var employeePayments = new List<EmployeePaymentViewModel>();
            if (employees.Count > 0)
            {
                var paymentPerEmployee = totalPayment / employees.Count;
                foreach (var employee in employees)
                {
                    employeePayments.Add(new EmployeePaymentViewModel
                    {
                        StylistId = employee.StylistId,
                        StylistName = $"{employee.FirstName} {employee.LastName}",
                        PaymentAmount = paymentPerEmployee
                    });
                }
            }

            // Create and save the revenue record
            var revenue = new Revenue
            {
                RevenueId = Guid.NewGuid(),
                BranchId = branchId,
                Date = date,
                TotalPrice = totalPrice,
                TotalPayment = totalPayment
            };

            _dbContext.Revenues.Add(revenue);
            await _dbContext.SaveChangesAsync();

            var viewModel = new BranchDailyReportViewModel
            {
                BranchName = branch.BranchName,
                Date = date,
                TotalPrice = totalPrice,
                TotalPayment = totalPayment,
                EmployeePayments = employeePayments
            };

            return View(viewModel);
        }



    }
}
