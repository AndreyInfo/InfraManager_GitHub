using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.BLL.CalendarService
{
    public class WorkTimeByUserData
    {
        public DateTime utcStartDate { get; init; }
        public DateTime utcFinishDate { get; init; }
        public Guid UserID { get; init; }
    }
}
