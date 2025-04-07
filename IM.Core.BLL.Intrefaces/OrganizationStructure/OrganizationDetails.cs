using System;

namespace InfraManager.BLL.OrganizationStructure
{
    public class OrganizationDetails
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Note { get; init; }
        public string ExternalId { get; init; }
        public Guid? PeripheralDatabaseId { get; init; }
        public Guid? ComplementaryId { get; init; }
        public Guid? CalendarWorkScheduleId { get; init; }
        public bool? IsLockedForOsi { get; init; }
    }
}
