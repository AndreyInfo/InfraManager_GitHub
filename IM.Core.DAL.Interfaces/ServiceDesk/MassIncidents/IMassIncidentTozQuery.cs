using System;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    public interface IMassIncidentTozQuery
    {
        IQueryable<MassIncident> Query(Guid userID);
    }
}
