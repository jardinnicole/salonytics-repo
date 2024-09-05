using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace salonytics.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Shop/Index
        public async Task<IActionResult> Index()
        {
            var inventories = await _context.Inventories.ToListAsync();
            return View(inventories);
        }

        // GET: Shop/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _context.Inventories.FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        public IActionResult Purchase(int itemId, int quantity)
        {
            var item = _context.Inventories.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                return NotFound();
            }

            decimal totalPrice = item.Price * quantity;

            // Create a new order
            var order = new Order
            {
                // Populate other properties of the order as needed
                OrderDate = DateTime.UtcNow,
            
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // Save changes to get the OrderId

            // Create order detail
            var orderDetail = new OrderDetail
            {
                OrderId = order.Id, // Use the OrderId from the created order
                InventoryId = itemId,
                Quantity = quantity,
                Price = totalPrice
            };

            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();

            return RedirectToAction("PurchaseConfirmed", new { id = orderDetail.Id });
        }

        public async Task<IActionResult> PurchaseConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }


       

        // POST: Shop/ConfirmPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int orderId, string transactionId, string payerId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Inventory)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.TransactionId = transactionId;
            order.PayerId = payerId;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return View("ConfirmPayment", order);
        }
    }
}
