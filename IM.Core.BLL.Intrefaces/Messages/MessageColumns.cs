using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Messages;

[ListViewItem(ListView.MessageColumns)]
public class MessageColumns
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Type))]
    public string TypeName { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.CallState))]
    public string StateName { get; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Seriousness))]
    public string SeverityName { get; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.SC_Repeat))]
    public string Count { get; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.CallDateRegistered))]
    public string UtcDateRegistered { get; }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.Close))]
    public string UtcDateClosed { get; }
}
