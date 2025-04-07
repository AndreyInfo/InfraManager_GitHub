using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Calendar.CalendarWeekends
{
    [ListViewItem(ListView.CalendarWeekend)]
    public class CalendarWeekendForTable
    {
        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.Name))]
        public string Name { get; }

        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.Day_Sunday))]
        public bool Sunday { get; }

        [ColumnSettings(2, 100)]
        [Label(nameof(Resources.Day_Monday))]
        public bool Monday { get; }

        [ColumnSettings(3, 100)]
        [Label(nameof(Resources.Day_Tuesday))]
        public bool Tuesday { get; }

        [ColumnSettings(4, 100)]
        [Label(nameof(Resources.Day_Wednesday))]
        public bool Wednesday { get; }

        [ColumnSettings(5, 100)]
        [Label(nameof(Resources.Day_Thursday))]
        public bool Thursday { get; }

        [ColumnSettings(6, 100)]
        [Label(nameof(Resources.Day_Friday))]
        public bool Friday { get; }

        [ColumnSettings(7, 100)]
        [Label(nameof(Resources.Day_Saturday))]
        public bool Saturday { get; }

    }
}
