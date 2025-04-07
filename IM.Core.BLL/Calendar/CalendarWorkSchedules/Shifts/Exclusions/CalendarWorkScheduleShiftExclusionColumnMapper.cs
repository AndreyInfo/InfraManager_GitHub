using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.CalendarWorkSchedules;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

internal sealed class CalendarWorkScheduleShiftExclusionColumnMapper : IColumnMapperSetting<CalendarWorkScheduleShiftExclusion, CalendarWorkScheduleShiftExclusionsColumns>
    , ISelfRegisteredService<IColumnMapperSetting<CalendarWorkScheduleShiftExclusion, CalendarWorkScheduleShiftExclusionsColumns>>
{
    public void Configure(IColumnMapperSettingsBase<CalendarWorkScheduleShiftExclusion, CalendarWorkScheduleShiftExclusionsColumns> configurer)
    {
        configurer.ShouldBe(c => c.ExclusionName, c => c.Exclusion.Name);
        configurer.ShouldBe(c => c.TimeEnd, c => c.TimeStart);
    }

}
