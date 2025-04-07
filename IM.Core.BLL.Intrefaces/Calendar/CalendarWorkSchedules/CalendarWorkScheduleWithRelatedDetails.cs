using System;
using System.Collections.Generic;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules;

/// <summary>
/// Для получения общей информации модели
/// </summary>
public class CalendarWorkScheduleWithRelatedDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public int Year { get; init; }
    public string ShiftTemplate { get; init; }
    public byte ShiftTemplateLeft { get; init; }
    public byte[] RowVersion { get; init; }

    public ICollection<CalendarWorkScheduleItemDetails> WorkScheduleItems { get; init; }
    public ICollection<CalendarWorkScheduleItemExclusionDetails> WorkScheduleItemExclusions { get; init; }
    public ICollection<CalendarWorkScheduleShiftDetails> WorkScheduleShifts { get; init; }

}

