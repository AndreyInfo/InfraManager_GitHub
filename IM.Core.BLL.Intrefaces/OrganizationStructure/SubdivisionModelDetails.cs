using System;

namespace InfraManager.BLL.OrganizationStructure;

public class SubdivisionModelDetails
{
    public Guid ID { get; private set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }
    public Guid? SubdivisionID { get; set; }
    public string Note { get; set; }
    public string ExternalId { get; set; }
    public Guid? PeripheralDatabaseId { get; set; }
    public Guid? ComplementaryId { get; set; }
    public Guid? CalendarWorkScheduleId { get; set; }
    public bool? IsLockedForOsi { get; set; }
    public bool It { get; set; }
}
