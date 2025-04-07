using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class NegotiationMassIncidentsListSubQuery : 
        IListQuery<MassIncident, NegotiationListSubQueryResultItem>,
        ISelfRegisteredService<IListQuery<MassIncident, NegotiationListSubQueryResultItem>>
    {
        private readonly DbContext _db;

        public NegotiationMassIncidentsListSubQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<NegotiationListSubQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<MassIncident, bool>>> predicates)
        {
            var massiveIncidents = _db.Set<MassIncident>().AsNoTracking().Where(predicates);

            return from massIncident in massiveIncidents 
                   let messagesCount = _db.Set<Note<MassIncident>>()
                    .Count(n => n.ParentObjectID == massIncident.IMObjID && n.Type == SDNoteType.Message)
                   let notesCount = _db.Set<Note<MassIncident>>()
                    .Count(n => n.ParentObjectID == massIncident.IMObjID && n.Type == SDNoteType.Note)
                   select new NegotiationListSubQueryResultItem
                   {
                       ObjectID = massIncident.IMObjID,
                       Number = massIncident.ID,
                       ClassID = ObjectClass.MassIncident,
                       CategorySortColumn = Issues.MassIncident,
                       Name = DbFunctions.CastAsString(massIncident.Name),
                       WorkflowSchemeIdentifier = massIncident.WorkflowSchemeIdentifier,
                       WorkflowSchemeVersion = massIncident.WorkflowSchemeVersion,
                       EntityStateName = massIncident.EntityStateName,
                       EntityStateID = massIncident.EntityStateID,
                       WorkflowSchemeID = massIncident.WorkflowSchemeID,
                       PriorityName = massIncident.Priority.Name,
                       PriorityColor = massIncident.Priority.Color,
                       PriorityID = massIncident.PriorityID,
                       PrioritySequence = massIncident.Priority.Sequence,
                       TypeFullName = DbFunctions.CastAsString(massIncident.Type.Name),
                       TypeID = massIncident.Type.IMObjID,
                       OwnerID = massIncident.OwnedBy.IMObjID,
                       ExecutorID = massIncident.ExecutedByUser.IMObjID,
                       QueueID = massIncident.ExecutedByGroupID,
                       UtcDateRegistered = massIncident.UtcRegisteredAt,
                       UtcDateModified = massIncident.UtcDateModified,
                       UtcDateClosed = massIncident.UtcDateClosed,                       
                       UtcDatePromised = DbFunctions.CastAsDateTime(massIncident.UtcCloseUntil),
                       UtcDateAccomplished = massIncident.UtcDateAccomplished,
                       ClientID = massIncident.CreatedBy.IMObjID,
                       ClientSubdivisionID = massIncident.CreatedBy.SubdivisionID,
                       ClientOrganizationID = massIncident.CreatedBy.Subdivision.OrganizationID,
                       ClientFullName = User.GetFullName(massIncident.CreatedBy.IMObjID),
                       ClientSubdivisionFullName = Subdivision.GetFullSubdivisionName(massIncident.CreatedBy.SubdivisionID),
                       ClientOrganizationName = DbFunctions.CastAsString(massIncident.CreatedBy.Subdivision.Organization.Name),
                       CanBePicked = false,
                       IsFinished = massIncident.UtcDateAccomplished != null,
                       NoteCount = notesCount,
                       MessagesCount = messagesCount
                   };
        }
    }
}
