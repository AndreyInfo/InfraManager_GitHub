using InfraManager.ResourcesArea;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using Inframanager.BLL.ListView;
using Inframanager.BLL;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;

[ListViewItem(ListView.ServiceItem)]
public class ServiceItemColumns
{

    [LikeFilter]
    [ColumnSettings(1)]
    [Label(nameof(Resources.Name))]
    public string Name { get; init; }
    
    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.CallState))]
    public string StateName { get { return default; } }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get { return default; } }
}
