using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestListItemPredicateBuilders :
        FilterBuildersAggregate<ChangeRequestQueryResultItem, ChangeRequestListItem>,
        ISelfRegisteredService<IAggregatePredicateBuilders<ChangeRequestQueryResultItem, ChangeRequestListItem>>
    {
        public ChangeRequestListItemPredicateBuilders() : base()
        {
            AddPredicateBuilder(x => x.UnreadMessageCount);
            AddPredicateBuilder(x => x.EntityStateName);
            AddPredicateBuilder(x => x.WorkOrderCount);
            AddPredicateBuilder(x => x.ReasonObjectName);
        }
    }
}
