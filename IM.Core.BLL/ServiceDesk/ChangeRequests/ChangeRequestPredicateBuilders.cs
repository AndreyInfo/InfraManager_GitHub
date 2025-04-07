using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestPredicateBuilders :
         FilterBuildersAggregate<ChangeRequest, ChangeRequestListItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<ChangeRequest, ChangeRequestListItem>>
    {
        public ChangeRequestPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(ChangeRequestListItem.Number),  x => x.Number);
            AddPredicateBuilder(nameof(ChangeRequestListItem.Summary), x => x.Summary);
            AddPredicateBuilder(nameof(ChangeRequestListItem.UrgencyName), x => x.UrgencyID);
            AddPredicateBuilder(nameof(ChangeRequestListItem.InfluenceName), x => x.InfluenceID);
            AddPredicateBuilder(nameof(ChangeRequestListItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(ChangeRequestListItem.TypeFullName), x => x.Type.ID);
            AddPredicateBuilder(nameof(ChangeRequestListItem.CategoryFullName), x => x.CategoryID);
            AddPredicateBuilder(nameof(ChangeRequestListItem.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(ChangeRequestListItem.InitiatorFullName), x => x.InitiatorID);
            AddPredicateBuilder(nameof(ChangeRequestListItem.UtcDateDetected), x => x.UtcDateDetected);
            AddPredicateBuilder(nameof(ChangeRequestListItem.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(ChangeRequestListItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(ChangeRequestListItem.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(ChangeRequestListItem.UtcDateSolved), x => x.UtcDateSolved);
            AddPredicateBuilder(nameof(ChangeRequestListItem.UtcDateStarted), x => x.UtcDateStarted);
            AddPredicateBuilder(nameof(ChangeRequestListItem.Target), x => x.Target);
            AddPredicateBuilder(nameof(ChangeRequestListItem.Description), x => DbFunctions.CastAsString(x.Description));
            AddPredicateBuilder(nameof(ChangeRequestListItem.FundingAmount), x => (int)x.FundingAmount);
        }
    }
}
