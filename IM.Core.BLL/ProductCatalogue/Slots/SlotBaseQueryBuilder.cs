using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.Slots;

internal sealed class SlotBaseQueryBuilder<TEntity, TDetails> : IBuildEntityQuery<TEntity, TDetails, SlotBaseFilter>
    where TEntity : SlotBase
{
    private readonly IReadonlyRepository<TEntity> _repository;

    public SlotBaseQueryBuilder(IReadonlyRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<TEntity> Query(SlotBaseFilter filterBy)
    {
        var query = _repository.With(x => x.SlotType)
            .Query(x => x.ObjectID == filterBy.ObjectID);

        if (!string.IsNullOrEmpty(filterBy.SearchString))
            query.Where(x => x.Number.ToString().Contains(filterBy.SearchString));

        return query;
    }
}