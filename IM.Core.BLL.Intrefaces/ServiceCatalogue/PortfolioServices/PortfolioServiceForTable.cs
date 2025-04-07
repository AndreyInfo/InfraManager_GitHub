using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

[ListViewItem(ListView.PortfolioService)]
public class PortfolioServiceForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get { return default; } }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.CallServiceCategory))]
    public string ServiceCategoryName { get { return default; } }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Type))]
    public string TypeName { get { return default; } }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.CallState))]
    public string StateName { get { return default; } }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalId { get { return default; } }
}
