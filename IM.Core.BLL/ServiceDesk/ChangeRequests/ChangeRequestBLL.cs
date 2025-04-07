using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestBLL :
        StandardBLL<Guid, ChangeRequest, ChangeRequestData, ChangeRequestDetails, ChangeRequestListFilter>,
        IChangeRequestBLL,
        ISelfRegisteredService<IChangeRequestBLL>
    {

        #region .ctor

        private readonly IListViewBLL<ChangeRequestListItem, ServiceDeskListFilter> _dataListBuilder;
        private readonly IFindEntityByGlobalIdentifier<ChangeRequest> _finder;
        private readonly ILoadEntity<Guid, ChangeRequest, ChangeRequestDetails> _loader;
        private readonly IReadonlyRepository<MassIncident> _massIncidents;
        private readonly IValidateObjectPermissions<Guid, ChangeRequest> _objectAccessValidator;
        private readonly ILightListViewBLL<ChangeRequestReferenceListItem> _referenceListView;
        private readonly IUserAccessBLL _userAccess;

        public ChangeRequestBLL(
            IRepository<ChangeRequest> repository,
            ILogger<ChangeRequestBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<ChangeRequestDetails, ChangeRequest> detailsBuilder,
            IInsertEntityBLL<ChangeRequest, ChangeRequestData> insertEntityBLL,
            IModifyEntityBLL<Guid, ChangeRequest, ChangeRequestData, ChangeRequestDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, ChangeRequest> removeEntityBLL,
            IGetEntityBLL<Guid, ChangeRequest, ChangeRequestDetails> detailsBLL,
            ILoadEntity<Guid, ChangeRequest, ChangeRequestDetails> loader,
            IGetEntityArrayBLL<Guid, ChangeRequest, ChangeRequestDetails, ChangeRequestListFilter> detailsArrayBLL,
            IListViewBLL<ChangeRequestListItem, ServiceDeskListFilter> dataListBuilder,
            IFindEntityByGlobalIdentifier<ChangeRequest> finder,
            IReadonlyRepository<MassIncident> massIncidents,
            IValidateObjectPermissions<Guid, ChangeRequest> objectAccessValidator,
            ILightListViewBLL<ChangeRequestReferenceListItem> referenceListView,
            IUserAccessBLL userAccess)
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
            _dataListBuilder = dataListBuilder;
            _finder = finder;
            _loader = loader;
            _massIncidents = massIncidents;
            _objectAccessValidator = objectAccessValidator;
            _referenceListView = referenceListView;
            _userAccess = userAccess;
        }

        #endregion

        public Task<ChangeRequestListItem[]> GetChangeRequestsAsync(ListViewFilterData<ServiceDeskListFilter> filterBy, CancellationToken cancellationToken = default)
        {
            return _dataListBuilder.BuildAsync(filterBy, cancellationToken);
        }

        //TODO: Это копипаста из CallBLL
        public async Task<ChangeRequestReferenceListItem[]> GetAvailableMassIncidentReferencesAsync(InframanagerObjectListViewFilter filterBy, CancellationToken cancellationToken = default)
        {
            var operation = "change requests available to be referenced by mass incident";
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
                        .SelectMany(x => x.ChangeRequests)
                        .Select(x => x.Reference));

            return await _referenceListView.GetListItemsPageAsync(query, filterBy, cancellationToken);
        }

    }
}
