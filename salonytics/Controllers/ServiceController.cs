using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models.Entities;
using salonytics.Models;

namespace salonytics.Controllers
{
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Stylists/ManageServices
        public IActionResult ManageServices()
        {
            var viewModel = new StylistServiceViewModel
            {
                Services = _context.Services.Select(s => new SelectListItem
                {
                    Value = s.ServiceId.ToString(),
                    Text = s.Name
                }).ToList()
            };

            viewModel.Stylist = new Stylist(); // Initialize the stylist object

            return View(viewModel);
        }

        // POST: Stylists/ManageServices
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageServices(StylistServiceViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Stylist);
                await _context.SaveChangesAsync();

                if (viewModel.SelectedServices != null)
                {
                    foreach (var serviceId in viewModel.SelectedServices)
                    {
                        var stylistService = new StylistService { StylistId = viewModel.Stylist.StylistId, ServiceId = serviceId };
                        _context.Add(stylistService);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            viewModel.Services = _context.Services.Select(s => new SelectListItem
            {
                Value = s.ServiceId.ToString(),
                Text = s.Name
            }).ToList();

            return View(viewModel);
        }


        // GET: Stylists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stylist = await _context.Stylists
                .Include(s => s.StylistServices)
                .ThenInclude(ss => ss.Service)
                .FirstOrDefaultAsync(s => s.StylistId == id);

            if (stylist == null)
            {
                return NotFound();
            }

            var viewModel = new StylistServiceViewModel
            {
                Stylist = stylist,
                SelectedServices = stylist.StylistServices.Select(ss => ss.ServiceId).ToList(),
                Services = _context.Services.Select(s => new SelectListItem
                {
                    Value = s.ServiceId.ToString(),
                    Text = s.Name
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Stylists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StylistServiceViewModel viewModel)
        {
            if (id != viewModel.Stylist.StylistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Stylist);

                    // Remove existing services
                    var existingStylistServices = _context.StylistServices.Where(ss => ss.StylistId == viewModel.Stylist.StylistId);
                    _context.StylistServices.RemoveRange(existingStylistServices);

                    // Add selected services
                    if (viewModel.SelectedServices != null)
                    {
                        foreach (var serviceId in viewModel.SelectedServices)
                        {
                            var newStylistService = new StylistService { StylistId = viewModel.Stylist.StylistId, ServiceId = serviceId };
                            _context.Add(newStylistService);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StylistExists(viewModel.Stylist.StylistId))
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

            viewModel.Services = _context.Services.Select(s => new SelectListItem
            {
                Value = s.ServiceId.ToString(),
                Text = s.Name
            }).ToList();

            return View(viewModel);
        }

        private bool StylistExists(Guid id)
        {
            return _context.Stylists.Any(e => e.StylistId == id);
        }
    }
}
