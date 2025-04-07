using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Asset.History;
public class ObjectHistoryNameGetter<TEntity> : IObjectHistoryNameGetter where TEntity : class, IHistoryNamedEntity, IGloballyIdentifiedEntity
{
    private readonly IReadonlyRepository<TEntity> _repository;

    public ObjectHistoryNameGetter(IReadonlyRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<string> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _repository.FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken);
        return GetObjectNameOrDefault(entity);
    }

    private string GetObjectNameOrDefault(TEntity entity) => entity == null ? null : entity.GetObjectName() ?? string.Empty;
}
