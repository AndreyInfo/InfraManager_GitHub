using System;

namespace IM.Core.WF.BLL.Interfaces.Models
{
    public class CalendarInfo
    {
        public DateTime? UtcEnteredAt { get; set; }
        
        public Guid? CalendarWorkScheduleId { get; set; }
        
        public string TimeZoneId { get; set; }
    }
}
