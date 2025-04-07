using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Calendar;
using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

public class FilterShiftsExclusion : BaseFilter
{
    public Guid CalendarWorkSheduleShiftID { get; init; }
    public ExclusionType? Type { get; init; }
}
