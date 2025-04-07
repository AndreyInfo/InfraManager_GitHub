using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.Adapters;
internal sealed class AdapterBuilder : IBuildObject<Adapter, AdapterData>
    , ISelfRegisteredService<IBuildObject<Adapter, AdapterData>>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<AdapterType> _adapterTypeRepository;
    private readonly IServiceMapper<ProductTemplate, IEntityCharacteristicsProvider> _serviceMapper;

    public AdapterBuilder(IMapper mapper
        , IReadonlyRepository<AdapterType> adapterTypeRepository
        , IServiceMapper<ProductTemplate, IEntityCharacteristicsProvider> serviceMapper)
    {
        _mapper = mapper;
        _serviceMapper = serviceMapper;
        _adapterTypeRepository = adapterTypeRepository;
    }

    public async Task<Adapter> BuildAsync(AdapterData data, CancellationToken cancellationToken = default)
    {
        var adapter = _mapper.Map<Adapter>(data);

        var adapterType = await _adapterTypeRepository
            .With(x => x.ProductCatalogType)
            .FirstOrDefaultAsync(x => x.IMObjID == data.AdapterTypeID, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(data.AdapterTypeID.Value, ObjectClass.AdapterModel);

        var modelCharacteristics = await _serviceMapper.Map(adapterType.ProductCatalogType.ProductCatalogTemplateID)
            .GetAsync(adapter.AdapterTypeID.Value, cancellationToken);

        var modelCharacteristicsData = _mapper.Map<EntityCharacteristicsDataBase>(modelCharacteristics);
        modelCharacteristicsData.ID = adapter.IMObjID;

        await _serviceMapper.Map(adapterType.ProductCatalogType.ProductCatalogTemplateID)
            .InsertAsync(modelCharacteristicsData, cancellationToken);

        return adapter;
    }

    public async Task<IEnumerable<Adapter>> BuildManyAsync(IEnumerable<AdapterData> dataItems, CancellationToken cancellationToken = default)
    {
        var adapters = new List<Adapter>();

        foreach (var item in dataItems)
        {
            adapters.Add(await BuildAsync(item, cancellationToken));
        }

        return adapters.ToArray();
    }
}
