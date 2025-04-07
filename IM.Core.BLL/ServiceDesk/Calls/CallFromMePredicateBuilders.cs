using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallFromMePredicateBuilders :
         FilterBuildersAggregate<Call, CallFromMeListItem>,
         ISelfRegisteredService<IAggregatePredicateBuilders<Call, CallFromMeListItem>>
    {
        public CallFromMePredicateBuilders() : base()
        {
            AddPredicateBuilder(nameof(CallFromMeListItem.ClientFullName), x => x.ClientID); 
            AddPredicateBuilder(nameof(CallFromMeListItem.ClientSubdivisionFullName), x => x.ClientSubdivisionID); 
            AddPredicateBuilder(nameof(CallFromMeListItem.ClientOrganizationName), x => x.ClientSubdivision.Organization.ID); 
            AddPredicateBuilder(nameof(CallFromMeListItem.InitiatorFullName), x => x.InitiatorID);
            AddPredicateBuilder(nameof(CallFromMeListItem.ReceiptTypeString), x => x.ReceiptType);
            AddPredicateBuilder(nameof(CallFromMeListItem.TypeFullName), x => x.CallType.ID);
            AddPredicateBuilder(nameof(CallFromMeListItem.Summary), x => x.CallSummaryName);
            AddPredicateBuilder(nameof(CallFromMeListItem.UrgencyName), x => x.Urgency.ID);
            AddPredicateBuilder(nameof(CallFromMeListItem.Description), x => DbFunctions.CastAsString(x.Description));
            AddPredicateBuilder(nameof(CallFromMeListItem.Solution), x => DbFunctions.CastAsString(x.Solution));
            AddPredicateBuilder(nameof(CallFromMeListItem.UtcDateRegistered), x => x.UtcDateRegistered);
            AddPredicateBuilder(nameof(CallFromMeListItem.UtcDatePromised), x => x.UtcDatePromised);
            AddPredicateBuilder(nameof(CallFromMeListItem.UtcDateAccomplished), x => x.UtcDateAccomplished);
            AddPredicateBuilder(nameof(CallFromMeListItem.UtcDateClosed), x => x.UtcDateClosed); 
            AddPredicateBuilder(nameof(CallFromMeListItem.UtcDateModified), x => x.UtcDateModified); 
            AddPredicateBuilder(nameof(CallFromMeListItem.UtcDateCreated), x => x.UtcDateCreated);
            AddPredicateBuilder(nameof(CallFromMeListItem.Price), x => x.Price);
            AddPredicateBuilder(nameof(CallFromMeListItem.Grade), x => x.Grade);          
            AddPredicateBuilder(nameof(CallFromMeListItem.Number), x => x.Number);
            AddPredicateBuilder(nameof(CallFromMeListItem.EntityStateName), x => x.EntityStateName);
            
        }
    }
}
