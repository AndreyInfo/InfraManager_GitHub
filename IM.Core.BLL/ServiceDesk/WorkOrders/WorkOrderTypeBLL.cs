using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.DAL.ServiceDesk;
using Inframanager.BLL;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderTypeBLL : 
        StandardBLL<Guid, WorkOrderType, WorkOrderTypeData, WorkOrderTypeDetails, LookupListFilter>,
        IWorkOrderTypeBLL, 
        ISelfRegisteredService<IWorkOrderTypeBLL>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<WorkOrderType> _repository;

        public WorkOrderTypeBLL(
            IRepository<WorkOrderType> repository, 
            ILogger<WorkOrderTypeBLL> logger, 
            IUnitOfWork unitOfWork, 
            ICurrentUser currentUser,
            IBuildObject<WorkOrderTypeDetails, WorkOrderType> detailsBuilder,
            IInsertEntityBLL<WorkOrderType, WorkOrderTypeData> insertEntityBLL, 
            IModifyEntityBLL<Guid, WorkOrderType, WorkOrderTypeData, WorkOrderTypeDetails> modifyEntityBLL, 
            IRemoveEntityBLL<Guid, WorkOrderType> removeEntityBLL, 
            IGetEntityBLL<Guid, WorkOrderType, WorkOrderTypeDetails> detailsBLL, 
            IGetEntityArrayBLL<Guid, WorkOrderType, WorkOrderTypeDetails, LookupListFilter> detailsArrayBLL,
            IMapper mapper) 
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
            _mapper = mapper;
        }

        public async Task<WorkOrderTypeDetails> UpdateAsync(Guid id, WorkOrderTypeData workOrderType,
            CancellationToken cancellationToken = default)
        {
            if (workOrderType.Default && await _repository.AnyAsync(x => x.Default && x.ID != id, cancellationToken))
            {
                throw new InvalidObjectException("Нельзя установить данный тип заданий как стандартный"); //TODO locale
            }
            return await base.UpdateAsync(id, workOrderType, cancellationToken);
        }

        
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            if (await _repository.AnyAsync(x => x.Default && x.ID == id, cancellationToken))
            {
                throw new InvalidObjectException("Нельзя удалить стандартный тип заданий"); //TODO locale
            }

            await base.DeleteAsync(id, cancellationToken);
        }
    }
}
