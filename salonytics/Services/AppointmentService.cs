using salonytics.Data;
using Salonytics.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace salonytics.Services
{
    public class AppointmentService
    {
        private readonly AppDbContext _dbContext;

        public AppointmentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetBranchCode(Guid branchId)
        {
            return branchId switch
            {
                Guid id when id == Guid.Parse("b437b886-248f-4e6c-9706-47572ba76615") => "BOCA",
                Guid id when id == Guid.Parse("9f60aed9-9a2f-4917-b063-599ed76fbf3c") => "SMLB",
                Guid id when id == Guid.Parse("68eb0e8c-47ba-45ad-a69e-6e9b1567809d") => "APLB",
                Guid id when id == Guid.Parse("4a0b9a40-9675-4dbd-bcc7-a2bdb68fcf08") => "PULB",
                Guid id when id == Guid.Parse("0e47e3cb-41a8-4fcf-95d7-a7d8d3b171b5") => "PLRB",
                Guid id when id == Guid.Parse("ac194824-c21d-4102-a91d-b3f0d6d302ea") => "SFPB",
                Guid id when id == Guid.Parse("4a65a8b9-a58f-4528-af0c-c40bf584ca33") => "GUIG",
                Guid id when id == Guid.Parse("18acf19b-1e92-4a1f-a5a3-cb96d5236b80") => "MOJN",
                Guid id when id == Guid.Parse("4f862fcf-81c7-4b10-b0e6-dde64fac5873") => "SRGB",
                Guid id when id == Guid.Parse("9e6206f8-098d-4a96-b32d-f90bc9dc8cb9") => "MMNB",
                _ => "UNKN"
            };
        }

        public async Task<string> GenerateAppointmentCodeAsync(Guid branchId, DateTime appointmentDate)
        {
            var branchCode = GetBranchCode(branchId);
            var currentYear = appointmentDate.Year;
            var appointmentCount = await _dbContext.Appointments
                .CountAsync(a => a.BranchId == branchId && a.Date.Year == currentYear) + 1;

            return $"{branchCode}-{currentYear}-{appointmentCount:0000}";
        }

        public async Task CreateAppointmentAsync(Appointment appointment)
        {
            // Generate the AppointmentCode
            appointment.AppointmentCode = await GenerateAppointmentCodeAsync(appointment.BranchId, appointment.Date);

            // Add the appointment to the database
           
        }
    }

}