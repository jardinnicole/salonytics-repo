using salonytics.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace salonytics.Models
{
    public class StylistScheduleViewModel
    {
        public Guid StylistId { get; set; }
        public bool CopyToAllDays { get; set; }
        public int SelectedDayIndex { get; set; }

        public List<DayScheduleViewModel> DaySchedules { get; set; }
    }

        public class DayScheduleViewModel
        {
            public DayOfWeek DayOfWeek { get; set; }
            public bool IsWorking { get; set; }
            public bool IsRestDay { get; set; }
            public string StartTimeString { get; set; }
            public string EndTimeString { get; set; }
        }
}
