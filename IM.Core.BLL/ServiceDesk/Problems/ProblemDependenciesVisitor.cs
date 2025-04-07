using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems;

public class ProblemDependenciesVisitor : 
    IVisitNewEntity<ProblemDependency>,
    ISelfRegisteredService<IVisitNewEntity<ProblemDependency>>,
    IVisitDeletedEntity<ProblemDependency>,
    ISelfRegisteredService<IVisitDeletedEntity<ProblemDependency>>
{
    private readonly IFinder<Problem> _problemFinder;

    public ProblemDependenciesVisitor(IFinder<Problem> problemFinder)
    {
        _problemFinder = problemFinder;
    }
    public void Visit(ProblemDependency entity)
    {
        SetProblemModified(entity.OwnerObjectID).GetAwaiter().GetResult();
    }

    public async Task VisitAsync(ProblemDependency entity, CancellationToken cancellationToken)
    {
        await SetProblemModified(entity.OwnerObjectID);
    }

    public void Visit(IEntityState originalState, ProblemDependency entity)
    {
        SetProblemModified(entity.OwnerObjectID).GetAwaiter().GetResult();
    }

    public async Task VisitAsync(IEntityState originalState, ProblemDependency entity, CancellationToken cancellationToken)
    {
        await SetProblemModified(entity.OwnerObjectID);
    }

    private async Task SetProblemModified(Guid problemId)
    {
        var problem = await _problemFinder.FindAsync(problemId);
        problem.UtcDateModified = DateTime.Now;
    }
}