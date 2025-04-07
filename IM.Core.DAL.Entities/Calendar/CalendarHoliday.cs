using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL;

[ObjectClassMapping(ObjectClass.CalendarHoliday)]
[OperationIdMapping(ObjectAction.Insert, OperationID.CalendarHoliday_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.CalendarHoliday_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.CalendarHoliday_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.CalendarHoliday_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.CalendarHoliday_Properties)]
public class CalendarHoliday : Catalog<Guid>
{
    public CalendarHoliday()
    {
        ID = Guid.NewGuid();
    }

    public byte[] RowVersion { get; init; }
    public Guid? ComplementaryID { get; init; }

    public virtual ICollection<CalendarHolidayItem> CalendarHolidayItems { get; init; }
}
