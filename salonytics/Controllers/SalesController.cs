using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models;
using Salonytics.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salonytics.Controllers
{
    public class SalesController : Controller
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var sales = await _context.Sales.ToListAsync();
            return View(sales);
        }

        public async Task<IActionResult> Summary()
        {
            var summary = await _context.Sales
                .GroupBy(s => s.Service)
                .Select(g => new SalesSummaryViewModel
                {
                    Service = g.Key,
                    TotalSales = g.Sum(s => s.Cost),
                    AverageSales = g.Average(s => s.Cost),
                    MaxSale = g.Max(s => s.Cost),
                    MinSale = g.Min(s => s.Cost),
                    Count = g.Count()
                })
                .ToListAsync();

            // Get the highest and lowest sale dates
            var highestSale = summary.OrderByDescending(x => x.TotalSales).FirstOrDefault();
            var lowestSale = summary.OrderBy(x => x.TotalSales).FirstOrDefault();

            // Get the actual dates for the highest and lowest sales
            if (highestSale != null)
            {
                highestSale.Date = await _context.Sales
                    .Where(s => s.Service == highestSale.Service)
                    .OrderByDescending(s => s.Cost)
                    .Select(s => s.Date)
                    .FirstOrDefaultAsync();
            }

            if (lowestSale != null)
            {
                lowestSale.Date = await _context.Sales
                    .Where(s => s.Service == lowestSale.Service)
                    .OrderBy(s => s.Cost)
                    .Select(s => s.Date)
                    .FirstOrDefaultAsync();
            }

            // Pass the highest and lowest sales to the view
            ViewBag.HighestSale = highestSale;
            ViewBag.LowestSale = lowestSale;

            return View(summary);
        }

    }
}
