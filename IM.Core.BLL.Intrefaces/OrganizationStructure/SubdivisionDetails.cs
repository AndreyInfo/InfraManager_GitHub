using System;

namespace InfraManager.BLL.OrganizationStructure;

public class SubdivisionDetails
{
    public Guid ID { get; init; }
    public Guid OrganizationID { get; init; }
    public string Name { get; init; }
    public Guid? SubdivisionID { get; init; }
    public string Note { get; init; }
    public string ExternalId { get; init; }
    public Guid? PeripheralDatabaseID { get; init; }
    public Guid? ComplementaryId { get; init; }
    public Guid? CalendarWorkScheduleID { get; set; }
    public bool? IsLockedForOsi { get; init; }
    public bool It { get; init; }
    public string Path { get; init; }
}
