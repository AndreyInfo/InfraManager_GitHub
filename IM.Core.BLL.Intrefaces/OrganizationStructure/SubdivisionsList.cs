using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.OrganizationStructure;

[ListViewItem(ListView.SubdivisionsList)]
public class SubdivisionsList
{
    [ColumnSettings(1)]
    [Label(nameof(Resources.Subdivision_Name))]
    public string Name { get; init; }

    [ColumnSettings(2)]
    [Label(nameof(Resources.Subdivision_Note))]
    public string Note { get; init; }
}