using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels;
internal sealed class ProductCatalogModelBuilder<TEntity> : IBuildObject<ProductModelOutputDetails, TEntity>
    where TEntity : class, IProductModel
{
    private readonly IMapper _mapper;

    public ProductCatalogModelBuilder(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ProductModelOutputDetails> BuildAsync(TEntity data, CancellationToken cancellationToken = default)
    {
        var details = _mapper.Map<ProductModelOutputDetails>(data);
        details.CategoryName = await GetCategoryPath(data.ProductCatalogType.ProductCatalogCategory);

        return details;
    }

    public async Task<IEnumerable<ProductModelOutputDetails>> BuildManyAsync(IEnumerable<TEntity> dataItems, CancellationToken cancellationToken = default)
    {
        var detailsList = new List<ProductModelOutputDetails>();

        foreach (var item in dataItems)
        {
            detailsList.Add(await BuildAsync(item, cancellationToken));
        }

        return detailsList;
    }

    private async Task<string> GetCategoryPath(ProductCatalogCategory category)
    {
        if (category.ParentProductCatalogCategory is null)
        {
            return category.Name;
        }
        else
        {
            return await GetCategoryPath(category.ParentProductCatalogCategory) + "/ " + category.Name;
        }
    }
}
