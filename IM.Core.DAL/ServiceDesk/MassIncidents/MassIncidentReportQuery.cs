using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk.MassIncidents;

internal class MassIncidentReportQuery :
    IListQuery<MassIncident, MassIncidentsReportQueryResultItem>,
    ISelfRegisteredService<IListQuery<MassIncident, MassIncidentsReportQueryResultItem>>
{
    private readonly DbSet<MassIncident> _massIncidents;
    private readonly DbSet<MassIncidentCause> _causes;

    public MassIncidentReportQuery(DbSet<MassIncident> massIncidents, DbSet<MassIncidentCause> causes)
    {
        _massIncidents = massIncidents;
        _causes = causes;
    }

    public IQueryable<MassIncidentsReportQueryResultItem> Query(Guid userId, IEnumerable<Expression<Func<MassIncident, bool>>> predicates)
    {
        var massIncidents = _massIncidents.Where(predicates);

        return from massIncident in massIncidents
            join cause in _causes.AsNoTracking()
                on massIncident.CauseID equals cause.ID into causeTemp
            from cause in causeTemp.DefaultIfEmpty()
            select new MassIncidentsReportQueryResultItem
            {
                ID = massIncident.ID,
                IMObjID = massIncident.IMObjID,
                Type = massIncident.Type.Name,
                Summary = massIncident.Name,
                ServiceName = massIncident.Service.Name,
                Cause = cause.Name,
                Owner = User.GetFullName(massIncident.OwnedBy.IMObjID),
                Initiator = User.GetFullName(massIncident.CreatedBy.IMObjID),
                Priority = massIncident.Priority.Name,
                EntityStateName = massIncident.EntityStateName,
                UtcCreatedAt = massIncident.UtcCreatedAt,
                UtcLastModifiedAt = massIncident.UtcDateModified,
            };
    }
}