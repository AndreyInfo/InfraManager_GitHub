using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.BLL.Workflow;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallModifier :
        IModifyObject<Call, CallData>,
        ISelfRegisteredService<IModifyObject<Call, CallData>>
    {
        private readonly IMapper _mapper;
        private readonly ICallServiceProvider _callServiceProvider;
        private readonly IReadonlyRepository<GroupUser> _groupUsers;
        private readonly ISetAgreement<Call> _slaSetter;
        private readonly IFinder<Group> _groupFinder;
        private readonly ICreateWorkflow<Call> _workflow;
        private readonly ISelectWorkflowScheme<Call> _workflowSchemeProvider;
        private readonly IModifyWorkOrderExecutorControl _modifyWorkOrderExecutorControl;

        public CallModifier(
            IMapper mapper,
            ICallServiceProvider callServiceProvider,
            IReadonlyRepository<GroupUser> groupUsers,
            ISetAgreement<Call> slaSetter,
            IFinder<Group> groupFinder,
            ICreateWorkflow<Call> workflow,
            ISelectWorkflowScheme<Call> workflowSchemeProvider,
            IServiceMapper<ObjectClass, IModifyWorkOrderExecutorControl> modifyWorkOrderExecutorControlMapper)
        {
            _mapper = mapper;
            _callServiceProvider = callServiceProvider;
            _groupUsers = groupUsers;
            _slaSetter = slaSetter;
            _groupFinder = groupFinder;
            _workflow = workflow;
            _workflowSchemeProvider = workflowSchemeProvider;
            _modifyWorkOrderExecutorControl = modifyWorkOrderExecutorControlMapper.Map(ObjectClass.Call);
        }

        public async Task ModifyAsync(Call call, CallData data, CancellationToken cancellationToken = default)
        {
            var typeChanged = data.CallTypeID.HasValue && data.CallTypeID != call.CallTypeID;
            var serviceItemOrAttendanceChanged = data.ServiceItemAttendanceID.HasValue
                && data.ServiceItemAttendanceID
                    != (call.CallService.ServiceItemID ?? call.CallService.ServiceAttendanceID);
            var workOrderExecutorControlChanged = data.OnWorkOrderExecutorControl.HasValue && data.OnWorkOrderExecutorControl.Value != call.OnWorkOrderExecutorControl;

            _mapper.Map(data, call);

            if (typeChanged)
            {
                call.WorkflowSchemeIdentifier = await _workflowSchemeProvider.SelectIdentifierAsync(call, cancellationToken);
            }

            if (serviceItemOrAttendanceChanged)
            {
                var callService = await _callServiceProvider.GetOrCreateAsync(data.ServiceItemAttendanceID, cancellationToken)
                    ?? throw new InvalidObjectException("Service item or attendance was either removed or not found."); // TODO: Ситуация маловероятна, но возможна и при штатной работе пользователя (надо локализовать текст ошибки и обработать на клиенте

                call.CallService = callService;

                call.WorkflowSchemeIdentifier = await _workflowSchemeProvider.SelectIdentifierAsync(call, cancellationToken);

                if (!call.WorkflowSchemeID.HasValue && !call.CallService.IsNull) // У заявки не было Workflow, а теперь он будет
                {
                    await _workflow.TryStartNewAsync(call, cancellationToken);
                }
            }

            await call.SetCallGroupIfNeededAsync(data.QueueID, _groupFinder, cancellationToken);

            // TODO: Переделать этот костыль в нормальное бизнес правило
            // поменяли группу и исполнитель не входит в группу - очищаем исполнителя 
            if (!data.QueueID.Ignore && data.QueueID.Value.HasValue && data.ExecutorID.Ignore && call.ExecutorID.HasValue
                && !await _groupUsers.AnyAsync(x => x.GroupID == data.QueueID.Value && x.UserID == call.ExecutorID, cancellationToken))
            {
                call.ExecutorID = null;
            }
            // поменяли исполнителя и исполнитель не входит в группу - очищаем группу
            if (data.QueueID.Ignore && call.QueueID.HasValue && !data.ExecutorID.Ignore && data.ExecutorID.Value.HasValue
                && !await _groupUsers.AnyAsync(x => x.GroupID == call.QueueID && x.UserID == data.ExecutorID.Value, cancellationToken))
            {
                call.SetGroup(null);
            }

            if (data.RefreshAgreement != null)
            {
                await _slaSetter.SetAsync(call,
                    cancellationToken, data.RefreshAgreement?.CountUtcCloseDateFrom, data.RefreshAgreement?.AgreementID);
            }

            if (workOrderExecutorControlChanged)
            {
                await _modifyWorkOrderExecutorControl
                    .SetUnderControlAsync(call.IMObjID, call.OnWorkOrderExecutorControl, cancellationToken);
            }
        }

        public void SetModifiedDate(Call call)
        {
            call.UtcDateModified = DateTime.UtcNow;
        }
    }
}