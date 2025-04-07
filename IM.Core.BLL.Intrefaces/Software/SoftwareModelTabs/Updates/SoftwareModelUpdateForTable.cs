using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Updates;

[ListViewItem(ListView.SoftwareModelUpdateForTable)]
public class SoftwareModelUpdateForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.ModelVersion))]
    public string Version { get; set; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get; set; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.ParentModelName))]
    public string ParentModelName { get; set; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.ParentModelVersion))]
    public string ParentModelVersion { get; set; }
}
