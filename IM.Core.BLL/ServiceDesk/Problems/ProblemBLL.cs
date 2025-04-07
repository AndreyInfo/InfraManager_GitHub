using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.BLL.AccessManagement;
using InfraManager.Linq;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class ProblemBLL :
        StandardBLL<Guid, Problem, ProblemData, ProblemDetails, ProblemListFilter>,
        IProblemBLL,
        ISelfRegisteredService<IProblemBLL>
    {
        #region .ctor

        private readonly IListViewBLL<ProblemListItem, ServiceDeskListFilter> _dataListBuilder;
        private readonly IFindEntityByGlobalIdentifier<Problem> _finder;
        private readonly ILoadEntity<Guid, Problem, ProblemDetails> _loader;
        private readonly IValidatePermissions<Problem> _permissionsValidator;
        private readonly IFinder<ChangeRequest> _changeRequestFinder;
        private readonly IReadonlyRepository<MassIncident> _massIncidents;
        private readonly IUserAccessBLL _userAccess;
        private readonly IValidateObjectPermissions<Guid, Problem> _objectAccessValidator;
        private readonly ILightListViewBLL<ProblemReferenceListItem> _referenceListView;

        public ProblemBLL(
            IRepository<Problem> repository,
            ILogger<ProblemBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<ProblemDetails, Problem> detailsBuilder,
            IInsertEntityBLL<Problem, ProblemData> insertEntityBLL,
            IModifyEntityBLL<Guid, Problem, ProblemData, ProblemDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, Problem> removeEntityBLL,
            IGetEntityBLL<Guid, Problem, ProblemDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, Problem, ProblemDetails, ProblemListFilter> detailsArrayBLL,
            IListViewBLL<ProblemListItem, ServiceDeskListFilter> dataListBuilder,
            IFindEntityByGlobalIdentifier<Problem> finder,
            ILoadEntity<Guid, Problem, ProblemDetails> loader,
            IValidatePermissions<Problem> permissionsValidator,
            IFinder<ChangeRequest> changeRequestFinder,
            IReadonlyRepository<MassIncident> massIncidents,
            IUserAccessBLL userAccess,
            IValidateObjectPermissions<Guid, Problem> objectAccessValidator,
            ILightListViewBLL<ProblemReferenceListItem> referenceListView)
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
            _permissionsValidator = permissionsValidator;
            _changeRequestFinder = changeRequestFinder;
            _massIncidents = massIncidents;
            _userAccess = userAccess;
            _objectAccessValidator = objectAccessValidator;
            _referenceListView = referenceListView;
        }

        #endregion

        #region Список Проблемы

        public Task<ProblemListItem[]> AllProblemsArrayAsync(ListViewFilterData<ServiceDeskListFilter> filterBy, CancellationToken cancellationToken = default)
        {
            return _dataListBuilder.BuildAsync(filterBy, cancellationToken);
        }
        #endregion


        public async Task<ProblemToChangeRequestReferenceDetails> AddChangeRequestAsync(Guid problemID, Guid changeRequestID, CancellationToken cancellationToken = default)
        {
            Logger.LogTrace($"User (ID = {CurrentUser.UserId}) is adding {nameof(ChangeRequest)} (ID = {changeRequestID}) to problem (ID = {problemID}).");

            await ValidatePermissionAsync($"Associate {nameof(ChangeRequest)} with problem", ObjectAction.Update, cancellationToken);

            var problem = await _loader.LoadAsync(problemID, cancellationToken);
            var changeRequest = await _changeRequestFinder.FindAsync(changeRequestID, cancellationToken)
                                ?? throw new InvalidObjectException($"{nameof(ChangeRequest)} is either removed or not found.");

            Logger.LogTrace($"{nameof(ChangeRequest)} (ID = {changeRequestID}) is found.");

            var reference = new ManyToMany<Problem, ChangeRequest>(changeRequest);
            await ModifyProblemAsync(problem, p => p.ChangeRequests.Add(reference), cancellationToken);

            Logger.LogInformation($"User (ID = {CurrentUser.UserId}) successfully added {nameof(ChangeRequest)} (ID = {changeRequestID}) to problem (ID = {problemID})");

            return new ProblemToChangeRequestReferenceDetails
            {
                ID = reference.ID,
                ProblemID = problem.IMObjID,
                ChangeRequestID = reference.Reference.IMObjID,
            };
        }

        public async Task RemoveChangeRequestAsync(Guid problemID, Guid changeRequestID, CancellationToken cancellationToken)
        {
            Logger.LogTrace($"User (ID = {CurrentUser.UserId}) is removing {nameof(ChangeRequest)} (ID = {changeRequestID}) from problem (ID = {problemID}).");

            await ValidatePermissionAsync($"remove {nameof(ChangeRequest)} from problem", ObjectAction.Update, cancellationToken);
            
            var problem = await _loader.LoadAsync(problemID, cancellationToken);
            var reference = problem.ChangeRequests.FirstOrDefault(x => x.Reference.IMObjID == changeRequestID)
                            ?? throw new ObjectNotFoundException($"Association of {nameof(ChangeRequest)} (ID = {changeRequestID}) and problem (ID = {problemID})");

            await ModifyProblemAsync(problem, p => p.ChangeRequests.Remove(reference), cancellationToken);
            
            Logger.LogInformation($"User (ID = {CurrentUser.UserId}) successfully removed {nameof(ChangeRequest)} (ID = {changeRequestID}) from problem (ID = {problemID})");
        }

        private async Task ModifyProblemAsync(Problem problem, Action<Problem> modifyAction, CancellationToken cancellationToken = default)
        {
            modifyAction?.Invoke(problem);
            problem.UtcDateModified = DateTime.UtcNow;
            await UnitOfWork.SaveAsync(cancellationToken);
            Logger.LogTrace($"Changes to problem (ID = {problem.IMObjID}) were saved.");
        }

        private async Task ValidatePermissionAsync(string operation, ObjectAction action, CancellationToken cancellationToken = default)
        {
            if (!await _permissionsValidator.UserHasPermissionAsync(CurrentUser.UserId, action, cancellationToken))
            {
                throw new AccessDeniedException(operation);
            }
            Logger.LogTrace($"User (ID = {CurrentUser.UserId}) permissions to {operation} is granted.");
        }

        //TODO: Это копипаста из CallBll
        public async Task<ProblemReferenceListItem[]> GetAvailableMassIncidentReferencesAsync(InframanagerObjectListViewFilter filterBy, CancellationToken cancellationToken = default)
        {
            var operation = "problems available to be referenced by mass incident";
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
                        .SelectMany(x => x.Problems)
                        .Select(x => x.Reference));

            return await _referenceListView.GetListItemsPageAsync(query, filterBy, cancellationToken);
        }
    }
}
