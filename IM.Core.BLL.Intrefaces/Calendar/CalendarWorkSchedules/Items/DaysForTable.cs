using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;

[ListViewItem(ListView.DaysForTable)]
public class DaysForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Number))]
    public string DayOfYear { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.PeriodType_Day))]
    public string DayOfYearDate { get; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Type))]
    public string DayTypeName { get; }


    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.Shift))]
    public string ShiftNumber { get; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.TimeStart))]
    public string TimeStart { get; }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.End))]
    public string TimeEnd { get; }
}
