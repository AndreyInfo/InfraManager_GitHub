using InfraManager.DAL.OrganizationStructure;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Import
{
    internal class SubdivisionsAddRangeQuery : ISubdivisionsAddRangeQuery, ISelfRegisteredService<ISubdivisionsAddRangeQuery>
    {
        private readonly CrossPlatformDbContext _dbContext;

        public SubdivisionsAddRangeQuery(CrossPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(Subdivision[] subdivisions, CancellationToken cancellationToken)
        {
            var subdivisionsDbSet = _dbContext.Set<Subdivision>();
            await subdivisionsDbSet.AddRangeAsync(subdivisions, cancellationToken);
        }
    }
}
