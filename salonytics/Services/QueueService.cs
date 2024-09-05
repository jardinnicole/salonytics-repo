using Microsoft.EntityFrameworkCore;
using salonytics.Data;
using salonytics.Models.Entities;

namespace salonytics.Services
{
    public class QueueService
    {
        private readonly AppDbContext _dbContext;

        public QueueService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GenerateCustomQueueId()
        {
            // Generate a unique identifier (e.g., UUID)
            string uniquePart = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            // You could add additional logic to ensure uniqueness if needed
            return $"QUEUE-{uniquePart}";
        }

        public async Task PopulateQueuesFromAppointmentsAsync()
        {
            // Get the current date
            var currentDate = DateTime.Today;

            // Remove existing queues from previous dates
            var oldQueues = await _dbContext.Queues
                .Where(q => _dbContext.Appointments
                    .Where(a => a.Date != currentDate)
                    .Select(a => a.AppointmentId)
                    .Contains(q.AppointmentId))
                .ToListAsync();

            _dbContext.Queues.RemoveRange(oldQueues);

            // Save changes to remove old queues
            await _dbContext.SaveChangesAsync();

            // Get all appointments for the current date
            var appointments = await _dbContext.Appointments
                .Where(a => a.Date == currentDate)
                .OrderBy(a => a.StartTime) // Order appointments by StartTime
                .ToListAsync();

            // Create queue entries for the current day's appointments
            var queues = new List<Queue>();
            int position = 1;

            foreach (var appointment in appointments)
            {
                // Create a new Queue entry
                var queue = new Queue
                {
                    QueueId = GenerateCustomQueueId(), // Or another method for QueueId
                    AppointmentId = appointment.AppointmentId,
                    Position = position,
                    Status = "Pending" // Or another default value
                };

                queues.Add(queue);
                position++;
            }

            // Add new queue entries
            _dbContext.Queues.AddRange(queues);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();
        }


        public async Task CreateQueueAsync(Queue queue)
        {
            _dbContext.Queues.Add(queue);
            await _dbContext.SaveChangesAsync();
        }
    }
    
}
