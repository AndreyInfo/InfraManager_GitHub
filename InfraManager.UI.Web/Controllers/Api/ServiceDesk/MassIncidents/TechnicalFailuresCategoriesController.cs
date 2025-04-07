using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TechnicalFailureCategoriesController : ControllerBase
{ 
    private readonly ITechnicalFailureCategoryBLL _service;

    public TechnicalFailureCategoriesController(ITechnicalFailureCategoryBLL service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<TechnicalFailureCategoryDetails[]> GetAsync(
        [FromQuery] TechnicalFailureCategoryFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _service.GetDetailsArrayAsync(filter, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<TechnicalFailureCategoryDetails> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _service.DetailsAsync(id, cancellationToken);
    }

    [HttpPost]
    public async Task<TechnicalFailureCategoryDetails> PostAsync(
        [FromBody] TechnicalFailureCategoryData data,
        CancellationToken cancellationToken = default)
    {
        return await _service.AddAsync(data, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<TechnicalFailureCategoryDetails> PutAsync(
        int id,
        TechnicalFailureCategoryData data, 
        CancellationToken cancellationToken = default)
    {
        return await _service.UpdateAsync(id, data, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteAsync(id, cancellationToken);
    }

    [HttpPost("{id}/services")]
    public async Task<ServiceReferenceDetails> PostServiceReferenceAsync(
        int id,
        [FromBody] ServiceReferenceData data,
        CancellationToken cancellationToken = default)
        => await _service.AddServiceReferenceAsync(id, data, cancellationToken);

    [HttpPut("{id}/services/{serviceID}")]
    public async Task<ServiceReferenceDetails> PutServiceReferenceAsync(
        int id,
        Guid serviceID,
        [FromBody] ServiceReferenceUpdatableData data,
        CancellationToken cancellationToken = default)
        => await _service.UpdateServiceReferenceAsync(id, serviceID, data, cancellationToken);

    [HttpDelete("{id}/services/{serviceID}")]
    public async Task DeleteServiceReferenceAsync(int id, Guid serviceID, CancellationToken cancellationToken = default) =>
        await _service.DeleteServiceReferenceAsync(id, serviceID, cancellationToken);
}