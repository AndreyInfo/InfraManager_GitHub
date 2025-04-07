using System;
using System.Collections.Generic;

namespace InfraManager.DAL.CalendarWorkSchedules
{
    public class CalendarWorkScheduleShift
    {
        public Guid ID { get; set; }

        public Guid CalendarWorkScheduleID { get; set; }

        public byte Number { get; set; }

        public DateTime TimeStart { get; set; }

        public short TimeSpanInMinutes { get; set; }

        public virtual CalendarWorkSchedule CalendarWorkSchedule { get; set; }

        public virtual ICollection<CalendarWorkScheduleShiftExclusion> WorkScheduleShiftExclusions { get; }
    }
}
