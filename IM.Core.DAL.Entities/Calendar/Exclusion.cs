using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Calendar;

/// <summary>
/// Является причиной отклонения
/// </summary>
[ObjectClassMapping(ObjectClass.Exclusion)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ExclusionAdd)]
[OperationIdMapping(ObjectAction.Update, OperationID.ExclusionUpdate)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ExclusionDelete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ExclusionProperties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ExclusionProperties)]
public class Exclusion
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public ExclusionType Type { get; init; }

    public byte[] RowVersion { get; init; }

    public virtual ICollection<CalendarExclusion> CalendarExclusions { get; }
}
