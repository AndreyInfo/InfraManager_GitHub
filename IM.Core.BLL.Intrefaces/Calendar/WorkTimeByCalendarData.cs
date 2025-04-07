using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.BLL.CalendarService
{
    public class WorkTimeByCalendarData
    {
        public DateTime utcStartDate { get; init; }
        public DateTime utcFinishDate { get; init; }
        public Guid? calendarWorkScheduleID { get; init; }
        public string calendarTimeZoneID { get; init; }
    }
}
