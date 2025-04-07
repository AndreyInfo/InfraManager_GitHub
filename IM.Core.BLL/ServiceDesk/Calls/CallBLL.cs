using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.Documents;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallBLL :
        StandardBLL<Guid, Call, CallData, CallDetails, CallListFilter>,
        ICallBLL,
        ISelfRegisteredService<ICallBLL>
    {
        #region .ctor

        private readonly IListViewBLL<CallListItem, ServiceDeskListFilter> _allCallsBll;
        private readonly IListViewBLL<CallFromMeListItem, CallFromMeListFilter> _callsFromMeBll;
        private readonly IFindEntityByGlobalIdentifier<Call> _finder;
        private readonly ILoadEntity<Guid, Call, CallDetails> _loader;
        private readonly IRepository<DocumentReference> _documentReferenceRepository;
        private readonly IReadonlyRepository<MassIncident> _massIncidents;
        private readonly ILightListViewBLL<CallReferenceListItem> _callReferencesListView;
        private readonly IUserAccessBLL _userAccess;
        private readonly IValidateObjectPermissions<Guid, Call> _objectAccessValidator;

        public CallBLL(
            IRepository<Call> repository,
            ILogger<CallBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<CallDetails, Call> detailsBuilder,
            IInsertEntityBLL<Call, CallData> insertEntityBLL,
            IModifyEntityBLL<Guid, Call, CallData, CallDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, Call> removeEntityBLL,
            IGetEntityBLL<Guid, Call, CallDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, Call, CallDetails, CallListFilter> detailsArrayBLL,
            IListViewBLL<CallListItem, ServiceDeskListFilter> allCallsBll,
            IListViewBLL<CallFromMeListItem, CallFromMeListFilter> callsFromMeBll,
            IFindEntityByGlobalIdentifier<Call> finder,
            ILoadEntity<Guid, Call, CallDetails> loader,
            IRepository<DocumentReference> documentReferenceRepository,
            IReadonlyRepository<MassIncident> massIncidents,
            ILightListViewBLL<CallReferenceListItem> callReferencesListView,
            IUserAccessBLL userAccess,
            IValidateObjectPermissions<Guid, Call> objectAccessValidator)
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
            _allCallsBll = allCallsBll;
            _callsFromMeBll = callsFromMeBll;
            _finder = finder;
            _loader = loader;
            _documentReferenceRepository = documentReferenceRepository;
            _massIncidents = massIncidents;
            _callReferencesListView = callReferencesListView;
            _userAccess = userAccess;
            _objectAccessValidator = objectAccessValidator;
        }


        #endregion

        #region Reports

        public Task<CallListItem[]> AllCallsAsync(
            ListViewFilterData<ServiceDeskListFilter> filterBy,
            CancellationToken cancellationToken = default)
        {
            return _allCallsBll.BuildAsync(filterBy, cancellationToken);
        }

        public Task<CallFromMeListItem[]> CallsFromMeAsync(
            ListViewFilterData<CallFromMeListFilter> filterBy,
            CancellationToken cancellationToken = default)
        {
            return _callsFromMeBll.BuildAsync(filterBy, cancellationToken);
        }

        public async Task<CallReferenceListItem[]> GetAvailableMassIncidentReferencesAsync(InframanagerObjectListViewFilter filterBy, CancellationToken cancellationToken = default)
        {
            var operation = "calls available to be referenced by mass incident";
            var flowID = Guid.NewGuid();
            Logger.LogInformation($"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) requesting {operation} (ID = {filterBy.IMObjID})");

            if (!await _userAccess.UserHasOperationAsync(CurrentUser.UserId, OperationID.MassIncident_Update, cancellationToken))
            {
                throw new AccessDeniedException($"View {operation}");
            }
            Logger.LogTrace($"FlowID: {flowID}. Access is granted");
                
            var query = Repository
                .Query(_objectAccessValidator.ObjectIsAvailable(CurrentUser.UserId).Intersect())
                .Except(
                    _massIncidents
                        .Query(massIncident => massIncident.IMObjID == filterBy.IMObjID)
                        .SelectMany(x => x.Calls)
                        .Select(x => x.Reference));

            return await _callReferencesListView.GetListItemsPageAsync(query, filterBy, cancellationToken);
        }
        #endregion

 
    }
}
