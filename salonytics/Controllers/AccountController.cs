using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using salonytics.Data;
using salonytics.Models.Entities;
using System.Security.Claims;
using System.Threading.Tasks;


namespace salonytics.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(AppDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public IActionResult Register()
        {
            var viewModel = new Customer(); // Use Customer directly as the view model
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserAsync(User).Result;
                viewModel.CustomerId = user.Id;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _dbContext.Customers.AddAsync(customer);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Dashboard", "Home");
            }

            return View(customer);
        }

        [AllowAnonymous]
        public IActionResult ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var info = _signInManager.GetExternalLoginInfoAsync().Result;
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            var result = _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true).Result;
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = new IdentityUser { UserName = email, Email = email };
                    var createResult = _userManager.CreateAsync(user).Result;
                    if (createResult.Succeeded)
                    {
                        var addLoginResult = _userManager.AddLoginAsync(user, info).Result;
                        if (addLoginResult.Succeeded)
                        {
                            _signInManager.SignInAsync(user, isPersistent: false).Wait();
                            return Redirect(returnUrl);
                        }
                    }
                }
                return RedirectToAction("Login");
            }
        }

    }
}
