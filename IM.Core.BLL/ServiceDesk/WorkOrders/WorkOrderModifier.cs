using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.BLL.Workflow;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class WorkOrderModifier :
        IModifyObject<WorkOrder, WorkOrderData>,
        ISelfRegisteredService<IModifyObject<WorkOrder, WorkOrderData>>
    {
        private readonly IMapper _mapper;
        private readonly IProvideWorkOrderReference _referenceProvider;
        private readonly IReadonlyRepository<GroupUser> _groupUsers;
        private readonly ISelectWorkflowScheme<WorkOrder> _workflowSchemeProvider;
        private readonly IServiceMapper<ObjectClass, IModifyWorkOrderExecutorControl> _underControlModifierMapper;

        public WorkOrderModifier(
            IMapper mapper,
            IProvideWorkOrderReference referenceProvider,
            IReadonlyRepository<GroupUser> groupUsers,
            ISelectWorkflowScheme<WorkOrder> workflowSchemeProvider,
            IServiceMapper<ObjectClass, IModifyWorkOrderExecutorControl> underControlModifierMapper)
        {
            _mapper = mapper;
            _referenceProvider = referenceProvider;
            _groupUsers = groupUsers;
            _workflowSchemeProvider = workflowSchemeProvider;
            _underControlModifierMapper = underControlModifierMapper;
        }

        public async Task ModifyAsync(WorkOrder entity, WorkOrderData data, CancellationToken cancellationToken = default)
        {
            var typeChanged = data.TypeID.HasValue && entity.TypeID != data.TypeID;
            var executorID = entity.ExecutorID;
            var workOrderReference = entity.WorkOrderReference;

            _mapper.Map(data, entity);

            // TODO: Переделать этот костыль в нормальное бизнес правило
            // поменяли группу и исполнитель не входит в группу - очищаем исполнителя 
            if (!data.QueueID.Ignore && data.QueueID.Value.HasValue && data.ExecutorID.Ignore && entity.ExecutorID.HasValue
                && !await _groupUsers.AnyAsync(x => x.GroupID == data.QueueID.Value && x.UserID == entity.ExecutorID, cancellationToken))
            {
                entity.ExecutorID = null;
            }
            // поменяли исполнителя и исполнитель не входит в группу - очищаем группу
            if (data.QueueID.Ignore && entity.QueueID.HasValue && !data.ExecutorID.Ignore && data.ExecutorID.Value.HasValue
                && !await _groupUsers.AnyAsync(x => x.GroupID == entity.QueueID && x.UserID == data.ExecutorID.Value, cancellationToken))
            {
                entity.QueueID = null;
            }

            if (data.ReferencedObject.HasValue)
            {
                entity.WorkOrderReference = await _referenceProvider.GetOrCreateAsync(data.ReferencedObject, cancellationToken);
            }

            if (!entity.WorkOrderReference.IsDefault && entity.ExecutorID.HasValue && _underControlModifierMapper.HasKey(entity.WorkOrderReference.ObjectClassID))
            {
                await _underControlModifierMapper
                    .Map(entity.WorkOrderReference.ObjectClassID)
                    .SetUnderControlIfNeededAsync(entity.WorkOrderReference.ObjectID, entity.ExecutorID.Value, cancellationToken);
            }

            if (typeChanged)
            {
                entity.WorkflowSchemeIdentifier = await _workflowSchemeProvider.SelectIdentifierAsync(entity, cancellationToken);
            }
        }

        public void SetModifiedDate(WorkOrder entity)
        {
            entity.UtcDateModified = DateTime.UtcNow;
        }
    }
}
