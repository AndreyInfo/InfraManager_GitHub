using System.Collections.Generic;
using Inframanager;

namespace InfraManager.DAL.ServiceDesk
{
    [OperationIdMapping(ObjectAction.ViewDetails,OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class TimeZone
    {
        public string ID { get; init; }
        public string Name { get; set; }
        public short BaseUtcOffsetInMinutes { get; set; }
        public bool SupportsDaylightSavingTime { get; set; }

        public virtual ICollection<TimeZoneAdjustmentRule> TimeZoneAdjustmentRules { get; } =
            new HashSet<TimeZoneAdjustmentRule>();
    }
}
