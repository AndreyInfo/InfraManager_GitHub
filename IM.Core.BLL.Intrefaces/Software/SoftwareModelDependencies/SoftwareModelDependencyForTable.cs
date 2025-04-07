using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Software.SoftwareModelDependencies;

[ListViewItem(ListView.SoftwareModelDependencyForTable)]
public class SoftwareModelDependencyForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.ParentModelID))]
    public string ParentSoftwareModelID { get; set; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Identifier))]

    public string ChildSoftwareModelID { get; set; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Type))]
    public string Type { get; set; }
}
