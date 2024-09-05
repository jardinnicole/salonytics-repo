using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Salonytics.Models.Entities;
using salonytics.Areas.Identity;

public class HeaderController : Controller
{
    private readonly NotificationService _notificationService;
    private readonly UserManager<ApplicationUser> _userManager;

    public HeaderController(NotificationService notificationService, UserManager<ApplicationUser> userManager)
    {
        _notificationService = notificationService;
        _userManager = userManager;
    }

    public async Task<IActionResult> GetNotifications()
    {
        // Get the logged-in user's Id
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { message = "No notifications", data = new List<object>() });
        }

        // Get the email from UserManager using the userId
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || string.IsNullOrEmpty(user.Email))
        {
            return Json(new { message = "No notifications", data = new List<object>() });
        }

        var userEmail = user.Email;

        // Determine if the user is a customer or a stylist and get their notifications
        List<Appointment> notifications;
        if (User.IsInRole("Customer"))
        {
            notifications = _notificationService.GetCustomerNotifications(userEmail);
        }
        else if (User.IsInRole("Stylist"))
        {
            notifications = _notificationService.GetStylistNotifications(userEmail);
        }
        else
        {
            return Json(new { message = "No notifications", data = new List<object>() });
        }

        // Mark notifications as sent and return the notification data
        foreach (var appointment in notifications)
        {
            _notificationService.MarkNotificationAsSent(appointment);
        }

        return Json(notifications.Select(a => new {
            FullName = a.FullName,
            StartTime = a.StartTime.ToString(),
            AppointmentCode = a.AppointmentCode
        }));
    }
}
