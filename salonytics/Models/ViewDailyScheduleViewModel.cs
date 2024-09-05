using salonytics.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace salonytics.Models
{
    public class ViewDailyScheduleViewModel
    {
        public Guid StylistId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public StylistSchedule Schedule { get; set; }
    }
}
