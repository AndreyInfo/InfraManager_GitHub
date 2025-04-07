using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrders;

public class WorkOrderProblemUpdater :
    IVisitModifiedEntity<WorkOrder>,
    ISelfRegisteredService<IVisitModifiedEntity<WorkOrder>>,
    IVisitNewEntity<WorkOrder>,
    ISelfRegisteredService<IVisitNewEntity<WorkOrder>>
{
    private readonly IFindEntityByGlobalIdentifier<Problem> _problemFinder;
    private readonly IFinder<WorkOrderReference> _referenceFinder;

    public WorkOrderProblemUpdater(IFindEntityByGlobalIdentifier<Problem> problemFinder, IFinder<WorkOrderReference> referenceFinder)
    {
        _problemFinder = problemFinder;
        _referenceFinder = referenceFinder;
    }
    
    public void Visit(IEntityState originalState, WorkOrder currentState)
    {
        var previousReferenceID = (long)originalState[nameof(WorkOrder.WorkOrderReferenceID)];
        if (currentState.WorkOrderReferenceID != previousReferenceID)
        {
            var previousReference = _referenceFinder.Find(previousReferenceID); 
            var currentReference = _referenceFinder.Find(currentState.WorkOrderReferenceID);
            var refToUpdate = !currentReference.IsDefault ? currentReference : previousReference;
            UpdateProblemIfNeededAsync(refToUpdate, CancellationToken.None).GetAwaiter().GetResult();
        }
    }

    public async Task VisitAsync(IEntityState originalState, WorkOrder currentState, CancellationToken cancellationToken)
    {
        var previousReferenceID = (long)originalState[nameof(WorkOrder.WorkOrderReferenceID)];
        if (currentState.WorkOrderReferenceID != previousReferenceID)
        {
            var previousReference = await _referenceFinder.FindAsync(previousReferenceID, cancellationToken: cancellationToken); 
            var currentReference = await _referenceFinder.FindAsync(currentState.WorkOrderReferenceID, cancellationToken: cancellationToken);
            var refToUpdate = !currentReference.IsDefault ? currentReference : previousReference;
            await UpdateProblemIfNeededAsync(refToUpdate, CancellationToken.None);
        }
    }

    public void Visit(WorkOrder entity)
    {
        var reference = _referenceFinder.Find(entity.WorkOrderReferenceID);
        UpdateProblemIfNeededAsync(reference, CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task VisitAsync(WorkOrder entity, CancellationToken cancellationToken)
    {
        var reference = await _referenceFinder.FindAsync(entity.WorkOrderReferenceID, cancellationToken);
        await UpdateProblemIfNeededAsync(reference, cancellationToken);
    }
    
    private async Task UpdateProblemIfNeededAsync(WorkOrderReference reference, CancellationToken cancellationToken)
    {
        if (reference.ObjectClassID == ObjectClass.Problem)
        {
            var problem = await _problemFinder.FindAsync(reference.ObjectID, cancellationToken);
            problem.UtcDateModified = DateTime.UtcNow;
        }
    }
}