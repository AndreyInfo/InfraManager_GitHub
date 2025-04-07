using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Slots;
internal sealed class SlotBaseFinder<TEntity> : IFinder<TEntity>
    where TEntity : SlotBase
{
    private readonly IReadonlyRepository<TEntity> _repository;

    public SlotBaseFinder(IReadonlyRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public TEntity Find(params object[] keys)
    {
        var key = keys[0] as SlotBaseKey;
        if (keys.Length == 1 && keys[0] is SlotBaseKey)
            return _repository.SingleOrDefault(x => x.ObjectID == key.ObjectID && x.Number == key.Number);

        throw new ObjectNotFoundException<SlotBaseKey>(key, ObjectClass.Slot);
    }

    public async ValueTask<TEntity> FindAsync(object[] keys, CancellationToken token = default)
    {
        var key = keys[0] as SlotBaseKey;
        if (keys.Length == 1 && keys[0] is SlotBaseKey)
            return await _repository.SingleOrDefaultAsync(x => x.ObjectID == key.ObjectID && x.Number == key.Number, token);

        throw new ObjectNotFoundException<SlotBaseKey>(key, ObjectClass.Slot);
    }

    public IFinder<TEntity> With<TProperty>(System.Linq.Expressions.Expression<Func<TEntity, TProperty>> include)
    {
        throw new NotImplementedException();
    }

    public IFinder<TEntity> WithMany<TProperty>(System.Linq.Expressions.Expression<Func<TEntity, IEnumerable<TProperty>>> include)
    {
        throw new NotImplementedException();
    }
}
