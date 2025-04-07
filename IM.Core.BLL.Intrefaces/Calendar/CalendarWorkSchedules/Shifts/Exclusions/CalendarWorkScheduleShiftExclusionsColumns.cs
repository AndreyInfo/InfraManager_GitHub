using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

[ListViewItem(ListView.ExclusionsForTable)]
public sealed class CalendarWorkScheduleShiftExclusionsColumns
{

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Cause))]
    public string ExclusionName { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.TimeStart))]
    public string TimeStart { get; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.End))]
    public string TimeEnd { get; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.Duration))]
    public string TimeSpanInMinutes { get; }
}
