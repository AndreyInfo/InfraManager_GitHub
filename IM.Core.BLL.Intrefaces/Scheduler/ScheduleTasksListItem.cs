using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using InfraManager.Services.ScheduleService;
using System;
using System.Text.Json.Serialization;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    [ListViewItem(ListView.ScheduleTasksList)]
    public class ScheduleTasksListItem
    {
        public Guid ID { get { return default; } }

        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.Name))]
        public string Name { get { return default; } }
        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.LinkNote))]
        public string Description { get { return default; } }
        [ColumnSettings(2, 100)]
        [Label(nameof(Resources.ELPListScheduleLabel))]
        public string Schedule { get { return default; } }
        [ColumnSettings(3, 100)]
        [Label(nameof(Resources.ELPListState))]
        public string TaskStateName { get { return default; } }
        [ColumnSettings(4, 100)]
        [Label(nameof(Resources.NextRunAt))]
        public DateTime? NextRunAt { get { return default; } }
        [ColumnSettings(5, 100)]
        [Label(nameof(Resources.FinishRunAt))]
        public DateTime? FinishRunAt { get { return default; } }

    }
}

