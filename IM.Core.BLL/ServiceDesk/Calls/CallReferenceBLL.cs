using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class CallReferenceBLL<TReference> : ICallReferenceBLL where TReference : class, IHaveUtcModifiedDate, IGloballyIdentifiedEntity
    {
        private readonly IRepository<CallReference<TReference>> _repository;
        private readonly IRepository<Call> _callRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFinder<Call> _callFinder;
        private readonly IFinder<TReference> _objectFinder;
        private readonly IValidatePermissions<Call> _callPermissionsValidator;
        private readonly IValidatePermissions<TReference> _objectPermissionsValidator;
        private readonly IValidateObjectPermissions<Guid, Call> _callAccessValidator;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger _logger;
        private readonly IGuidePaggingFacade<AllCallsQueryResultItem, CallListItem> _paggingFacade;
        private readonly IListQuery<Call, AllCallsQueryResultItem> _allCallsQuery;

        public CallReferenceBLL(
            IRepository<CallReference<TReference>> repository,
            IUnitOfWork unitOfWork,
            IFinder<Call> callFinder,
            IFinder<TReference> objectFinder,
            IValidatePermissions<Call> callPermissionsValidator,
            IValidatePermissions<TReference> objectPermissionsValidator,
            IValidateObjectPermissions<Guid, Call> callAccessValidator,
            IMapper mapper,
            ICurrentUser currentUser,
            ILogger<CallReferenceBLL<TReference>> logger,
            IRepository<Call> callRepository,
            IGuidePaggingFacade<AllCallsQueryResultItem, CallListItem> paggingFacade,
            IListQuery<Call, AllCallsQueryResultItem> allCallsQuery)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _callFinder = callFinder;
            _objectFinder = objectFinder;
            _callPermissionsValidator = callPermissionsValidator;
            _objectPermissionsValidator = objectPermissionsValidator;
            _callAccessValidator = callAccessValidator;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
            _callRepository = callRepository;
            _paggingFacade = paggingFacade;
            _allCallsQuery = allCallsQuery;
        }

        public async Task<CallReferenceData> AddAsync(CallReferenceData data, CancellationToken cancellationToken = default)
        {
            var flowID = Guid.NewGuid();            

            CallReference<TReference> reference;
            try
            {
                reference = new CallReference<TReference>(data.CallID, data.ObjectID);
            }
            catch(NotSupportedException)
            {
                throw new ObjectNotFoundException($"Call does not have references to entities of type {typeof(TReference)}");
            }
            _logger.LogTrace($"FlowID: {flowID}. User (ID = {_currentUser}) is attempting to add {reference}");

            await ValidatePermissionsOrRaiseErrorAsync(flowID, reference, cancellationToken);
            await ValidateCallPermissionsOrRaiseErrorAsync(flowID, reference, cancellationToken);
            await ValidateObjectPermissionsOrRaiseErrorAsync(flowID, reference, cancellationToken);

            _repository.Insert(reference);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) added {reference}.");

            return data;
        }

        public async Task DeleteAsync(CallReferenceData key, CancellationToken cancellationToken = default)
        {
            var flowID = Guid.NewGuid();
            _logger.LogTrace($"FlowID: {flowID}. User (ID = {_currentUser.UserId}) is attempting to delete reference between call (ID = {key.CallID}) and {typeof(TReference).Name} (ID = {key.ObjectID}).");

            var reference = await _repository.SingleOrDefaultAsync(x => x.CallID == key.CallID && x.ObjectID == key.ObjectID, cancellationToken)
                ?? throw new ObjectNotFoundException($"CallReference (CallID = {key.CallID}, {typeof(TReference).Name}ID = {key.ObjectID})");

            await ValidatePermissionsOrRaiseErrorAsync(flowID, reference, cancellationToken);
            await ValidateCallPermissionsOrRaiseErrorAsync(flowID, reference, cancellationToken);
            await ValidateObjectPermissionsOrRaiseErrorAsync(flowID, reference, cancellationToken);

            _repository.Delete(reference);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) removed {reference}.");
        }

        public async Task<CallReferenceData[]> GetAsync(CallReferenceListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var flowID = Guid.NewGuid();
            _logger.LogTrace(
                $"FlowID: {flowID}. User (ID = {_currentUser.UserId}) is requesting list of references between call and {typeof(TReference).Name} filtered by {filterBy}.");
            var query = _repository.Query();

            if (filterBy.CallID.HasValue)
            {
                query = query.Where(x => x.CallID == filterBy.CallID);
            }

            if (filterBy.ObjectID.HasValue)
            {
                query = query.Where(x => x.ObjectID == filterBy.ObjectID);
            }

            var references = await query.ExecuteAsync(cancellationToken);
            _logger.LogTrace($"FlowID: {flowID}. {references.Length} entities found.");

            return references.Select(reference => _mapper.Map<CallReference, CallReferenceData>(reference)).ToArray();
        }

        public async Task<CallListItem[]> GetReferencesAsync(CallReferenceListFilter filterBy,
            CancellationToken cancellationToken = default)
        {
            var allCallsQuery = _allCallsQuery.Query(_currentUser.UserId, Array.Empty<Expression<Func<Call, bool>>>());
            var callsIDs = (await GetAsync(filterBy, cancellationToken)).Select(x => x.CallID);

            var query = allCallsQuery.Where(z => callsIDs.Contains(z.ID));

            var result =  await _paggingFacade.GetPaggingAsync(filterBy,
                query,
                null,
                cancellationToken);

            return _mapper.Map<CallListItem[]>(result);
        }

        private async Task ValidatePermissionsOrRaiseErrorAsync(Guid flowID, CallReference<TReference> reference,
            CancellationToken cancellationToken = default)
        {
            if (!await _callPermissionsValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.Update, cancellationToken)
                && !await _objectPermissionsValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.Update, cancellationToken))
            {
                throw new AccessDeniedException($"FlowID: {flowID}. Add or delete {reference}.");
            }
        }

        private async Task ValidateCallPermissionsOrRaiseErrorAsync(Guid flowID, CallReference<TReference> reference,
            CancellationToken cancellationToken = default)
        {
            var call = await _callFinder.FindAsync(reference.CallID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(reference.CallID, ObjectClass.Call);
            _logger.LogTrace($"FlowID: {flowID}. Call exists.");

            if (!await _callAccessValidator.ObjectIsAvailableAsync(_currentUser.UserId, reference.CallID, cancellationToken))
            {
                throw new AccessDeniedException(reference.CallID, ObjectClass.Call);
            }
            _logger.LogTrace($"FlowID: {flowID}. Call is accessible.");
        }

        private async Task ValidateObjectPermissionsOrRaiseErrorAsync(Guid flowID, CallReference<TReference> reference,
            CancellationToken cancellationToken = default)
        {
            var referencedObject = await _objectFinder.FindAsync(reference.ObjectID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(reference.ObjectID, reference.ObjectClassID);
            _logger.LogTrace($"FlowID: {flowID}. {nameof(TReference)} exists.");

            if (!await _callAccessValidator.ObjectIsAvailableAsync(_currentUser.UserId, reference.CallID, cancellationToken))
            {
                throw new AccessDeniedException(reference.CallID, ObjectClass.Call);
            }
            _logger.LogTrace($"FlowID: {flowID}. Call is accessible.");
        }

    }
}
