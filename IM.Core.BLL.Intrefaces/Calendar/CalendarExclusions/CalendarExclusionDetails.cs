using System;

namespace InfraManager.BLL.Calendar.CalendarExclusions;

public class CalendarExclusionDetails
{
    public Guid ID { get; init; }

    public ObjectClass ObjectClassID { get; set; }

    public Guid ObjectID { get; set; }

    public Guid ExclusionID { get; set; }

    public DateTime UtcPeriodStart { get; init; } = DateTime.UtcNow;

    public DateTime UtcPeriodEnd { get; set; }

    public bool IsWorkPeriod { get; set; }

    public Guid? ServiceReferenceID { get; set; }

    public string RelatedObjectName { get; set; }

    public Guid? RelatedObjectID { get; set; }

    public ObjectClass? RelatedObjectClassID { get; set; }

    public string ExclusionName { get; set; }

    public int Durability { get; set; }
}
