using salonytics.Models.Entities;

namespace salonytics.Models
{
    public class DailyScheduleViewModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<StylistSchedule> Schedules { get; set; } = new List<StylistSchedule>();
    }

    public class WeeklyScheduleViewModel
    {
        public Guid StylistId { get; set; }
        public string StylistName { get; set; }
        public List<DailyScheduleViewModel> DailySchedules { get; set; } = new List<DailyScheduleViewModel>();
    }

}
