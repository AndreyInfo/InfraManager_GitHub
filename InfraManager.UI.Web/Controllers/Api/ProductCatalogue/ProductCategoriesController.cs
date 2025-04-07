using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue;
using InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductCategoriesController
{
    private readonly IProductCatalogCategoryBLL _service;

    public ProductCategoriesController(IProductCatalogCategoryBLL service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ProductCatalogCategoryDetails[]> ListAsync([FromQuery] ProductCatalogCategoryFilter filterBy
        , CancellationToken cancellationToken = default)
        => await _service.GetDetailsArrayAsync(filterBy, cancellationToken);

    [HttpGet("{id:guid}")]
    public async Task<ProductCatalogCategoryDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default) 
        => await _service.DetailsAsync(id, cancellationToken);

    [HttpPost]
    public async Task<ProductCatalogCategoryDetails> PostAsync([FromBody] ProductCatalogCategoryData data
        , CancellationToken cancellationToken = default) 
        => await _service.AddAsync(data, cancellationToken);

    [HttpPut("{id:guid}")]
    public async Task<ProductCatalogCategoryDetails> PutAsync([FromRoute] Guid id
        , [FromBody] ProductCatalogCategoryData data 
        , CancellationToken cancellationToken = default)
        => await _service.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id:guid}")]
    public async Task DeleteAsync([FromRoute] Guid id
        , ProductCatalogDeleteFlags flags
        , CancellationToken cancellationToken = default)
        => await _service.DeleteAsync(id, flags, cancellationToken);
}