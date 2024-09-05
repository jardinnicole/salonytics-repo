using salonytics.Data;
using Salonytics.Models.Entities;

public class NotificationService
{
    private readonly AppDbContext _dbContext;

    public NotificationService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Appointment> GetCustomerNotifications(string customerEmail)
    {
        var currentTime = DateTime.Now;
        var notifyTime = currentTime.AddHours(3);

        // Fetch appointments for the customer happening within the next 3 hours
        var upcomingAppointments = _dbContext.Appointments
            .Where(a => a.Email == customerEmail
                        && a.Date == currentTime.Date
                        && a.StartTime >= currentTime.TimeOfDay
                        && a.StartTime <= notifyTime.TimeOfDay
                        && !a.NotificationSent)
            .ToList();

        return upcomingAppointments;
    }

    public List<Appointment> GetStylistNotifications(string stylistEmail)
    {
        var currentTime = DateTime.Now;
        var notifyTime = currentTime.AddHours(3);

        // Fetch appointments for the stylist happening within the next 3 hours
        var upcomingAppointments = _dbContext.Appointments
            .Where(a => a.Stylist != null && a.Stylist.EmailAddress == stylistEmail
                        && a.Date == currentTime.Date
                        && a.StartTime >= currentTime.TimeOfDay
                        && a.StartTime <= notifyTime.TimeOfDay
                        && !a.NotificationSent)
            .ToList();

        return upcomingAppointments;
    }

    public void MarkNotificationAsSent(Appointment appointment)
    {
        appointment.NotificationSent = true;
        _dbContext.SaveChanges();
    }
}
