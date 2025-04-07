using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.ResourcesArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;


internal sealed class ProductCatalogCategoryBuilder
    : IBuildObject<ProductCatalogCategory, ProductCatalogCategoryData>
    , ISelfRegisteredService<IBuildObject<ProductCatalogCategory, ProductCatalogCategoryData>>
{
    private readonly IMapper _mapper;
    private readonly ILocalizeText _localizeText;
    private readonly IReadonlyRepository<ProductCatalogCategory> _repository;

    public ProductCatalogCategoryBuilder(
          IMapper mapper
        , ILocalizeText localizeText
        , IReadonlyRepository<ProductCatalogCategory> repository
        )
    {
        _mapper = mapper;
        _repository = repository;
        _localizeText = localizeText;
    }

    public async Task<ProductCatalogCategory> BuildAsync(ProductCatalogCategoryData data, CancellationToken cancellationToken = default)
    {
        if (_repository.Any(x => x.Name.ToLower() == data.Name.ToLower()))
            throw new InvalidObjectException(string.Format(_localizeText.Localize(nameof(Resources.ErrorExistsWithSameParametr))));

        return _mapper.Map<ProductCatalogCategory>(data);
    }

    public Task<IEnumerable<ProductCatalogCategory>> BuildManyAsync(IEnumerable<ProductCatalogCategoryData> dataItems, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}