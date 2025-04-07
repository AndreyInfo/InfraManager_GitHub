using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.CalendarService
{
    class CalendarDaylightTime
    {
        public CalendarDaylightTime(DateTime start, DateTime end, TimeSpan delta)
        {
            this.Start = start;
            this.End = end;
            this.Delta = delta;
        }

        public DateTime Start { get; }
        public DateTime End { get; }
        public TimeSpan Delta { get; }
    }
}
