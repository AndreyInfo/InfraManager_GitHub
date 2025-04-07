using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.ResourcesArea;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Slots;
internal sealed class SlotBaseBuilder<TEntity, TData> : IBuildObject<TEntity, TData>
    where TEntity : SlotBase
    where TData : SlotBaseData
{
    private readonly IMapper _mapper;
    private readonly ILocalizeText _localizeText;
    private readonly IReadonlyRepository<TEntity> _repository;

    public SlotBaseBuilder(IMapper mapper
        , ILocalizeText localizeText
        , IReadonlyRepository<TEntity> repository)
    {
        _mapper = mapper;
        _repository = repository;
        _localizeText = localizeText;
    }

    public async Task<TEntity> BuildAsync(TData data, CancellationToken cancellationToken = default)
    {
        // TODO: Перенести обработку ошибки на слой DAL.
        if (_repository.Any(x => x.ObjectID == data.ObjectID && x.Number == data.Number))
            throw new InvalidObjectException(string.Format(_localizeText.Localize(nameof(Resources.ValidationErrorType_AlreadyExist)), data.ObjectID, data.Number));

        return _mapper.Map<TEntity>(data);
    }

    public async Task<IEnumerable<TEntity>> BuildManyAsync(IEnumerable<TData> dataItems, CancellationToken cancellationToken = default)
    {
        var entities = new List<TEntity>();

        foreach (var item in dataItems)
            entities.Add(await BuildAsync(item, cancellationToken));

        return entities;
    }
}
