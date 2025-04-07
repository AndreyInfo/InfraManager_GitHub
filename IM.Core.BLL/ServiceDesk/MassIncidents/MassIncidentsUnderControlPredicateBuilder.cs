using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentsUnderControlPredicateBuilder :
        FilterBuildersAggregate<MassIncident, ObjectUnderControl>,
        ISelfRegisteredService<IAggregatePredicateBuilders<MassIncident, ObjectUnderControl>>
    {
        public MassIncidentsUnderControlPredicateBuilder()
        {
            AddPredicateBuilder(listItem => listItem.CategoryName, x => Issues.MassIncident);
            AddPredicateBuilder(listItem => listItem.Name, x => x.Name);
            AddPredicateBuilder(listItem => listItem.PriorityName, x => x.PriorityID);
            AddPredicateBuilder(listItem => listItem.TypeFullName, x => x.TypeID);
            AddPredicateBuilder(listItem => listItem.OwnerFullName, x => x.OwnedBy.IMObjID);
            AddPredicateBuilder(listItem => listItem.ExecutorFullName, x => x.ExecutedByUser.IMObjID);
            AddPredicateBuilder(listItem => listItem.ClientFullName, x => x.CreatedBy.IMObjID);
            AddPredicateBuilder(listItem => listItem.ClientSubdivisionFullName, x => x.CreatedBy.SubdivisionID);
            AddPredicateBuilder(listItem => listItem.ClientOrganizationName, x => x.CreatedBy.Subdivision.OrganizationID);
            AddPredicateBuilder(listItem => listItem.UtcDateRegistered, x => x.UtcRegisteredAt);
            AddPredicateBuilder(listItem => listItem.UtcDatePromised, x => x.UtcCloseUntil);
            AddPredicateBuilder(listItem => listItem.UtcDateClosed, x => x.UtcDateClosed);
            AddPredicateBuilder(listItem => listItem.UtcDateAccomplished, x => x.UtcDateAccomplished);
            AddPredicateBuilder(listItem => listItem.UtcDateModified, x => x.UtcDateModified);
            AddPredicateBuilder(listItem => listItem.QueueName, x => x.ExecutedByGroupID);
            AddPredicateBuilder(listItem => listItem.EntityStateName, x => x.EntityStateName);
            AddPredicateBuilder(listItem => listItem.Number, x => x.ID);
        }
    }
}
