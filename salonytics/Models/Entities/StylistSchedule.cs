using Salonytics.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salonytics.Models.Entities
{
    public class StylistSchedule
    {
        [Key]
        public Guid ScheduleId { get; set; }       

        public Guid StylistId { get; set; }

        [EnumDataType(typeof(DayOfWeek))]
        public DayOfWeek DayOfWeek { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        [Required]
        public bool IsRestDay { get; set; }
        public bool IsWorking { get; set; }
    }
}
