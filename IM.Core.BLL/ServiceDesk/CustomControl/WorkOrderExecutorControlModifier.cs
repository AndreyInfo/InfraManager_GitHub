using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.CustomControl;

internal class WorkOrderExecutorControlModifier<TEntity> : IModifyWorkOrderExecutorControl
    where TEntity : IWorkOrderExecutorControl
{
    private readonly IFinder<TEntity> _finder;
    private readonly IEditCustomControl _editCustomControl;
    private readonly IReadonlyRepository<WorkOrder> _repository;
    private readonly IObjectClassProvider<TEntity> _classProvider;

    public WorkOrderExecutorControlModifier(
        IFinder<TEntity> finder,
        IEditCustomControl editCustomControl,
        IReadonlyRepository<WorkOrder> repository,
        IObjectClassProvider<TEntity> classProvider)
    {
        _finder = finder;
        _editCustomControl = editCustomControl;
        _repository = repository;
        _classProvider = classProvider;
    }

    public async Task SetUnderControlIfNeededAsync(Guid objectID, Guid userID, CancellationToken cancellationToken = default)
    {
        var entity = await _finder.FindOrRaiseErrorAsync(objectID, cancellationToken);

        if (entity.OnWorkOrderExecutorControl)
        {
            var imObject = new InframanagerObject(entity.IMObjID, _classProvider.GetObjectClass());
            await _editCustomControl.SetCustomControlAsync(imObject, userID, true, cancellationToken);
        }
    }

    public async Task RemoveUnderControlIfNeededAsync(WorkOrderReference reference, Guid userID, CancellationToken cancellationToken = default)
    {
        var entity = await _finder.FindOrRaiseErrorAsync(reference.ObjectID, cancellationToken);

        var imObject = new InframanagerObject(entity.IMObjID, _classProvider.GetObjectClass());

        var hasAnyOtherReferences = 1 < await _repository
            .CountAsync(x => x.WorkOrderReferenceID == reference.ID && x.ExecutorID == userID, cancellationToken);

        if (!hasAnyOtherReferences)
        {
            await _editCustomControl.SetCustomControlAsync(imObject, userID, false, cancellationToken);
        }
    }

    public async Task SetUnderControlAsync(Guid objectID, bool underControl, CancellationToken cancellationToken)
    {
        var imObject = new InframanagerObject(objectID, _classProvider.GetObjectClass());

        var executorIDList = (await _repository
                .ToArrayAsync(x => x.ExecutorID.HasValue
                                   && x.WorkOrderReference.ObjectClassID == imObject.ClassId
                                   && x.WorkOrderReference.ObjectID == imObject.Id,
                    cancellationToken))
            .Select(x => x.ExecutorID.Value)
            .Distinct();

        foreach (var executorID in executorIDList)
        {
            await _editCustomControl.SetCustomControlAsync(imObject, executorID, underControl, cancellationToken);
        }
    }
}