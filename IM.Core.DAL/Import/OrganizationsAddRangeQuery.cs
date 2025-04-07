using System.Collections.Generic;
using InfraManager.DAL.OrganizationStructure;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Import
{
    internal class OrganizationsAddRangeQuery : IOrganizationsAddRangeQuery, ISelfRegisteredService<IOrganizationsAddRangeQuery>
    {
        private readonly CrossPlatformDbContext _dbContext;

        public OrganizationsAddRangeQuery(CrossPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(IEnumerable<Organization> organizations, CancellationToken cancellationToken)
        {
            var organizationsDbSet = _dbContext.Set<Organization>();
            await organizationsDbSet.AddRangeAsync(organizations, cancellationToken);
        }
    }
}
