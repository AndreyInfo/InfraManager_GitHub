using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.BLL.CalendarService
{
    public class WorkDateByUserData
    {
        public DateTime utcStartDate { get; init; }
        public TimeSpan Duration { get; init; }
        public Guid UserID { get; init; }
    }
}
