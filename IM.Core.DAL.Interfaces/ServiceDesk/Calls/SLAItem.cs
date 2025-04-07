using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    public class SLAItem
    {
        public Guid ID { get; init; }
        
        public string Name { get; init; }
        
        public string Note { get; init; }
        
        public string Number { get; set; }
        
        public DateTime? UtcStartDate { get; set; }
        
        public DateTime? UtcFinishDate { get; set; }

        public string TimeZoneID { get; set; }

        public string TimeZoneName { get; set; }

        public Guid? CalendarWorkScheduleID { get; set; }

        public string CalendarWorkScheduleName { get; init; }
    }
}
