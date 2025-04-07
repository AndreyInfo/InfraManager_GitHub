using InfraManager.DAL.Documents;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class AllMassIncidentsQuery : IListQuery<MassIncident, AllMassIncidentsReportQueryResultItem>,
        ISelfRegisteredService<IListQuery<MassIncident, AllMassIncidentsReportQueryResultItem>>
    {
        private readonly DbContext _db;

        public AllMassIncidentsQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<AllMassIncidentsReportQueryResultItem> Query(
            Guid userId, 
            IEnumerable<Expression<Func<MassIncident, bool>>> predicates)
        {
            var massiveIncidents = _db.Set<MassIncident>()
                .AsNoTracking()
                .Where(predicates);

            return from massIncident in massiveIncidents
                   join cause in _db.Set<MassIncidentCause>()
                    on massIncident.CauseID equals cause.ID into causeTemp
                    from cause in causeTemp.DefaultIfEmpty()             
                   join _group in _db.Set<Group>()
                    on massIncident.ExecutedByGroupID equals _group.IMObjID into groupsTemp
                    from _group in groupsTemp.DefaultIfEmpty()
                   join ola in _db.Set<OperationalLevelAgreement>()
                    on massIncident.OperationalLevelAgreementID equals ola.ID into slaTemp
                    from sla in slaTemp.DefaultIfEmpty()
                   let documentsQuantity = _db.Set<DocumentReference>().Count(x => x.ObjectID == massIncident.IMObjID)
                   select new AllMassIncidentsReportQueryResultItem
                   {
                       ID = massIncident.ID,
                       IMObjID = massIncident.IMObjID,
                       ServiceName = massIncident.Service.Name,
                       InformationChannelID = massIncident.InformationChannelID,
                       Priority = massIncident.Priority.Name,
                       PriorityColor = massIncident.Priority.Color,
                       PrioritySequence = massIncident.Priority.Sequence,
                       Criticality = massIncident.Criticality.Name,
                       Type = massIncident.Type.Name,
                       Cause = cause.Name,
                       DocumentsQuantity = documentsQuantity,
                       ShortDescription = massIncident.Name,
                       FullDescription = massIncident.Description.Plain,
                       Solution = massIncident.Solution.Plain,
                       WorkflowSchemeIdentifier = massIncident.WorkflowSchemeIdentifier,
                       EntityStateName = massIncident.EntityStateName,
                       UtcCreatedAt = massIncident.UtcCreatedAt,
                       UtcLastModifiedAt = massIncident.UtcDateModified,
                       UtcOpenedAt = massIncident.UtcOpenedAt,
                       UtcRegisteredAt = massIncident.UtcRegisteredAt,
                       UtcCloseUntil = massIncident.UtcCloseUntil,
                       InitiatorID = massIncident.CreatedBy.IMObjID,
                       CreatedByUserName = User.GetFullName(massIncident.CreatedBy.IMObjID),
                       OwnerID = massIncident.OwnedBy == null ? User.NullUserGloablIdentifier : massIncident.OwnedBy.IMObjID,
                       OwnedByUserName = User.GetFullName(massIncident.OwnedBy.IMObjID),
                       GroupName = _group.Name,
                       ServiceLevelAgreement = sla.Name,
                       WorkflowSchemeID = massIncident.WorkflowSchemeID,
                       WorkflowSchemeVersion = massIncident.WorkflowSchemeVersion
                   };
        }
    }
}
