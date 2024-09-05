using System;
using System.Collections.Generic;
using salonytics.Models.Entities;

namespace salonytics.Models
{
    public class ViewScheduleViewModel
    {
        public Guid StylistId { get; set; }
        public List<StylistSchedule> Schedules { get; set; }
    }
}
