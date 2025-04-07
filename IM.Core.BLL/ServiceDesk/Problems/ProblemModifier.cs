using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.BLL.Workflow;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.CustomControl;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class ProblemModifier :
        IModifyObject<Problem, ProblemData>,
        ISelfRegisteredService<IModifyObject<Problem, ProblemData>>

    {
        private readonly IMapper _mapper;
        private readonly ISelectWorkflowScheme<Problem> _workflowSchemeProvider;
        private readonly IModifyWorkOrderExecutorControl _modifyWorkOrderExecutorControl;

        public ProblemModifier(IMapper mapper,
            ISelectWorkflowScheme<Problem> workflowSchemeProvider,
            IServiceMapper<ObjectClass, IModifyWorkOrderExecutorControl> modifyWorkOrderExecutorControlMapper)
        {
            _mapper = mapper;
            _workflowSchemeProvider = workflowSchemeProvider;
            _modifyWorkOrderExecutorControl = modifyWorkOrderExecutorControlMapper.Map(ObjectClass.Problem);
        }

        public async Task ModifyAsync(Problem entity, ProblemData data, CancellationToken cancellationToken = default)
        {
            var typeChanged = data.TypeID.HasValue && data.TypeID.Value != entity.TypeID;
            var workOrderExecutorControlChanged = data.OnWorkOrderExecutorControl.HasValue && data.OnWorkOrderExecutorControl.Value != entity.OnWorkOrderExecutorControl;

            _mapper.Map(data, entity);

            if (workOrderExecutorControlChanged)
            {
                await _modifyWorkOrderExecutorControl
                    .SetUnderControlAsync(entity.IMObjID, entity.OnWorkOrderExecutorControl, cancellationToken);
            }

            if (typeChanged)
            {
                entity.WorkflowSchemeIdentifier = 
                    await _workflowSchemeProvider.SelectIdentifierAsync(entity, cancellationToken);
            }
        }

        public void SetModifiedDate(Problem entity)
        {
            entity.UtcDateModified = DateTime.UtcNow;
        }
    }
}
