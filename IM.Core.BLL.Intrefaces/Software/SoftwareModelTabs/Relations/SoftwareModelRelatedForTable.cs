using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Relations;

[ListViewItem(ListView.SoftwareModelRelatedForTable)]
public class SoftwareModelRelatedForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.ModelVersion))]
    public string Version { get; set; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get; set; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.ParentModelName))]
    public string ParentName { get; set; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.ParentModelVersion))]
    public string ParentVersion { get; set; }
}
