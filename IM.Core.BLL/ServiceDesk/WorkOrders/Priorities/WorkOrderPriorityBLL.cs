using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Priorities;

internal class WorkOrderPriorityBLL :
    StandardBLL<Guid, WorkOrderPriority, WorkOrderPriorityData, WorkOrderPriorityDetails, LookupListFilter>,
    IWorkOrderPriorityBLL,
    ISelfRegisteredService<IWorkOrderPriorityBLL>

{
    private readonly IRepository<WorkOrderPriority> _repository;

    public WorkOrderPriorityBLL(
        IRepository<WorkOrderPriority> repository,
        ILogger<WorkOrderPriorityBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<WorkOrderPriorityDetails, WorkOrderPriority> detailsBuilder,
        IInsertEntityBLL<WorkOrderPriority, WorkOrderPriorityData> insertEntityBLL,
        IModifyEntityBLL<Guid, WorkOrderPriority, WorkOrderPriorityData, WorkOrderPriorityDetails> modifyEntityBLL,
        IRemoveEntityBLL<Guid, WorkOrderPriority> removeEntityBLL,
        IGetEntityBLL<Guid, WorkOrderPriority, WorkOrderPriorityDetails> detailsBLL,
        IGetEntityArrayBLL<Guid, WorkOrderPriority, WorkOrderPriorityDetails, LookupListFilter> detailsArrayBLL)
        : base(
              repository,
              logger,
              unitOfWork,
              currentUser,
              detailsBuilder,
              insertEntityBLL,
              modifyEntityBLL,
              removeEntityBLL,
              detailsBLL,
              detailsArrayBLL)
    {
        _repository = repository;
    }
    
    public async Task<WorkOrderPriorityDetails> UpdateAsync(Guid id, WorkOrderPriorityData workOrderType,
        CancellationToken cancellationToken = default)
    {
        if (workOrderType.Default && await _repository.AnyAsync(x => x.Default && x.ID != id, cancellationToken)) 
        {
            throw new InvalidObjectException("Нельзя установить данный приоритет задания как стандартный"); //TODO locale
        }
            
        return await base.UpdateAsync(id, workOrderType, cancellationToken);
    }


    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        if (await _repository.AnyAsync(x => x.Default && x.ID == id, cancellationToken))
        {
            throw new InvalidObjectException("Нельзя удалить стандартный приоритет задания"); //TODO locale
        }

        await base.DeleteAsync(id, cancellationToken);
    }
}
