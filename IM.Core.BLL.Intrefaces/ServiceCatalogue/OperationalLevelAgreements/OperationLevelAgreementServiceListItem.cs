using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

[ListViewItem(ListView.OperationLevelAgreementServiceColumns)]
public class OperationLevelAgreementServiceListItem
{
    [ColumnSettings(1)]
    [Label(nameof(Resources.Name))]
    public string Name { get; init; }
    
    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.CallState))]
    public string StateName { get { return default; } }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Owner))]
    public string OwnerName { get { return default; } }
    
    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.LinkCategory))]
    public string Category { get { return default; } }
}