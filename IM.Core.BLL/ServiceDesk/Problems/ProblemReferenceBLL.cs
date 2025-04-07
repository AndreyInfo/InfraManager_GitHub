using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.Problems;

namespace InfraManager.BLL.ServiceDesk.Problems;

internal class ProblemReferenceBLL : IProblemReferenceBLL, ISelfRegisteredService<IProblemReferenceBLL>
{
    private readonly IServiceMapper<ObjectClass, ICallReferenceBLL> _services;
    private readonly IReadonlyRepository<MassIncident> _massIncidents;
    private readonly IListQuery<Problem, ProblemListQueryResultItem> _query;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;
    private readonly IGuidePaggingFacade<ProblemListQueryResultItem, ProblemListItem> _paggingFacade;

    public ProblemReferenceBLL(IServiceMapper<ObjectClass, ICallReferenceBLL> services,
        IReadonlyRepository<MassIncident> massIncidents,
        IListQuery<Problem, ProblemListQueryResultItem> query,
        ICurrentUser currentUser,
        IMapper mapper,
        IGuidePaggingFacade<ProblemListQueryResultItem, ProblemListItem> paggingFacade)
    {
        _services = services;
        _massIncidents = massIncidents;
        _query = query;
        _currentUser = currentUser;
        _mapper = mapper;
        _paggingFacade = paggingFacade;
    }

    public async Task<ProblemListItem[]> GetReferencedProblemsAsync(BaseFilter filter, Guid objectID,
        ObjectClass referenceObjectClass, CancellationToken cancellationToken = default)
    {
        var ids = new List<Guid>();

        if (referenceObjectClass == ObjectClass.Call)
        {
            ids = (await _services.Map(ObjectClass.Problem)
                    .GetAsync(new CallReferenceListFilter { CallID = objectID }, cancellationToken))
                .Select(x => x.ObjectID).ToList();
        }
        else if(referenceObjectClass == ObjectClass.MassIncident)
        {
            var massIncident = await
                _massIncidents.With(x => x.Problems)
                    .FirstOrDefaultAsync(x => x.IMObjID == objectID, cancellationToken) ?? throw
                    new ObjectNotFoundException("Mass incident not found");

            ids = massIncident.Problems.Select(x => x.Reference.IMObjID).ToList();
        }

        var newQuery = _query.Query(_currentUser.UserId, Array.Empty<Expression<Func<Problem, bool>>>())
            .Where(x => ids.Contains(x.ID));

        var result = await _paggingFacade.GetPaggingAsync(filter, newQuery, null, cancellationToken);

        return _mapper.Map<ProblemListItem[]>(result);
    }
}