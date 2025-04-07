using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Calendar.CalendarHolidays;

[ListViewItem(ListView.CalendarHoliday)]
public class CalendarHolidayForTable
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }
}
