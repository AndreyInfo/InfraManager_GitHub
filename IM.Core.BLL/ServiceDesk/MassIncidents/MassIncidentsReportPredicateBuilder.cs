using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents;

internal class MassIncidentsReportPredicateBuilder<TReportItem> :
    FilterBuildersAggregate<MassIncidentsReportQueryResultItem, TReportItem>
{
}