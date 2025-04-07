using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk;

internal class MassIncidentsToAssociateFilterPredicatesBuilder : IBuildListViewFilterPredicates<MassIncident, MassIncidentsToAssociateFilter>
{
    private readonly IValidateObjectPermissions<Guid, MassIncident> _permissionValidator;

    public MassIncidentsToAssociateFilterPredicatesBuilder(IValidateObjectPermissions<Guid, MassIncident> permissionValidator)
    {
        _permissionValidator = permissionValidator;
    }

    public IEnumerable<Expression<Func<MassIncident, bool>>> Build(Guid userID, MassIncidentsToAssociateFilter filter)
    {
        return _permissionValidator.ObjectIsAvailable(userID)
            .Union(massIncident => massIncident.Problems.All(p => p.Reference.IMObjID != filter.ProblemID));
    }
}