using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;


namespace InfraManager.BLL.WorkFlow
{
    [ListViewItem(ListView.WorkflowScheme)]
    public class WorkflowSchemeForTable
    {
        public Guid ID { get; set; }

        [ColumnSettings(1)]
        [Label(nameof(Resources.WorkflowScheme_Name))]
        public string Name { get; set; }

        [ColumnSettings(2)]
        [Label(nameof(Resources.WorkflowScheme_Identifier))]
        public string Identifier { get; set; }

        [ColumnSettings(3)]
        [Label(nameof(Resources.WorkflowScheme_version))]
        public string Version { get; set; }

        [ColumnSettings(4)]
        [Label(nameof(Resources.WorkflowScheme_Status))]
        public string Status { get; set; }

        [ColumnSettings(5)]
        [Label(nameof(Resources.WorkflowScheme_UtcDateModified))]
        public DateTime UtcDateModified { get; set; }

        [ColumnSettings(6)]
        [Label(nameof(Resources.WorkflowScheme_ObjectClass))]
        public string ObjectClassName { get; set; }

    }
}
