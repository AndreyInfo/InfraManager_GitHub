using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Calendar.Exclusions;

[ListViewItem(ListView.ExclusionForTable)]
public class ExclusionForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Exclusion_Type))]
    public int TypeName { get; }
}
