using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceAttendances;

[ListViewItem(ListView.ServiceAttendance)]
public class ServiceAttendanceForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get { return default; } }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.CallState))]
    public string StateName { get { return default; } }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get { return default; } }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.WorkProcedure))]
    public string WorkflowSchemeIdentifier { get { return default; } }
}
