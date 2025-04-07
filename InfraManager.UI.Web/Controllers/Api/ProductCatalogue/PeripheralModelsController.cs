using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PeripheralModelsController
{
    private readonly IPeripheralModelBLL _service;

    public PeripheralModelsController(IPeripheralModelBLL service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<ProductModelDetails[]> ListAsync([FromQuery] ProductModelListFilter filterBy, CancellationToken cancellationToken = default)
    {
        return _service.GetDetailsArrayAsync(filterBy, cancellationToken);
    }
    
    [HttpGet("{id:guid}")]
    public Task<ProductModelDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _service.DetailsAsync(id, cancellationToken);
    }
}