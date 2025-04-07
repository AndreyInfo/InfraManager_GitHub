using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.CustomControl
{
    internal class ControlListItemPredicateBuilders : 
        FilterBuildersAggregate<ObjectUnderControlQueryResultItem, ObjectUnderControl>, 
        ISelfRegisteredService<IAggregatePredicateBuilders<ObjectUnderControlQueryResultItem, ObjectUnderControl>>
    {
        public ControlListItemPredicateBuilders() : base()
        {
            AddPredicateBuilder(x => x.UnreadMessageCount);
            AddPredicateBuilder(x => x.DocumentCount);
            AddPredicateBuilder(x => x.CategorySortColumn);
        }
    }
}
