using Inframanager;
using System.Collections.Generic;

namespace InfraManager.DAL.CalendarWorkSchedules;

[ObjectClassMapping(ObjectClass.CalendarWorkSchedule)]
[OperationIdMapping(ObjectAction.InsertAs, OperationID.CalendarWorkSchedule_AddAs)]
[OperationIdMapping(ObjectAction.Insert, OperationID.CalendarWorkSchedule_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.CalendarWorkSchedule_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.CalendarWorkSchedule_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.CalendarWorkSchedule_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.CalendarWorkSchedule_Properties)]
public class CalendarWorkSchedule : Lookup
{
    public CalendarWorkSchedule()
    { }

    public CalendarWorkSchedule(string name) : base(name)
    { }
    public int Year { get; set; }

    public string ShiftTemplate { get; init; }

    public byte ShiftTemplateLeft { get; init; }

    public string Note { get; init; }

    public virtual ICollection<CalendarWorkScheduleItem> WorkScheduleItems { get; init; }

    public virtual ICollection<CalendarWorkScheduleShift> WorkScheduleShifts { get; init; }
}
