using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.ProductClass;
using InfraManager.DAL.ProductCatalogue;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

internal sealed class ProductCatalogBuilder: IBuildObject<ProductCatalogTypeDetails, ProductCatalogType>,
    ISelfRegisteredService<IBuildObject<ProductCatalogTypeDetails, ProductCatalogType>>
{
    private readonly IMapper _mapper;
    private readonly IProductClassBLL _productClassBLL;

    public ProductCatalogBuilder(IMapper mapper
        , IProductClassBLL productClassBLL)
    {
        _mapper = mapper;
        _productClassBLL = productClassBLL;
    }

    public Task<ProductCatalogTypeDetails> BuildAsync(ProductCatalogType data, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ProductCatalogTypeDetails>(data);
        entity.ModelClassID = _productClassBLL.GetModelClassByProductClass(entity.ClassID);
        return Task.FromResult(entity);
    }

    public async Task<IEnumerable<ProductCatalogTypeDetails>> BuildManyAsync(IEnumerable<ProductCatalogType> dataItems, CancellationToken cancellationToken = default)
    {
        var result = new List<ProductCatalogTypeDetails>(dataItems.Count());
        foreach (var item in dataItems) 
        {
            result.Add(await BuildAsync(item, cancellationToken));
        }

        return result;
    }
}