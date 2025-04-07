using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.OrganizationStructure;

[ListViewItem(ListView.OrganizationsList)]
public class OrganizationsList
{
    [ColumnSettings(1)]
    [Label(nameof(Resources.Organization_Name))]
    public string Name { get; init; }

    [ColumnSettings(2)]
    [Label(nameof(Resources.Organization_Note))]
    public string Note { get; init; }
}