using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallBuilder : 
        IBuildObject<Call, CallData>, 
        ISelfRegisteredService<IBuildObject<Call, CallData>>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private IReadonlyRepository<Priority> _priorityRepository;
        private readonly ICallServiceProvider _callServiceProvider;
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
        private readonly IKnowledgeBaseBLL _knowledgeBaseBLL;
        private readonly ISetAgreement<Call> _slaSetter;
        private readonly IFinder<Group> _groupFinder;
        private readonly ICreateWorkflow<Call> _workflow;

        public CallBuilder(
            IMapper mapper,
            ICurrentUser currentUser,
            IReadonlyRepository<Priority> priorityRepository,
            ICallServiceProvider callServiceProvider,
            IFindEntityByGlobalIdentifier<User> userFinder,
            IKnowledgeBaseBLL knowledgeBaseBLL,
            ISetAgreement<Call> slaSetter,
            IFinder<Group> groupFinder,
            ICreateWorkflow<Call> workflow)
        {
            _mapper = mapper;
            _currentUser = currentUser;
            _priorityRepository = priorityRepository;
            _callServiceProvider = callServiceProvider;
            _userFinder = userFinder;
            _knowledgeBaseBLL = knowledgeBaseBLL;
            _slaSetter = slaSetter;
            _groupFinder = groupFinder;
            _workflow = workflow;
        }

        public async Task<Call> BuildAsync(CallData data, CancellationToken cancellationToken = default)
        {
            var call = _mapper.Map<Call>(data);
            call.ClientID = data.ClientID ?? _currentUser.UserId;
            if (data.CreatedByClient && call.ClientID != _currentUser.UserId)
            {
                call.InitiatorID = _currentUser.UserId;
            }

            if (!data.PriorityID.HasValue)
            {
                call.PriorityID = await _priorityRepository.GetDefaultPriorityIDAsync(cancellationToken: cancellationToken)
                    ?? throw new InvalidObjectException("Priority is missing"); //TODO локализация
            }

            call.CallService = await _callServiceProvider.GetOrCreateAsync(data.ServiceItemAttendanceID, cancellationToken);

            if (!call.ClientSubdivisionID.HasValue)
            {
                var user = await _userFinder.FindAsync(call.ClientID, cancellationToken);
                call.ClientSubdivisionID = user.SubdivisionID;
            }

            await call.SetCallGroupIfNeededAsync(data.QueueID, _groupFinder, cancellationToken);
            
            await _slaSetter.SetAsync(call, cancellationToken);
            
            if (data.KBArticleID.HasValue)
            {
                await _knowledgeBaseBLL.AttachReferenceAsync(
                    data.KBArticleID.Value,
                    new InframanagerObject(call.IMObjID, ObjectClass.Call),
                    cancellationToken);

                //Присваиваем нулевые значения и статуса "Закрыта" в случае если испольовалась статья из БД. (перенос старого функционала). И не делаем присвоения Workflow
                //TOGO: Логика из легаси. Пока оставляем как есть
                var finishState = "Закрыта";
                call.EntityStateName = finishState;
            }
            else
                await _workflow.TryStartNewAsync(call, cancellationToken);

            return call;
        }

        public Task<IEnumerable<Call>> BuildManyAsync(IEnumerable<CallData> dataItems, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException(); // TODO: реализовать если понадобится
        }
    }
}
