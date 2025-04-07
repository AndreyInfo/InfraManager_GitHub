using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules;

[ListViewItem(ListView.CalendarSchedule)]
public class CalendarSchedule
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Note))]
    public string Note { get; }


    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Year))]
    public int Year { get; }
    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.ShiftTemplateLeft))]
    public byte ShiftTemplateLeft { get; }

}
