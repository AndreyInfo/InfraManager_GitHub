using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk;

internal class ProblemMassIncidentFilterPredicatesBuilder : IBuildListViewFilterPredicates<MassIncident, ProblemMassIncidentFilter>
{
    private readonly IValidateObjectPermissions<Guid, MassIncident> _massIncidentPermissionValidator;
    private readonly IValidateObjectPermissions<Guid, Problem> _problemPermissionValidator;

    public ProblemMassIncidentFilterPredicatesBuilder(
        IValidateObjectPermissions<Guid, MassIncident> massIncidentPermissionValidator,
        IValidateObjectPermissions<Guid, Problem> problemPermissionValidator)
    {
        _massIncidentPermissionValidator = massIncidentPermissionValidator;
        _problemPermissionValidator = problemPermissionValidator;
    }

    public IEnumerable<Expression<Func<MassIncident, bool>>> Build(Guid userID, ProblemMassIncidentFilter filter)
    {     
        
        if (!_problemPermissionValidator.ObjectIsAvailableAsync(userID, filter.ProblemID.Value).GetAwaiter().GetResult())
        {
            return new Expression<Func<MassIncident, bool>>[] { new Specification<MassIncident>(false) };
        }

        var specification = filter.ProblemID.HasValue
            ? new Specification<MassIncident>(massIncident => massIncident.Problems.Any(p => p.Reference.IMObjID == filter.ProblemID.Value))
            : new Specification<MassIncident>(true);


        if (filter.IDList != null && filter.IDList.Any())
        {
            specification = new IDListSpecification<MassIncident>(filter.IDList) && specification;
        }

        return _massIncidentPermissionValidator
            .ObjectIsAvailable(userID)
            .Union(specification);
    }
}