using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Components;

[ListViewItem(ListView.SoftwareModelComponentForTable)]
public class SoftwareModelComponentForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; set; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.ModelVersion))]
    public string Version { get; set; }
  
    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get; set; }
}
