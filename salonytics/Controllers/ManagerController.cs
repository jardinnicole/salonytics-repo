using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models;
using salonytics.Models.Entities;
using Salonytics.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace salonytics.Controllers
{
    public class ManagerController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManagerController(AppDbContext appDbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Manager/RegisterStylist
        [HttpGet]
        public IActionResult RegisterStylist()
        {
            ViewBag.Branches = _appDbContext.Branches.ToList();
            return View();
        }

        // POST: /Manager/RegisterStylist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStylist(StylistRegistrationViewModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress
            };

            var stylist = new Stylist
            {
                EmailAddress = user.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BranchId = model.BranchId,
                Position = model.Position
            };

            _appDbContext.Stylists.Add(stylist);
            await _appDbContext.SaveChangesAsync();
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //ADD STYLIST ACCOUNT
                await _roleManager.CreateAsync(new IdentityRole("Stylist"));

                await _userManager.AddToRoleAsync(user, "Stylist");

                return RedirectToAction("Index", "Home");
            }
            else {
                return View(model);
            }


            
        }

        
    }
}
