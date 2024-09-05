using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models;
using Salonytics.Models;
using Salonytics.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salonytics.Controllers
{
    public class TransactionController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index(TransactionIndexViewModel model)
        {
            var transactions = _context.Transactions.Include(t => t.Appointment).AsQueryable();

            if (model.AppointmentId.HasValue)
            {
                transactions = transactions.Where(t => t.AppointmentId == model.AppointmentId);
            }

            if (!string.IsNullOrEmpty(model.TransactionId))
            {
                transactions = transactions.Where(t => t.TransactionId.Contains(model.TransactionId));
            }

            if (!string.IsNullOrEmpty(model.PayerId))
            {
                transactions = transactions.Where(t => t.PayerId.Contains(model.PayerId));
            }

            if (!string.IsNullOrEmpty(model.Status))
            {
                transactions = transactions.Where(t => t.Status.Contains(model.Status));
            }

            if (model.CreatedAt.HasValue)
            {
                transactions = transactions.Where(t => t.CreatedAt.Date == model.CreatedAt.Value.Date);
            }

            model.Transactions = await transactions.ToListAsync();

            // Populate Appointments dropdown
            model.Appointments = await _context.Appointments
                .Select(a => new SelectListItem
                {
                    Value = a.AppointmentId.ToString(),
                    Text = $"Appointment ID: {a.AppointmentId} - {a.Date}"
                })
                .ToListAsync();

            return View(model);
        }

        // GET: Transaction/Details/{id} 
        // GET: Transaction/Details/{id}
        // GET: Transaction/Details/{id}
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Appointment)
                    .ThenInclude(a => a.Stylist) // Include the Stylist related to the Appointment
                .Include(t => t.Appointment)
                    .ThenInclude(a => a.Service) // Include the Service related to the Appointment
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }
    }
}
