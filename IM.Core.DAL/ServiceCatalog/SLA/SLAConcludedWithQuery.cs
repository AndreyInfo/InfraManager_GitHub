using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue.SLA;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceCatalog.SLA;

internal class SLAConcludedWithQuery : ISLAConcludedWithQuery, ISelfRegisteredService<ISLAConcludedWithQuery>
{
    private readonly DbContext _dbSet;

    public SLAConcludedWithQuery(CrossPlatformDbContext dbSet)
    {
        _dbSet = dbSet;
    }

    public async Task<SLAConcludedWithItem[]> ExecuteAsync(Guid slaID, CancellationToken cancellationToken = default)
    {
        var slqConcludedWithQuery = _dbSet.Set<OrganizationItemGroup>().AsQueryable()
            .Where(x => x.ItemClassID != ObjectClass.Owner);

        return await slqConcludedWithQuery.Where(x => x.ID == slaID).Select(x =>
            new SLAConcludedWithItem
            {
                ConcludedWith = x.ItemClassID == ObjectClass.User
                    ? DbFunctions.GetFullObjectName(x.ItemClassID, x.ItemID)
                    : DbFunctions.GetFullObjectLocation(x.ItemClassID, x.ItemID) + " \\ " +
                      DbFunctions.GetFullObjectName(x.ItemClassID, x.ItemID),
                ObjectClass = x.ItemClassID,
                ObjectID = x.ItemID
            }).OrderBy(x => x.ConcludedWith).ToArrayAsync(cancellationToken);
    }
}