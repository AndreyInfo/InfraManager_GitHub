using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public class ConcordanceBLL : IConcordanceBLL, ISelfRegisteredService<IConcordanceBLL>
    {
        private readonly IRepository<Concordance> _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ConcordanceBLL(IRepository<Concordance> repository, IMapper mapper, IMemoryCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ConcordanceModel[]> ListAsync(CancellationToken cancellationToken)
        {
            var concordances = await GetEntitiesAsync(cancellationToken);
            return concordances.Select(x => _mapper.Map<ConcordanceModel>(x)).ToArray();
        }

        private Task<Concordance[]> GetEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return _cache.GetOrCreateAsync(
                "Lookup_Concordance", 
                entry => _repository.With(x => x.Priority).ToArrayAsync(cancellationToken));
        }
    }
}
