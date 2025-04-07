using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class MassIncidentTozQuery : IMassIncidentTozQuery, ISelfRegisteredService<IMassIncidentTozQuery>
    {
        private readonly DbSet<MassIncident> _massIncidents;

        public MassIncidentTozQuery(DbSet<MassIncident> massIncidents)
        {
            _massIncidents = massIncidents;
        }

        public IQueryable<MassIncident> Query(Guid userID)
        {
            return _massIncidents.Where(
                massIncident => 
                       DbFunctions.AccessIsGranted(ObjectClass.Service, massIncident.ServiceID, userID, ObjectClass.User, AccessTypes.TOZ_sks, false)
                    || (massIncident.CreatedByUserID != User.NullUserId 
                        && DbFunctions.AccessIsGranted(ObjectClass.User, massIncident.CreatedBy.IMObjID, userID, ObjectClass.User, AccessTypes.TOZ_org, false))
                    || (massIncident.OwnedByUserID != User.NullUserId
                        && DbFunctions.AccessIsGranted(ObjectClass.User, massIncident.OwnedBy.IMObjID, userID, ObjectClass.User, AccessTypes.TOZ_org, false))
                    || (massIncident.ExecutedByUserID != User.NullUserId
                        && DbFunctions.AccessIsGranted(ObjectClass.User, massIncident.ExecutedByUser.IMObjID, userID, ObjectClass.User, AccessTypes.TOZ_org, false)));
        }

    }
}
