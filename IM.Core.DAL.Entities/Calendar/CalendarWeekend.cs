using Inframanager;
using System;

namespace InfraManager.DAL;

[ObjectClassMapping(ObjectClass.CalendarWeekend)]
[OperationIdMapping(ObjectAction.Insert, OperationID.CalendarWeekend_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.CalendarWeekend_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.CalendarWeekend_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.CalendarWeekend_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.CalendarWeekend_Properties)]
public class CalendarWeekend : Catalog<Guid>
{
    public bool Sunday { get; set; }
    public bool Monday { get; set; }
    public bool Tuesday { get; set; }
    public bool Wednesday { get; set; }
    public bool Thursday { get; set; }
    public bool Friday { get; set; }
    public bool Saturday { get; set; }
    public byte[] RowVersion { get; set; }
    public Guid? ComplementaryID { get; set; }
}
