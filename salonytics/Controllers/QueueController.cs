using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models;
using salonytics.Models.Entities;
using salonytics.Services;
using static salonytics.Controllers.QueueController;

namespace salonytics.Controllers
{
    public class QueueController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly QueueService _queueService;
        public QueueController(AppDbContext dbContext, QueueService queueService)
        {
            _dbContext = dbContext;
            _queueService = queueService;
        }

        //this method populates the queue table with data from the appt table
        public async Task<IActionResult> PopulateQueues()
        {
            await _queueService.PopulateQueuesFromAppointmentsAsync();
            return RedirectToAction("Index");
        }

        // GET: Queue
        public async Task<IActionResult> Index()
        {
            await _queueService.PopulateQueuesFromAppointmentsAsync();

            var queues = await _dbContext.Queues
                .Include(q => q.Appointment)
                .ToListAsync();
            return View(queues);
        }

        // GET: Queue/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var queue = await _dbContext.Queues
                .Include(q => q.Appointment)
                .FirstOrDefaultAsync(m => m.QueueId == id);
            if (queue == null)
            {
                return NotFound();
            }
            return View(queue);
        }

        // GET: Queue/Create
        public IActionResult Create()
        {
            ViewData["AppointmentId"] = new SelectList(_dbContext.Appointments, "AppointmentId", "FullName");
            return View();
        }

        // POST: Queue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QueueId,AppointmentId,Position,Status")] Queue queue)
        {
            if (ModelState.IsValid)
            {
                await _queueService.CreateQueueAsync(queue);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentId"] = new SelectList(_dbContext.Appointments, "AppointmentId", "FullName", queue.AppointmentId);
            return View(queue);
        }
    }
}
