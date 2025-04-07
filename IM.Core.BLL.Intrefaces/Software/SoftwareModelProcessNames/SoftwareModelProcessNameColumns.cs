using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Software.SoftwareModelProcessNames;

[ListViewItem(ListView.SoftwareModelProcessNameColumns)]
public class SoftwareModelProcessNameColumns
{
    [ColumnSettings(1,100)]
    [Label(nameof(Resources.SoftwareModel_ProcessNames))]
    public string ProcessName { get; }
}
