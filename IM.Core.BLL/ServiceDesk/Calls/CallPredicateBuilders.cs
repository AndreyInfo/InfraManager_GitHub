using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallPredicateBuilders :
         FilterBuildersAggregate<Call, CallListItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<Call, CallListItem>>
    {
        public CallPredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(CallListItem.ClientFullName), x => x.ClientID);
            AddPredicateBuilder(nameof(CallListItem.ClientSubdivisionFullName), x => x.ClientSubdivisionID);
            AddPredicateBuilder(nameof(CallListItem.ClientOrganizationName), x => x.ClientSubdivision.Organization.ID);
            AddPredicateBuilder(nameof(CallListItem.InitiatorFullName), x => x.InitiatorID);
            AddPredicateBuilder(nameof(CallListItem.OwnerFullName), x => x.OwnerID);
            AddPredicateBuilder(nameof(CallListItem.AccomplisherFullName), x => x.AccomplisherID);
            AddPredicateBuilder(nameof(CallListItem.ExecutorFullName), x => x.ExecutorID);
            AddPredicateBuilder(nameof(CallListItem.QueueName), x => x.QueueID);
            AddPredicateBuilder(nameof(CallListItem.ReceiptTypeString), x => x.ReceiptType);
            AddPredicateBuilder(nameof(CallListItem.TypeFullName), x => x.CallType.ID);
            AddPredicateBuilder(nameof(CallListItem.Summary), x => x.CallSummaryName);
            AddPredicateBuilder(nameof(CallListItem.ServiceName), x => x.CallService.Service.ID);
            AddPredicateBuilder(nameof(CallListItem.UrgencyName), x => x.Urgency.ID);
            AddPredicateBuilder(nameof(CallListItem.InfluenceName), x => x.Influence.ID);
            AddPredicateBuilder(nameof(CallListItem.PriorityName), x => x.Priority.ID);
            AddPredicateBuilder(nameof(CallListItem.Description), x => DbFunctions.CastAsString(x.Description));
            AddPredicateBuilder(nameof(CallListItem.Solution), x => DbFunctions.CastAsString(x.Solution));
            AddPredicateBuilder(nameof(CallListItem.UtcDateRegistered), x => x.UtcDateRegistered);
            AddPredicateBuilder(nameof(CallListItem.UtcDateOpened), x => x.UtcDateOpened);
            AddPredicateBuilder(nameof(CallListItem.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(CallListItem.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(CallListItem.UtcDateClosed), x => x.UtcDateClosed);
            AddPredicateBuilder(nameof(CallListItem.UtcDateModified), x => x.UtcDateModified);
            AddPredicateBuilder(nameof(CallListItem.UtcDateCreated), x => x.UtcDateCreated);
            AddPredicateBuilder(nameof(CallListItem.SLAName), x => x.SLAName);
            AddPredicateBuilder(nameof(CallListItem.UserField1), x => x.UserField1);
            AddPredicateBuilder(nameof(CallListItem.UserField2), x => x.UserField2);
            AddPredicateBuilder(nameof(CallListItem.UserField3), x => x.UserField3);
            AddPredicateBuilder(nameof(CallListItem.UserField4), x => x.UserField4);
            AddPredicateBuilder(nameof(CallListItem.UserField5), x => x.UserField5);
            AddPredicateBuilder(nameof(CallListItem.Number), x => x.Number);
            AddPredicateBuilder(nameof(CallListItem.EntityStateName), x => x.EntityStateName);
            AddPredicateBuilder(nameof(CallListItem.Grade), x => x.Grade);
            AddPredicateBuilder(nameof(CallListItem.Price), x => x.Price);
        }
    }

}
