using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.AdapterTypes;
internal sealed class AdapterTypeBuilder : IBuildObject<AdapterType, ProductCatalogModelData>
    , ISelfRegisteredService<IBuildObject<AdapterType, ProductCatalogModelData>>
{

    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<ProductCatalogType> _productCatalogTypeRepository;
    private readonly IServiceMapper<ProductTemplate, IEntityCharacteristicsProvider> _serviceMapper;

    public AdapterTypeBuilder(IMapper mapper
        , IReadonlyRepository<ProductCatalogType> productCatalogTypeRepository
        , IServiceMapper<ProductTemplate, IEntityCharacteristicsProvider> serviceMapper)
    {
        _mapper = mapper;
        _serviceMapper = serviceMapper;
        _productCatalogTypeRepository = productCatalogTypeRepository;
    }

    public async Task<AdapterType> BuildAsync(ProductCatalogModelData data, CancellationToken cancellationToken = default)
    {
        var adapterType = _mapper.Map<AdapterType>(data);

        var templateID = (await _productCatalogTypeRepository
            .FirstOrDefaultAsync(x => x.IMObjID == data.ProductCatalogTypeID, cancellationToken)).ProductCatalogTemplateID;

        if (templateID is not ProductTemplate.Adapter)
            await _serviceMapper.Map(templateID).InsertAsync(new EntityCharacteristicsDataBase() { ID = adapterType.IMObjID }, cancellationToken);

        return adapterType;
    }

    public async Task<IEnumerable<AdapterType>> BuildManyAsync(IEnumerable<ProductCatalogModelData> dataItems, CancellationToken cancellationToken = default)
    {
        var adapterTypes = new List<AdapterType>();

        foreach (var item in dataItems)
        {
            adapterTypes.Add(await BuildAsync(item, cancellationToken));
        }

        return adapterTypes.ToArray();
    }
}
