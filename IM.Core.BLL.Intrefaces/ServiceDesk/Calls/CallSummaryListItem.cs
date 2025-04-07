using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;
using System.Text.Json.Serialization;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    [ListViewItem(ListView.CallSummaryList)]
    public class CallSummaryListItem
    {
        public Guid ID { get { return default; } }

        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.ShortDescription))]
        public string Name { get { return default; } }
        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.Service))]
        public string ServiceName { get { return default; } }
        [ColumnSettings(2, 100)]
        [Label(nameof(Resources.CallServiceItemOrAttendance))]
        public string ItemOrAttendanceName { get { return default; } }
        [ColumnSettings(3, 100)]
        [Label(nameof(Resources.ShowAtRegistration))]
        public bool Visible { get { return default; } }
        [ColumnSettings(4, 100)]
        [Label(nameof(Resources.ServiceCategoryName))]
        public string ServiceCategoryName { get { return default; } }

    }
}
