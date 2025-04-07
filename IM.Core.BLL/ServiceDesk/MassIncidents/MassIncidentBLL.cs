using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using Inframanager.BLL.ListView;
using InfraManager.BLL.WorkFlow;
using InfraManager.DAL;
using InfraManager.DAL.Documents;
using InfraManager.DAL.Events;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.Expressions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentBLL : IMassIncidentBLL, ISelfRegisteredService<IMassIncidentBLL>
    {
        #region .ctor

        private readonly IInsertEntityBLL<MassIncident, NewMassIncidentData> _creator;
        private readonly IBuildObject<MassIncidentDetails, MassIncident> _detailsBuilder;
        private readonly IFinder<MassIncident> _finder;
        private readonly IGetEntityBLL<int, MassIncident, MassIncidentDetails> _loader;
        private readonly IGetEntityArrayBLL<int, MassIncident, MassIncidentDetails, MassIncidentListFilter> _arrayLoader;
        private readonly IRepository<DocumentReference> _documentReferences;
        private readonly IModifyEntityBLL<int, MassIncident, MassIncidentData, MassIncidentDetails> _modifier;
        private readonly IRemoveEntityBLL<int, MassIncident> _remover;
        private readonly IReadonlyRepository<MassIncident> _repository;
        private readonly IValidatePermissions<MassIncident> _permissionsValidator;
        private readonly IValidateObjectPermissions<int, MassIncident> _accessValidator;
        private readonly IFinder<Call> _callFinder;
        private readonly IFinder<Problem> _problemFinder;
        private readonly IFinder<ChangeRequest> _changeRequestFinder;
        private readonly IFinder<Service> _serviceFinder;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger _logger;
        private readonly IListViewBLL<AllMassIncidentsReportItem, ServiceDeskListFilter> _allMassiveIncidentsBLL;
        private readonly IListViewBLL<MassIncidentsToAssociateReportItem, MassIncidentsToAssociateFilter> _massIncidentsToAssociateBLL;
        private readonly IListViewBLL<ProblemMassIncidentsReportItem, ProblemMassIncidentFilter> _problemMassIncidentBLL;
        private readonly ILightListViewBLL<MassIncidentReferencedCallListItem> _referencedCallsListView;
        private readonly ILightListViewBLL<MassIncidentReferencedChangeRequestListItem> _referencedChangeRequestsListView;
        private readonly ILightListViewBLL<MassIncidentReferencedProblemListItem> _referencedProblemsListView;
        private readonly IReadonlyRepository<WorkOrder> _workOrders;
        private readonly ILightListViewBLL<MassIncidentReferencedWorkOrderListItem> _referencedWorkOrdersListView;
        private readonly IRepository<ManyToMany<MassIncident, Call>> _massIncidentCalls;
        private readonly IRepository<ManyToMany<MassIncident, ChangeRequest>> _massIncidentChangeRequests;
        private readonly IRepository<ManyToMany<MassIncident, Problem>> _massIncidentProblems; 
        private readonly IRepository<ManyToMany<MassIncident, Service>> _massIncidentServices;
        private readonly ISendWorkflowEntityEvent<MassIncident> _workflowEventSender;

        public MassIncidentBLL(
            IListViewBLL<AllMassIncidentsReportItem, ServiceDeskListFilter> allMassiveIncidentsBLL,
            IInsertEntityBLL<MassIncident, NewMassIncidentData> creator, 
            IBuildObject<MassIncidentDetails, MassIncident> detailsBuilder,
            IFinder<MassIncident> finder,
            IGetEntityBLL<int, MassIncident, MassIncidentDetails> loader,
            IGetEntityArrayBLL<int, MassIncident, MassIncidentDetails, MassIncidentListFilter> arrayLoader,
            IRepository<DocumentReference> documentReferences, 
            IModifyEntityBLL<int, MassIncident, MassIncidentData, MassIncidentDetails> modifier, 
            IRemoveEntityBLL<int, MassIncident> remover, 
            IReadonlyRepository<MassIncident> repository, 
            IValidatePermissions<MassIncident> permissionsValidator,
            IValidateObjectPermissions<int, MassIncident> accessValidator,
            IFinder<Call> callFinder, 
            IFinder<Problem> problemFinder, 
            IFinder<ChangeRequest> changeRequestFinder,
            IFinder<Service> serviceFinder,
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            ICurrentUser currentUser, 
            ILogger<MassIncidentBLL> logger,
            IListViewBLL<MassIncidentsToAssociateReportItem, MassIncidentsToAssociateFilter> massIncidentsToAssociateBLL,
            IListViewBLL<ProblemMassIncidentsReportItem, ProblemMassIncidentFilter> problemMassIncidentBLL,
            ILightListViewBLL<MassIncidentReferencedCallListItem> referencedCallsListView,
            ILightListViewBLL<MassIncidentReferencedChangeRequestListItem> referencedChangeRequestsListView,
            ILightListViewBLL<MassIncidentReferencedProblemListItem> referencedProblemsListView,
            IReadonlyRepository<WorkOrder> workOrders,
            ILightListViewBLL<MassIncidentReferencedWorkOrderListItem> referencedWorkOrdersListView, 
            IRepository<ManyToMany<MassIncident, Call>> massIncidentCalls,
            IRepository<ManyToMany<MassIncident, ChangeRequest>> massIncidentChangeRequests,
            IRepository<ManyToMany<MassIncident, Problem>> massIncidentProblems,
            IRepository<ManyToMany<MassIncident, Service>> massIncidentServices,
            ISendWorkflowEntityEvent<MassIncident> workflowEventSender)
        {
            _allMassiveIncidentsBLL = allMassiveIncidentsBLL;
            _arrayLoader = arrayLoader;
            _creator = creator;
            _detailsBuilder = detailsBuilder;
            _finder = finder;
            _loader = loader;
            _documentReferences = documentReferences;
            _modifier = modifier;
            _remover = remover;
            _repository = repository;
            _permissionsValidator = permissionsValidator;
            _accessValidator = accessValidator;
            _callFinder = callFinder;
            _problemFinder = problemFinder;
            _changeRequestFinder = changeRequestFinder;
            _serviceFinder = serviceFinder;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _logger = logger;
            _massIncidentsToAssociateBLL = massIncidentsToAssociateBLL;
            _problemMassIncidentBLL = problemMassIncidentBLL;
            _referencedCallsListView = referencedCallsListView;
            _referencedChangeRequestsListView = referencedChangeRequestsListView;
            _referencedProblemsListView = referencedProblemsListView;
            _workOrders = workOrders;
            _referencedWorkOrdersListView = referencedWorkOrdersListView;
            _massIncidentCalls = massIncidentCalls;
            _massIncidentChangeRequests = massIncidentChangeRequests;
            _massIncidentProblems = massIncidentProblems;
            _massIncidentServices = massIncidentServices;
            _workflowEventSender = workflowEventSender;
        }

        #endregion

        #region Стандартный CRUD

        public async Task<MassIncidentDetails[]> GetDetailsArrayAsync(MassIncidentListFilter filterBy, CancellationToken cancellationToken = default)
        {
            return await _arrayLoader.ArrayAsync(filterBy, cancellationToken);
        }

        public Task<MassIncidentDetails> DetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) is requesting massive incident (ID = {id}) data.");
            return _loader.DetailsAsync(id, cancellationToken);
        }

        public async Task<MassIncidentDetails> AddAsync(NewMassIncidentData data, CancellationToken cancellationToken = default)
        {
            var massiveIncident = await _creator.CreateAsync(data, cancellationToken);

            using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
            {
                await _unitOfWork.SaveAsync(cancellationToken);

                _logger.LogInformation($"User (ID = {_currentUser.UserId}) successfully created new massive incident (ID = {massiveIncident.ID}[{massiveIncident.IMObjID}])");

                foreach (var documentID in data.DocumentIds ?? Array.Empty<Guid>())
                {
                    _documentReferences.Insert(new DocumentReference(documentID, massiveIncident.IMObjID, ObjectClass.MassIncident));
                    _logger.LogTrace(
                        $"User (ID = {_currentUser.UserId}) attached document (ID = {documentID}) to massive incident (ID = {massiveIncident.ID} [{massiveIncident.IMObjID}])");
                }

                await _unitOfWork.SaveAsync(cancellationToken);

                _workflowEventSender.Send(massiveIncident, EventType.EntityCreated);
                await _unitOfWork.SaveAsync(cancellationToken); 

                transaction.Complete();
            }

            return await _detailsBuilder.BuildAsync(massiveIncident, cancellationToken);
        }

        public async Task<MassIncidentDetails> UpdateAsync(int id, MassIncidentData data, CancellationToken cancellationToken = default)
        {
            var massiveIncident = await _modifier.ModifyAsync(id, data, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) successfully modified massive incident (ID = {massiveIncident.ID} [{massiveIncident.IMObjID}]).");

            return await _detailsBuilder.BuildAsync(massiveIncident, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var massIncident = await _finder.FindAsync(id, cancellationToken);
            massIncident.Problems.Select(p => p.Reference)
                .ForEach(SetProblemModified);
            await _remover.RemoveAsync(id, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) sucessfully deleted massive incident (ID = {id}).");
        }

        #endregion

        #region Дочерние сущности

        private async Task ValidatePermissionAsync(string operation, CancellationToken cancellationToken = default)
        {
            if (!await _permissionsValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.Update, cancellationToken))
            {
                throw new AccessDeniedException(operation);
            }

            _logger.LogTrace($"User (ID = {_currentUser.UserId}) permissions to {operation} is granted.");
        }

        private async Task<MassIncident> GetMassIncidentAsync<T>(
            int id,
            Expression<Func<MassIncident, ICollection<ManyToMany<MassIncident, T>>>> include,
            CancellationToken cancellationToken) where T : class
        {
            var includableExpression = new ExpressionResultConverter<IEnumerable<ManyToMany<MassIncident, T>>>()
                .Convert(include);

            var massiveIncident = await _repository
                .WithMany(includableExpression)
                .ThenWith(x => x.Reference)
                .FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<long>(id, "Массовый инцидент");

            _logger.LogTrace($"Massive incident (ID = {id}) is loaded.");

            return massiveIncident;
        }

        private async Task ModifyMassiveIncidentAsync(MassIncident massiveIncident, CancellationToken cancellationToken = default)
        {
            massiveIncident.UtcDateModified = DateTime.UtcNow;
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogTrace($"Changes to massive incident (ID = {massiveIncident.ID} [{massiveIncident.IMObjID}]) were saved.");
        }

        private async Task<TDetails> AddReferenceAsync<T, TDetails>(
            int id,
            Guid referenceID,
            Expression<Func<MassIncident, ICollection<ManyToMany<MassIncident, T>>>> collectionExpression,
            Func<Guid, Task<T>> finder,
            CancellationToken cancellationToken = default) where T : class where TDetails : MassIncidentReferenceDetails, new()
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) is adding {typeof(T).Name} (ID = {referenceID}) to massive incident (ID = {id}).");
            await ValidatePermissionAsync($"associate {typeof(T).Name} with massive incident", cancellationToken);
            var massiveIncident = await GetMassIncidentAsync(id, collectionExpression, cancellationToken);

            var referencedEntity = await finder(referenceID) ?? throw new InvalidObjectException($"{typeof(T).Name} is either removed or not found.");
            _logger.LogTrace($"{typeof(T).Name} (ID = {referenceID}) is found.");

            var collection = collectionExpression.Compile()(massiveIncident);
            var reference = new ManyToMany<MassIncident, T>(referencedEntity);
            collection.Add(reference);

            await ModifyMassiveIncidentAsync(massiveIncident, cancellationToken);
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) successfully added {typeof(T).Name} (ID = {referenceID}) to massive incident (ID = {id} [{massiveIncident.IMObjID}])");

            var details = new TDetails
            {
                MassIncidentID = massiveIncident.IMObjID
            };
            _mapper.Map(reference, details);

            return details;
        }

        private async Task<MassIncidentReferenceDetails> AddReferenceAsync<T>(
            int id,
            Guid referenceID,
            Expression<Func<MassIncident, ICollection<ManyToMany<MassIncident, T>>>> collectionExpression,
            Func<Guid, Task<T>> finder,
            CancellationToken cancellationToken = default) where T : class
        {
            return await AddReferenceAsync<T, MassIncidentReferenceDetails>(id, referenceID, collectionExpression, finder, cancellationToken);
        }

        //TODO: Исправить копипасту
        public async Task<MassIncidentReferenceDetails[]> GetCallsAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) requesting cals related to of mass incident (ID = {id})");
            await _permissionsValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);
            await _accessValidator.ValidateObjectAccessOrRaiseErrorAsync(id, _currentUser.UserId, _logger, cancellationToken);
            var massIncident = await _repository
                .WithMany(x => x.Calls)
                .SingleOrDefaultAsync(x => x.ID == id, cancellationToken)
                    ?? throw new ObjectNotFoundException<int>(id, ObjectClass.MassIncident);
            _logger.LogTrace($"Mass incident (ID = {id}) found.");

            return massIncident.Calls
                .Select(
                    x =>
                    {
                        var details = new MassIncidentReferenceDetails
                        {
                            MassIncidentID = massIncident.IMObjID
                        };
                        _mapper.Map(x, details);
                        return details;
                    })
                .ToArray();
        }

        public Task<MassIncidentReferenceDetails> AddCallAsync(int id, Guid callID, CancellationToken cancellationToken = default)
        {
            return AddReferenceAsync(id, callID, x => x.Calls, async callID => await _callFinder.FindAsync(callID, cancellationToken), cancellationToken);
        }

        private async Task RemoveReferenceAsync<T>(
            int id, 
            Guid referenceID,
            Expression<Func<MassIncident, ICollection<ManyToMany<MassIncident, T>>>> collectionExpression,
            Func<T, Guid> referenceIDAccessor,
            IRepository<ManyToMany<MassIncident, T>> repository,
            CancellationToken cancellationToken = default) where T : class
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) is removing {typeof(T).Name} (ID = {referenceID}) from massive incident (ID = {id}).");
            await ValidatePermissionAsync($"remove {typeof(T).Name} from massive incident", cancellationToken);
            var massiveIncident = await GetMassIncidentAsync(id, collectionExpression, cancellationToken);

            var collection = collectionExpression.Compile()(massiveIncident);
            var reference = collection.FirstOrDefault(x => referenceIDAccessor(x.Reference) == referenceID) 
                ?? throw new ObjectNotFoundException($"Association of {typeof(T).Name} (ID = {referenceID}) and massive incident (ID = {id} [{massiveIncident.IMObjID}])");

            repository.Delete(reference);
            await ModifyMassiveIncidentAsync(massiveIncident, cancellationToken);
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) successfully removed {typeof(T).Name} (ID = {referenceID}) from massive incident (ID = {id} [{massiveIncident.IMObjID}])");
        }

        public Task RemoveCallAsync(int id, Guid callID, CancellationToken cancellationToken = default)
        {
            return RemoveReferenceAsync(id, callID, x => x.Calls, call => call.IMObjID, _massIncidentCalls, cancellationToken);
        }

        //TODO: Исправить копипасту
        public async Task<MassIncidentReferenceDetails[]> GetProblemsAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) requesting problems related to of mass incident (ID = {id})");
            await _permissionsValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);
            await _accessValidator.ValidateObjectAccessOrRaiseErrorAsync(id, _currentUser.UserId, _logger, cancellationToken);
            var massIncident = await _repository
                .WithMany(x => x.Problems)
                .SingleOrDefaultAsync(x => x.ID == id, cancellationToken)
                    ?? throw new ObjectNotFoundException<int>(id, ObjectClass.MassIncident);
            _logger.LogTrace($"Mass incident (ID = {id}) found.");

            return massIncident.Problems
                .Select(
                    x =>
                    {
                        var details = new MassIncidentReferenceDetails
                        {
                            MassIncidentID = massIncident.IMObjID
                        };
                        _mapper.Map(x, details);
                        return details;
                    })
                .ToArray();
        }
        public async Task<MassIncidentReferenceDetails> AddProblemAsync(int id, Guid problemID, CancellationToken cancellationToken = default)
        {
            var problem = await _problemFinder.FindAsync(problemID, cancellationToken);
            SetProblemModified(problem);
            return await AddReferenceAsync(id, problemID, x => x.Problems, async problemID => await _problemFinder.FindAsync(problemID, cancellationToken), cancellationToken);
        }

        public async Task RemoveProblemAsync(int id, Guid problemID, CancellationToken cancellationToken = default)
        {
            var problem = await _problemFinder.FindAsync(problemID, cancellationToken);
            SetProblemModified(problem);
            await RemoveReferenceAsync(id, problemID, x => x.Problems, problem => problem.IMObjID, _massIncidentProblems, cancellationToken);
        }
        
        private void SetProblemModified(Problem problem)
        {
            problem.UtcDateModified = DateTime.UtcNow;
        }

        //TODO: Исправить копипасту
        public async Task<MassIncidentReferenceDetails[]> GetChangeRequestAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) requesting rfc related to of mass incident (ID = {id})");
            await _permissionsValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);
            await _accessValidator.ValidateObjectAccessOrRaiseErrorAsync(id, _currentUser.UserId, _logger, cancellationToken);
            var massIncident = await _repository
                .WithMany(x => x.ChangeRequests)
                .SingleOrDefaultAsync(x => x.ID == id, cancellationToken)
                    ?? throw new ObjectNotFoundException<int>(id, ObjectClass.MassIncident);
            _logger.LogTrace($"Mass incident (ID = {id}) found.");

            return massIncident.ChangeRequests
                .Select(
                    x =>
                    {
                        var details = new MassIncidentReferenceDetails
                        {
                            MassIncidentID = massIncident.IMObjID
                        };
                        _mapper.Map(x, details);
                        return details;
                    })
                .ToArray();
        }

        public Task<MassIncidentReferenceDetails> AddChangeRequestAsync(int id, Guid changeRequestID, CancellationToken cancellationToken = default)
        {
            return AddReferenceAsync(
                id,
                changeRequestID,
                x => x.ChangeRequests,
                async changeRequestID => await _changeRequestFinder.FindAsync(changeRequestID, cancellationToken),
                cancellationToken);
        }

        public Task RemoveChangeRequestAsync(int id, Guid changeRequestID, CancellationToken cancellationToken = default)
        {
            return RemoveReferenceAsync(
                id, 
                changeRequestID, 
                x => x.ChangeRequests, 
                changeRequest => changeRequest.IMObjID, 
                _massIncidentChangeRequests, 
                cancellationToken);
        }

        //TODO: Сделать ManyToManyReferenceBLL
        public async Task<MassIncidentReferenceDetails[]> GetAffectedServicesAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) requesting affected services of mass incident (ID = {id})");
            await _permissionsValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);
            await _accessValidator.ValidateObjectAccessOrRaiseErrorAsync(id, _currentUser.UserId, _logger, cancellationToken);
            var massIncident = await _repository
                .WithMany(x => x.AffectedServices)
                .SingleOrDefaultAsync(x => x.ID == id, cancellationToken) 
                    ?? throw new ObjectNotFoundException<int>(id, ObjectClass.MassIncident);
            _logger.LogTrace($"Mass incident (ID = {id}) found.");

            return massIncident.AffectedServices
                .Select(
                    x =>
                    {
                        var details = new MassIncidentReferenceDetails
                        {
                            MassIncidentID = massIncident.IMObjID
                        };
                        _mapper.Map(x, details);
                        return details; 
                    })
                .ToArray();
        }

        public Task<MassIncidentReferenceDetails> AddAffectedServiceAsync(int id, Guid serviceID, CancellationToken cancellationToken = default)
        {
            return AddReferenceAsync(
                id, 
                serviceID, 
                x => x.AffectedServices, 
                async itemOrAttendanceID => await _serviceFinder.FindAsync(serviceID, cancellationToken), 
                cancellationToken);
        }

        public Task RemoveAffectedServiceAsync(int id, Guid serviceID, CancellationToken cancellationToken = default)
        {
            return RemoveReferenceAsync(
                id,
                serviceID,
                x => x.AffectedServices,
                x => x.ID,
                _massIncidentServices,
                cancellationToken);
        }

        #endregion

        #region Отчеты

        public Task<AllMassIncidentsReportItem[]> AllMassIncidentsAsync(
            ListViewFilterData<ServiceDeskListFilter> filterBy, 
            CancellationToken cancellationToken = default)
        {
            return _allMassiveIncidentsBLL.BuildAsync(filterBy, cancellationToken);
        }

        public async Task<MassIncidentReferencedCallListItem[]> GetReferencedCallsReportAsync(
            int id,
            MassIncidentReferencesListViewFilter pageFilter, 
            CancellationToken cancellationToken = default)
        {
            LogReferenceReportInfo(id, "calls");
            await ValidateMassIncidentAccessOrRaiseErrorAsync(id, "calls", cancellationToken);

            return await _referencedCallsListView.GetListItemsPageAsync(
                QueryReferences(id, pageFilter.IDList, x => x.Calls), 
                pageFilter, 
                cancellationToken);
        }

        public async Task<MassIncidentReferencedChangeRequestListItem[]> GetReferencedChangeRequestsReportAsync(
            int id, 
            MassIncidentReferencesListViewFilter pageFilter, 
            CancellationToken cancellationToken = default)
        {
            LogReferenceReportInfo(id, "change requests");
            await ValidateMassIncidentAccessOrRaiseErrorAsync(id, "change requests", cancellationToken);

            return await _referencedChangeRequestsListView.GetListItemsPageAsync(
                QueryReferences(id, pageFilter.IDList, x => x.ChangeRequests),
                pageFilter,
                cancellationToken);
        }

        public async Task<MassIncidentReferencedProblemListItem[]> GetReferencedProblemsReportAsync(
            int id, 
            MassIncidentReferencesListViewFilter pageFilter, 
            CancellationToken cancellationToken = default)
        {
            LogReferenceReportInfo(id, "problems");
            await ValidateMassIncidentAccessOrRaiseErrorAsync(id, "problems", cancellationToken);
            return await _referencedProblemsListView.GetListItemsPageAsync(
                QueryReferences(id, pageFilter.IDList, x => x.Problems), 
                pageFilter, 
                cancellationToken);
        }

        public async Task<MassIncidentReferencedWorkOrderListItem[]> GetReferencedWorkOrdersReportAsync(
            int id, 
            MassIncidentReferencesListViewFilter pageFilter, 
            CancellationToken cancellationToken = default)
        {
            LogReferenceReportInfo(id, "workorders");
            await ValidateMassIncidentAccessOrRaiseErrorAsync(id, "workorders", cancellationToken);

            var massIncident = await _finder.FindAsync(id, cancellationToken)
                ?? throw new ObjectNotFoundException<int>(id, ObjectClass.MassIncident);
            _logger.LogTrace($"Mass Incident (ID = {id}) is found.");

            var referencedByMassIncident = new Specification<WorkOrder>(
                x => x.WorkOrderReference.ObjectClassID == ObjectClass.MassIncident
                            && x.WorkOrderReference.ObjectID == massIncident.IMObjID);
            var containsInIDList = new IDListSpecification<WorkOrder>(pageFilter.IDList);
            return await _referencedWorkOrdersListView.GetListItemsPageAsync(
                _workOrders
                    .Query(referencedByMassIncident && containsInIDList),
                pageFilter,
                cancellationToken);
        }

        private IQueryable<T> QueryReferences<T>(
            int id,
            Guid[] idList,
            Expression<Func<MassIncident, IEnumerable<ManyToMany<MassIncident, T>>>> references)
            where T : class, IGloballyIdentifiedEntity =>
                _repository
                    .Query(x => x.ID == id)
                    .SelectMany(references)
                    .Select(x => x.Reference)
                    .Where(new IDListSpecification<T>(idList));

        private void LogReferenceReportInfo(int id, string referenceName) =>
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) requested mass incident (ID = {id}) referenced {referenceName} report.");

        private async Task ValidateMassIncidentAccessOrRaiseErrorAsync(int id, string referenceName, CancellationToken cancellationToken = default)
        {
            if (!await _accessValidator.ObjectIsAvailableAsync(_currentUser.UserId, id, cancellationToken))
            {
                throw new AccessDeniedException($"View mass incident referenced {referenceName} report of mass incident (ID = {id})");
            }
            _logger.LogTrace($"View mass incident (ID = {id}) referenced {referenceName} report permission is granted.");
        }

        public Task<MassIncidentsToAssociateReportItem[]> GetMassIncidentsToAssociateAsync(
            ListViewFilterData<MassIncidentsToAssociateFilter> filterBy,
            CancellationToken cancellationToken = default)
        {
            return _massIncidentsToAssociateBLL.BuildAsync(filterBy, cancellationToken); // TODO: ListViewBLL тут ни к чему - переписать
        }

        public Task<ProblemMassIncidentsReportItem[]> GetProblemMassIncidentsAsync(
            ListViewFilterData<ProblemMassIncidentFilter> filterBy,
            CancellationToken cancellationToken = default)
        {
            return _problemMassIncidentBLL.BuildAsync(filterBy, cancellationToken); // TODO: ListViewBLL тут ни к чему - переписать
        }

        #endregion
    }
}
