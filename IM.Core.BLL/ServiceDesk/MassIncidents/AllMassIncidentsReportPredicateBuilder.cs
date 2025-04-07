using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class AllMassIncidentsReportPredicateBuilder :
        FilterBuildersAggregate<AllMassIncidentsReportQueryResultItem, AllMassIncidentsReportItem>,
        ISelfRegisteredService<IAggregatePredicateBuilders<AllMassIncidentsReportQueryResultItem, AllMassIncidentsReportItem>>
    {
        public AllMassIncidentsReportPredicateBuilder()
        {
            AddPredicateBuilder(reportItem => reportItem.DocumentsQuantity, queryItem => queryItem.DocumentsQuantity);
        }
    }
}
