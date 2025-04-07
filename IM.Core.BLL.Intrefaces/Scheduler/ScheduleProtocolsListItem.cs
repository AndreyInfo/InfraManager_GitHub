using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services.ScheduleService;
using System;
using System.Text.Json.Serialization;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    [ListViewItem(ListView.ScheduleProtocolsList)]
    public class ScheduleProtocolsListItem
    {
        public Guid ID { get { return default; } }
        public Guid TaskSettingID { get; set; }

        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.Name))]
        public string Name { get { return default; } }
        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.WorkOrderType))]
        public string TaskTypeName { get; init; }
        [ColumnSettings(2, 100)]
        [Label(nameof(Resources.ELPListState))]
        public string TaskStateName { get; init; }
        [ColumnSettings(3, 100)]
        [Label(nameof(Resources.ScheduleLastStart))]
        public DateTime? LastStartAt { get; init; }
        [ColumnSettings(4, 100)]
        [Label(nameof(Resources.ScheduleFinish))]
        public DateTime? FinishRunAt { get; init; }
    }
}

