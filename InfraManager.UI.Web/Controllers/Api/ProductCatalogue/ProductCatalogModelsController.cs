using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;

[Authorize]
[ApiController]
[Route("api/ProductCatalog/Models")]
public class ProductCatalogModelsController:ControllerBase
{
    private readonly IProductCatalogModelBLLFacade _productCatalogModelBllFacade;

    public ProductCatalogModelsController(IProductCatalogModelBLLFacade productCatalogModelBllFacade)
    {
        _productCatalogModelBllFacade = productCatalogModelBllFacade;
    }

    [HttpGet("{template-id}/{id}")]
    public async Task<ProductModelOutputDetails> GetAsync([FromRoute] Guid id
        , [FromRoute(Name ="template-id")] ProductTemplate templateID
        , CancellationToken cancellationToken = default)
        => await _productCatalogModelBllFacade.GetAsync(id, templateID, cancellationToken);


    [HttpGet]
    public async Task<ProductModelOutputDetails[]> GetListAsync([FromQuery]ProductCatalogModelFilter filter
        , CancellationToken cancellationToken = default)
        => await _productCatalogModelBllFacade.GetModelsAsync(filter, cancellationToken);

    [HttpPost]
    public async Task<ProductModelOutputDetails> PostAsync([FromBody] ProductCatalogModelData data
        , CancellationToken cancellationToken = default)
        => await _productCatalogModelBllFacade.InsertAsync(data, cancellationToken);


    [HttpPut("{template-id}/{id}")]
    public async Task<ProductModelOutputDetails> PutAsync([FromRoute] Guid id
        , [FromRoute(Name = "template-id")] ProductTemplate modelClassID
        , [FromBody] ProductCatalogModelData data
        , CancellationToken cancellationToken = default)
        => await _productCatalogModelBllFacade.UpdateAsync(id, modelClassID, data, cancellationToken);


    [HttpDelete("{template-id}/{id}")]
    public async Task DeleteAsync([FromRoute] Guid id
        , [FromRoute(Name = "template-id")] ProductTemplate modelClassID
        , [FromBody] ProductCatalogDeleteFlags flags
        , CancellationToken cancellationToken = default)
        => await _productCatalogModelBllFacade.DeleteAsync(modelClassID, id, flags, cancellationToken);

    [HttpGet("withoutTTZ")]
    public async Task<ProductModelOutputDetails[]> GetListWithoutTTZAsync([FromQuery] ProductCatalogModelFilter filter
        , CancellationToken cancellationToken = default)
        => await _productCatalogModelBllFacade.GetModelsWithoutTTZAsync(filter, cancellationToken);

    [HttpGet("withoutTTZ/{template-id}/{id}")]
    public async Task<ProductModelOutputDetails> GetWithoutTTZAsync([FromRoute] Guid id
        , [FromRoute(Name = "template-id")] ProductTemplate templateID
        , CancellationToken cancellationToken = default)
        => await _productCatalogModelBllFacade.GetWithoutTTZAsync(id, templateID, cancellationToken);

    [HttpPut("withoutTTZ/{template-id}/{id}")]
    public async Task<ProductModelOutputDetails> PutWithoutTTZAsync([FromRoute] Guid id
        , [FromRoute(Name = "template-id")] ProductTemplate modelClassID
        , [FromBody] ProductCatalogModelData data
        , CancellationToken cancellationToken = default)
        => await _productCatalogModelBllFacade.UpdateWithoutTTZAsync(id, modelClassID, data, cancellationToken);
}