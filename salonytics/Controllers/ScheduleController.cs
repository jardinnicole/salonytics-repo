using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models.Entities;
using salonytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Salonytics.Helpers;

namespace salonytics.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ScheduleController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Create(Guid stylistId)
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(WeeklyScheduleViewModel model)
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

        public async Task<IActionResult> Index(Guid stylistId)
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
    }
}
