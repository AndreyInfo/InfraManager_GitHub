using Inframanager.BLL.ListView;
using Inframanager.BLL;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Asset.ActivePort;

[ListViewItem(ListView.ActivePortColumns)]
public class ActivePortColumns
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.PortName))]
    public string PortName { get; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.ActiveEquipmentID))]
    public int? ActiveEquipmentID { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Number))]
    public int PortNumber { get; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Note))]
    public string Note { get; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.State))]
    public int? State { get; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.JackType))]
    public string JackTypeName { get; }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.TechnologyType))]
    public string TechnologyTypeName { get; }
    
}

