using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using System.Linq;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentQueryBuilder :
        IBuildEntityQuery<MassIncident, MassIncidentDetails, MassIncidentListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<MassIncident, MassIncidentDetails, MassIncidentListFilter>>
    {
        private readonly IReadonlyRepository<MassIncident> _repository;
        private readonly IReadonlyRepository<WorkOrder> _workOrders;

        public MassIncidentQueryBuilder(
            IReadonlyRepository<MassIncident> repository,
            IReadonlyRepository<WorkOrder> workOrders)
        {
            _repository = repository;
            _workOrders = workOrders;
        }

        public IExecutableQuery<MassIncident> Query(MassIncidentListFilter filterBy)
        {
            var refClassID = filterBy.ObjectClass;
            var refID = filterBy.ReferenceID;

            var ids = filterBy.GlobalIdentifiers ?? Array.Empty<Guid>();
            var query = _repository
                .With(x => x.FormValues)
                .ThenWithMany(x => x.Values)
                .ThenWith(x => x.FormField)
                .With(x=>x.CreatedBy)
                .With(x=>x.OwnedBy)
                .With(x=>x.ExecutedByUser)
                .WithMany(x => x.Calls)
                .WithMany(x => x.Problems)
                .WithMany(x => x.ChangeRequests)
                .DisableTrackingForQuery()
                .Query();

            if (refClassID.HasValue && refID.HasValue)
            {
                query = query
                    .Where(mi =>
                        (refClassID.Value == ObjectClass.Call && mi.Calls.Any(c => c.Reference.IMObjID == refID.Value))
                        || (refClassID.Value == ObjectClass.Problem && mi.Problems.Any(p => p.Reference.IMObjID == refID.Value))
                        || (refClassID.Value == ObjectClass.ChangeRequest && mi.ChangeRequests.Any(r => r.Reference.IMObjID == refID.Value))
                        || (refClassID.Value == ObjectClass.WorkOrder
                            && _workOrders
                                .With(wo => wo.WorkOrderReference)
                                .Any(wo =>
                                    wo.IMObjID == refID.Value
                                    && wo.WorkOrderReference.ObjectID == mi.IMObjID))
                    );

                return query.Where(mi => !ids.Any() || ids.Contains(mi.IMObjID));
            }

            return query.Where(massIncident => !refClassID.HasValue && !refID.HasValue && ids.Contains(massIncident.IMObjID));
        }
    }
}
