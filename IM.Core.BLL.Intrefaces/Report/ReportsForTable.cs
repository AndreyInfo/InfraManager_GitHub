using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.Report
{
    [ListViewItem(ListView.Reports)]
    public class ReportsForTable
    {
        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.Name))]
        public string Name { get; }

        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.UserNote))]
        public string Note { get; }

        [ColumnSettings(2, 100)]
        [Label(nameof(Resources.TimelineDateType_UtcDateCreated))]
        public DateTime DateCreated { get; }

        [ColumnSettings(3, 100)]
        [Label(nameof(Resources.ReportChangeDateModified))]
        public DateTime DateModified { get; }

        [ColumnSettings(4, 100)]
        [Label(nameof(Resources.Folder))]
        public string StringFolder { get; }
    }
}
