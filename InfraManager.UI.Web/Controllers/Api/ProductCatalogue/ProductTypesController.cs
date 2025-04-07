using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue;
using InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductTypesController
{
    private readonly IProductCatalogTypeBLL _service;

    public ProductTypesController(IProductCatalogTypeBLL service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ProductCatalogTypeDetails[]> GetListAsync([FromQuery] ProductCatalogTypeFilter filterBy
        , CancellationToken cancellationToken = default)
        => await _service.GetDetailsArrayAsync(filterBy, cancellationToken);

    [HttpGet("{id:guid}")]
    public async Task<ProductCatalogTypeDetails> GetAsync([FromRoute] Guid id
        , CancellationToken cancellationToken = default)
        => await _service.DetailsAsync(id, cancellationToken);

    [HttpPost]
    public async Task<ProductCatalogTypeDetails> PostAsync([FromBody] ProductCatalogTypeData data 
        ,CancellationToken cancellationToken = default)
        => await _service.AddAsync(data, cancellationToken);

    [HttpPut("{id:guid}")]
    public async Task<ProductCatalogTypeDetails> PutAsync([FromRoute] Guid id
        , [FromBody] ProductCatalogTypeData data
        , CancellationToken cancellationToken = default)
        => await _service.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id:guid}")]
    public async Task DeleteAsync([FromRoute] Guid id
        , ProductCatalogDeleteFlags flags
        , CancellationToken cancellationToken = default)
        => await _service.DeleteAsync(id, flags, cancellationToken);


    [HttpGet("withoutTTZ/{id:guid}")]
    public async Task<ProductCatalogTypeDetails> GetDetailsWithoutTTZAsync([FromRoute] Guid id
        , CancellationToken cancellationToken = default)
        => await _service.DetailsWithoutTTZAsync(id, cancellationToken);

    [HttpPut("withoutTTZ/{id:guid}")]
    public async Task<ProductCatalogTypeDetails> PutWithoutTTZAsync([FromRoute] Guid id
        , [FromBody] ProductCatalogTypeData data
        , CancellationToken cancellationToken = default)
        => await _service.UpdateWithoutTTZAsync(id, data, cancellationToken);

    [HttpDelete("withoutTTZ/{id:guid}")]
    public async Task DeleteWithoutTTZAsync([FromRoute] Guid id
        , ProductCatalogDeleteFlags flags
        , CancellationToken cancellationToken = default)
        => await _service.DeleteWithoutByFlagsTTZAsync(id, flags, cancellationToken);
}