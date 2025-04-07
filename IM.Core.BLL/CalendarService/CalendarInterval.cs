using InfraManager.DAL.CalendarWorkSchedules;
using System;

namespace InfraManager.BLL.CalendarService
{
    public class CalendarInterval
    {
        //public CalendarInterval() { }
        public CalendarInterval(CalendarDayType calendarDayType,  DateTime utcStartDate, DateTime utcEndDate)
        {
            CalendarDayType = calendarDayType;
            UtcStartDate = utcStartDate;
            UtcEndDate = utcEndDate;
        }
        public CalendarDayType CalendarDayType { get; init; }

        public DateTime UtcStartDate { get; init; }
        public DateTime UtcEndDate { get; init; }

        public TimeSpan Duration
        {
            get { return UtcEndDate - UtcStartDate; }
        }

    }
}
