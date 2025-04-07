using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System.Linq.Expressions;
using System;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class ProblemDetailsBuilder : IBuildObject<ProblemDetails, Problem>,
        ISelfRegisteredService<IBuildObject<ProblemDetails, Problem>>
    {
        private readonly IMapper _mapper;
        private readonly IReadonlyRepository<ObjectNote> _objectNotes;
        private readonly ICurrentUser _currentUser;
        private readonly IUserFieldsToDictionaryResolver _userFieldsToDictionaryResolver;
        private readonly IFinder<Service> _serviceFinder;
        private readonly IFinder<Group> _groupFinder;
        private readonly IReadonlyRepository<MassIncident> _massIncidents;
        private readonly IReadonlyRepository<ManyToMany<MassIncident, Problem>> _massIncidentsWithProblems;
        private readonly IReadonlyRepository<WorkOrder> _workOrders;

        public ProblemDetailsBuilder(
            IMapper mapper, 
            IReadonlyRepository<ObjectNote> objectNotes,
            ICurrentUser currentUser,
            IUserFieldsToDictionaryResolver userFieldsToDictionaryResolver,
            IFinder<Service> serviceFinder,
            IFinder<Group> groupFinder,
            IReadonlyRepository<MassIncident> massIncidents,
            IReadonlyRepository<ManyToMany<MassIncident, Problem>> massIncidentsWithProblems,
            IReadonlyRepository<WorkOrder> workOrders)
        {
            _mapper = mapper;
            _objectNotes = objectNotes;
            _currentUser = currentUser;
            _userFieldsToDictionaryResolver = userFieldsToDictionaryResolver;
            _serviceFinder = serviceFinder;
            _groupFinder = groupFinder;
            _massIncidents = massIncidents;
            _massIncidentsWithProblems = massIncidentsWithProblems;
            _workOrders = workOrders;
        }

        public async Task<IEnumerable<ProblemDetails>> BuildManyAsync(IEnumerable<Problem> dataItems, CancellationToken cancellationToken = default)
        {
            var problemDetailsList = new List<ProblemDetails>(dataItems.Count());
            foreach (var problem in dataItems)
            {
                var problemDetails = await BuildAsync(problem, cancellationToken);
                problemDetailsList.Add(problemDetails);
            }

            return problemDetailsList;
        }

        public async Task<ProblemDetails> BuildAsync(Problem data, CancellationToken cancellationToken = default)
        {
            var details = _mapper.Map<ProblemDetails>(data);
            details.UnreadNoteCount = await _objectNotes
                .CountAsync(x => !x.Read
                    && x.ObjectID== data.IMObjID
                    && x.UserID == _currentUser.UserId);

            details.MassIncidentCount = await _massIncidentsWithProblems
                .CountAsync(x => x.Reference.IMObjID == data.IMObjID);
            
            details.WorkOrderCount = await _workOrders
                .CountAsync(wo => wo.WorkOrderReference.ObjectID == data.IMObjID, cancellationToken);

            details.UserFieldNamesDictionary = _userFieldsToDictionaryResolver.Resolve(details);

            if (data.ServiceID.HasValue)
            {
                var service = await _serviceFinder.With(x => x.Category).FindAsync(data.ServiceID.Value, cancellationToken);
                details.ServiceName = service?.Name;
                details.ServiceCategoryName = service?.Category?.Name;
            }

            if (data.QueueID.HasValue)
            {
                var queue = await _groupFinder.FindAsync(data.QueueID.Value, cancellationToken);
                details.QueueName = queue?.Name;
            }

            return details;
        }
    }
}
