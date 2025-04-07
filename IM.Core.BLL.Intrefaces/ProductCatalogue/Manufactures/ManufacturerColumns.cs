using Inframanager.BLL;
using InfraManager.ResourcesArea;
using Inframanager.BLL.ListView;

namespace InfraManager.BLL.ProductCatalogue.Manufactures;

[ListViewItem(ListView.ManufacturerColumns)]
public sealed class ManufacturerColumns
{
    [ColumnSettings(1)]
    [Label(nameof(Resources.Name))]
    public string Name { get; init; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.IsCable))]
    public string IsCable { get; init; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.IsRack))]
    public bool IsRack { get; init; }
   
    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.IsPanel))]
    public bool IsPanel { get; init; }
    
    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.IsNetworkDevice))]
    public bool IsNetworkDevice { get; init; }
    
    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.IsComputer))]
    public bool IsComputer { get; init; }
    
    [ColumnSettings(7, 100)]
    [Label(nameof(Resources.IsOutlet))]
    public bool IsOutlet { get; init; }
    
    [ColumnSettings(8, 100)]
    [Label(nameof(Resources.IsCableCanal))]
    public bool IsCableCanal { get; init; }
    
    [ColumnSettings(9, 100)]
    [Label(nameof(Resources.IsSoftware))]
    public bool IsSoftware { get; init; }
    
    [ColumnSettings(10, 100)]
    [Label(nameof(Resources.IsMaterials))]
    public bool IsMaterials { get; init; }
    
    [ColumnSettings(11, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get; init; }
}
