using System;

namespace InfraManager.BLL.OrganizationStructure;

public class OrganizationData
{
    public string Name { get; init; }
    public string Note { get; init; }
    public string ExternalID { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryID { get; init; }
    public Guid? CalendarWorkScheduleID { get; init; }
    public bool? IsLockedForOsi { get; init; }
}