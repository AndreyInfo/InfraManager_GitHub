using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;


namespace InfraManager.BLL.Events
{
    [ListViewItem(ListView.EventSubjectList)]
    public class EventSubjectListItem
    {
        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.Event_Date))]
        public DateTime Date { get; init; }
        [ColumnSettings(1, 200)]
        [Label(nameof(Resources.Event_Author))]
        public string UserName { get; init; }
        [ColumnSettings(2, 300)]
        [Label(nameof(Resources.Event_Description))]
        public string Description { get; init; }
    }
}
