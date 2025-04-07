using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceDependencies;

[ListViewItem(ListView.ServiceDependency)]
public class ServiceDependencyForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.CallServiceCategory))]
    public string ServiceCategoryName { get; }


    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Type))]
    public string TypeName { get; }


    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.CallState))]
    public string StateName { get; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get; }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.Owner))]
    public string OwnerName { get; }
}
