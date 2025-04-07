using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.FormBuilder
{
    [ListViewItem(ListView.FormBuilderList)]
    public class FormBuilderForTable
    {
        public Guid ID { get; init; }

        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.Name))]
        public string Name { get; init; }

        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.FormBuilder_Description))]
        public string Description { get; init; }

        [ColumnSettings(2, 100)]
        [Label(nameof(Resources.FormBuilder_Version))]
        public string Version { get; init; }

        [ColumnSettings(3, 100)]
        [Label(nameof(Resources.FormBuilder_ObjectType))]
        public string Class { get; init; }

        [ColumnSettings(4, 100)]
        [Label(nameof(Resources.FormBuilder_Status))]
        public string Status { get; init; }

        [ColumnSettings(5, 100)]
        [Label(nameof(Resources.LastChange))]
        public DateTime UtcChanged { get; init; }
    }
}
