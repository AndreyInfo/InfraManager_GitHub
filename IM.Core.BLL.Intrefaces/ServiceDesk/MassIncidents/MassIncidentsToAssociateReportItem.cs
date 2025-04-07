using Inframanager.BLL.ListView;

namespace InfraManager.BLL.ServiceDesk.MassIncidents;

/// <summary>
/// Представляет элемент отчета "Массовые инциденты для ассоциации".
/// </summary>
[ListViewItem(ListView.MassIncidentsToAssociate, OperationID.Problem_Update)]
public class MassIncidentsToAssociateReportItem : ProblemMassIncidentsReportItemBase
{
}