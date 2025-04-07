using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Location;
public class LocationFullPathGetter<TEntity> : ILocationFullPathGetter where TEntity : class, ILocationObject, IGloballyIdentifiedEntity
{
    private readonly IReadonlyRepository<TEntity> _repository;

    public LocationFullPathGetter(IReadonlyRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<string> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _repository.FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken);
        return GetLocationFullPathOrDefault(entity);
    }

    private string GetLocationFullPathOrDefault(TEntity entity) => entity == null ? null : entity.GetFullPath() ?? string.Empty;
}
