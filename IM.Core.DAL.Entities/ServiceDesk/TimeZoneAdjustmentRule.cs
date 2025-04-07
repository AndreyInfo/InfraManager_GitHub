using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    public class TimeZoneAdjustmentRule
    {
        public string TimeZoneID { get; init; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public short DaylightDeltaInMinutes { get; set; }
        public bool TransitionStart_IsFixedDateRule { get; set; }
        public byte TransitionStart_Month { get; set; }
        public byte? TransitionStart_Day { get; set; }
        public DateTime TransitionStart_TimeOfDay { get; set; }
        public byte? TransitionStart_Week { get; set; }
        public byte? TransitionStart_DayOfWeek { get; set; }
        public bool TransitionEnd_IsFixedDateRule { get; set; }
        public byte TransitionEnd_Month { get; set; }
        public byte? TransitionEnd_Day { get; set; }
        public DateTime TransitionEnd_TimeOfDay { get; set; }
        public byte? TransitionEnd_Week { get; set; }
        public byte? TransitionEnd_DayOfWeek { get; set; }

    }
}
