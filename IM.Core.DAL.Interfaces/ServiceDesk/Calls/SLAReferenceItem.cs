using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    public class SLAReferenceItem
    {
        public Guid ID { get; init; }

        public ObjectClass ClassID { get; init; }

        public Guid ObjectID { get; init; }
        
        public string TimeZoneID { get; init; }
        
        public string  TimeZoneName { get; init; }
    
        public Guid? CalendarWorkScheduleID { get; init; }
        
        public string CalendarWorkScheduleName { get; init; }
    }
}
