using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.WorkflowTracking
{
    [ListViewItem(ListView.WorkflowTrackingList)]
    public class WorkflowTrackingListItem
    {
        public Guid ID { get; set; }

        public Guid WorkflowSchemeID { get; set; }

        public string WorkflowSchemeIdentifier { get; set; }

        public string WorkflowSchemeVersion { get; set; }

        public int EntityClassID { get; set; }

        public Guid EntityID { get; set; }
       
        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.ObjectCategory))]
        public string EntityCategoryName { get; set; }
        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.WorktrackingScheme))]
        public string WorkflowSchemeName { get; set; }
        [ColumnSettings(2, 100)]
        [Label(nameof(Resources.EntityType))]
        public string EntityTypeName { get; set; }
        [ColumnSettings(3, 100)]
        [Label(nameof(Resources.Entity))]
        public string EntityName { get; set; }
        [ColumnSettings(4, 100)]
        [Label(nameof(Resources.AdminTools_SessionTable_DateOpened))]
        public DateTime UtcInitializedAt { get; set; }
        [ColumnSettings(5, 100)]
        [Label(nameof(Resources.AdminTools_SessionTable_DateClosed))]
        public DateTime? UtcTerminatedAt { get; set; }
    }
}
