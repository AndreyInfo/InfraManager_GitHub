 using InfraManager.DAL.CalendarWorkSchedules;
using System;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;

namespace InfraManager.DAL.ServiceCatalogue
{
    public class SLAReference : ITimeZoneObject
    {
        public Guid SLAID { get; set; }

        public ObjectClass ClassID { get; init; }

        public Guid ObjectID { get; init; }

        public string TimeZoneID { get; set; }
        
        public virtual TimeZone TimeZone { get; }

        public Guid? CalendarWorkScheduleID { get; set; }

        public virtual CalendarWorkSchedule CalendarWorkSchedule { get; }
    }
}
