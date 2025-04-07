using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Problems;

internal class ProblemCauseBLL : IProblemCauseBLL, ISelfRegisteredService<IProblemCauseBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<ProblemCause> _repositoryProblemCauses;

    public ProblemCauseBLL(IRepository<ProblemCause> repositoryProblemCauses, IMapper mapper)
    {
        _repositoryProblemCauses = repositoryProblemCauses;
        _mapper = mapper;
    }

    public async Task<ProblemCauseDetails[]> GetListAsync(string search, CancellationToken cancellationToken)
    {
        ProblemCause[] problemCauses;
        if (string.IsNullOrEmpty(search))
            problemCauses = await _repositoryProblemCauses.ToArrayAsync(cancellationToken);
        else
            problemCauses = await _repositoryProblemCauses.ToArrayAsync(c=> c.Name.Contains(search), cancellationToken);

        return _mapper.Map<ProblemCauseDetails[]>(problemCauses);
    }
}
