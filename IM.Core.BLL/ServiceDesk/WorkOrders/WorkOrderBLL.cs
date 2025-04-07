using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using InfraManager.BLL.AccessManagement;
using InfraManager.DAL.Documents;
using InfraManager.BLL.WorkFlow;
using InfraManager.DAL.Events;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderBLL :
        StandardBLL<Guid, WorkOrder, WorkOrderData, WorkOrderDetails, WorkOrderListFilter>,
        IWorkOrderBLL,
        ISelfRegisteredService<IWorkOrderBLL>
    {
        private readonly IListViewBLL<WorkOrderListItem, ServiceDeskListFilter> _dataWorkOrderListBuilder;
        private readonly IListViewBLL<InventoryListItem, ServiceDeskListFilter> _dataInventoryListBuilder;
        private readonly IListViewBLL<ReferencedWorkOrderListItem, WorkOrderListFilter> _referencedWorkOrderListBuilder;
        private readonly IFindEntityByGlobalIdentifier<WorkOrder> _finder;
        private readonly ILoadEntity<Guid, WorkOrder, WorkOrderDetails> _loader;
        private readonly IRepository<DocumentReference> _documentReferenceRepository;
        private readonly IRepository<WorkOrderReference> _workOrderReferences;
        private readonly IUserAccessBLL _userAccess;
        private readonly ILightListViewBLL<WorkOrderReferenceListItem> _referenceListView;
        private readonly ISendWorkflowEntityEvent<WorkOrder> _workflowEventSender;

        public WorkOrderBLL(
            IRepository<WorkOrder> repository,
            ILogger<WorkOrderBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<WorkOrderDetails, WorkOrder> detailsBuilder,
            IInsertEntityBLL<WorkOrder, WorkOrderData> insertEntityBLL,
            IModifyEntityBLL<Guid, WorkOrder, WorkOrderData, WorkOrderDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, WorkOrder> removeEntityBLL,
            IGetEntityBLL<Guid, WorkOrder, WorkOrderDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, WorkOrder, WorkOrderDetails, WorkOrderListFilter> detailsArrayBLL,
            IListViewBLL<WorkOrderListItem, ServiceDeskListFilter> dataWorkOrderListBuilder,
            IListViewBLL<InventoryListItem, ServiceDeskListFilter> dataInventoryListBuilder,
            IFindEntityByGlobalIdentifier<WorkOrder> finder,
            ILoadEntity<Guid, WorkOrder, WorkOrderDetails> loader,
            IRepository<DocumentReference> documentReferenceRepository,
            IRepository<WorkOrderReference> workOrderReferences,
            IUserAccessBLL userAccess,
            ILightListViewBLL<WorkOrderReferenceListItem> referenceListView,
            IListViewBLL<ReferencedWorkOrderListItem, WorkOrderListFilter> referencedWorkOrderListBuilder,
            ISendWorkflowEntityEvent<WorkOrder> workflowEventSender)
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
            _dataWorkOrderListBuilder = dataWorkOrderListBuilder;
            _dataInventoryListBuilder = dataInventoryListBuilder;
            _finder = finder;
            _loader = loader;
            _documentReferenceRepository = documentReferenceRepository;
            _workOrderReferences = workOrderReferences;
            _userAccess = userAccess;
            _referenceListView = referenceListView;
            _referencedWorkOrderListBuilder = referencedWorkOrderListBuilder;
            _workflowEventSender = workflowEventSender;
        }

        #region StandardBLL

        protected override async Task EntityAddedAsync(WorkOrder entity)
        {
            _workflowEventSender.Send(entity, EventType.EntityCreated);
            await UnitOfWork.SaveAsync(); // без возможности отмены
        }

        #endregion

        public async Task<ReferencedWorkOrderListItem[]> Get(ListViewFilterData<WorkOrderListFilter> filter,
            CancellationToken cancellationToken = default)
        {
            return await _referencedWorkOrderListBuilder.BuildAsync(filter, cancellationToken);
        }
        
        public Task<WorkOrderListItem[]> GetAllWorkOrdersAsync(
            ListViewFilterData<ServiceDeskListFilter> filterBy,
            CancellationToken cancellationToken = default)
        {
            return _dataWorkOrderListBuilder.BuildAsync(filterBy, cancellationToken);
        }

        public Task<InventoryListItem[]> GetInventoryReportAsync(
            ListViewFilterData<ServiceDeskListFilter> filterBy,
            CancellationToken cancellationToken = default)
        {
            return _dataInventoryListBuilder.BuildAsync(filterBy, cancellationToken);
        }

        public Task<WorkOrderDetails[]> GetDetailsPageAsync(WorkOrderListFilter filter, CancellationToken cancellationToken = default)
        {
            return StandardBLLExtensions.GetDetailsPageAsync(this, filter, cancellationToken);
        }

        public async Task<WorkOrderReference[]> GetReferencesAsync(Guid id, CancellationToken cancellationToken)
        {
            var workOrder = await _finder.FindAsync(id, cancellationToken)
                ?? throw new ObjectNotFoundException($"WorkOrder not found: {id}.");

            return await _workOrderReferences.DisableTrackingForQuery()
                .ToArrayAsync(x => x.ID == workOrder.WorkOrderReferenceID, cancellationToken);
        }

        //TODO: копипаста CallBLL
        public async Task<WorkOrderReferenceListItem[]> GetAvailableMassIncidentReferencesAsync(
            InframanagerObjectListViewFilter filterBy, 
            CancellationToken cancellationToken = default)
        {
            var operation = "work orders available to be referenced by mass incident";
            var flowID = Guid.NewGuid();
            Logger.LogInformation($"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) requesting {operation} (ID = {filterBy.IMObjID})");

            if (!await _userAccess.UserHasOperationAsync(CurrentUser.UserId, OperationID.MassIncident_Update, cancellationToken))
            {
                throw new AccessDeniedException($"View {operation}");
            }
            Logger.LogTrace($"FlowID: {flowID}. Access is granted");

            var query = Repository.Query(
                wo => wo.WorkOrderReference.ObjectClassID != ObjectClass.MassIncident
                    || wo.WorkOrderReference.ObjectID != filterBy.IMObjID);

            return await _referenceListView.GetListItemsPageAsync(query, filterBy, cancellationToken);
        }
    }
}
