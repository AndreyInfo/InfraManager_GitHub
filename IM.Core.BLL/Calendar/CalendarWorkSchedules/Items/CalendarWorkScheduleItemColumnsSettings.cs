using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;

internal sealed class CalendarWorkScheduleItemColumnsSettings : IColumnMapperSetting<CalendarWorkScheduleItem, DaysForTable>
    , ISelfRegisteredService<IColumnMapperSetting<CalendarWorkScheduleItem, DaysForTable>>
{
    public void Configure(IColumnMapperSettingsBase<CalendarWorkScheduleItem, DaysForTable> configurer)
    {
        configurer.ShouldBe(c => c.DayOfYearDate, c => c.DayOfYear);
        configurer.ShouldBe(c => c.ShiftNumber, c => c.ShiftNumber);
        configurer.ShouldBe(c => c.DayTypeName, c => c.DayType);
        // TODO избавиться от ошибики, т.к. сортироваться должно по конечному времени, а не по продолжительности
        configurer.ShouldBe(c => c.TimeEnd, c => c.TimeSpanInMinutes);
    }
}
