using System;

namespace InfraManager.BLL.OrganizationStructure;

public class OrganizationModelDetails
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
    public string ExternalId { get; set; }
    public Guid? PeripheralDatabaseId { get; set; }
    public Guid? ComplementaryId { get; set; }
    public Guid? CalendarWorkScheduleId { get; set; }
    public bool? IsLockedForOsi { get; set; }
}
