using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Location;

internal abstract class LocationNodeQuery<TEntity> : ILocationNodesQuery
    where TEntity : class 
{
    protected readonly DbSet<ClassIcon> _classIcons;
    protected readonly DbSet<TEntity> _entities;
    protected abstract ObjectClass ClassID { get; }

    public LocationNodeQuery(DbSet<ClassIcon> classIcons,
                        DbSet<TEntity> entities)
    {
        _classIcons= classIcons;
        _entities = entities;
    }

    public abstract Task<LocationTreeNode[]> GetNodesAsync(int parentID
        , ObjectClass? childClassID = null
        , CancellationToken cancellationToken = default);
}





