using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.DAL.Settings;

namespace InfraManager.DAL.ServiceCatalog
{
    internal class PortfolioServiceGetInfrastructureQuery : IPortfolioServiceGetInfrastructureQuery, ISelfRegisteredService<IPortfolioServiceGetInfrastructureQuery>
    {
        private readonly DbContext _dbSet;

        public PortfolioServiceGetInfrastructureQuery(CrossPlatformDbContext dbSet)
        {
            _dbSet = dbSet;
        }

        public async Task<PortfolioServiceInfrastructureItem[]> ExecuteAsync(Guid serviceID, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.Set<ServiceReference>().AsQueryable();

            return await query.Where(x => x.ServiceID == serviceID).AsNoTracking().Select(x => new PortfolioServiceInfrastructureItem()
            {
                ID = x.ID,
                ServiceID = x.ServiceID,
                ClassID = x.ClassID,
                ObjectID = x.ObjectID,
                Name = DbFunctions.GetFullObjectName(x.ClassID, x.ObjectID),
                Location = DbFunctions.GetFullObjectLocation(x.ClassID, x.ObjectID),
                Category = _dbSet.Set<InframanagerObjectClass>().FirstOrDefault(z => z.ID == x.ClassID).Name //TODO пересмотреть как будет удален InframanagerObjectClass
            }).ToArrayAsync(cancellationToken);
        }
    }
}
