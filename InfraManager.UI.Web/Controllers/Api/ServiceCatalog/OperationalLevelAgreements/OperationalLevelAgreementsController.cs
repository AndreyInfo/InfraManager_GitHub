using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.ServiceCatalogue.SLA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceCatalog.OperationalLevelAgreements;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OperationalLevelAgreementsController : ControllerBase
{
    private readonly IOperationalLevelAgreementBLL _service;

    public OperationalLevelAgreementsController(IOperationalLevelAgreementBLL service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<OperationalLevelAgreementDetails> PostAsync([FromBody] OperationalLevelAgreementData data,
        CancellationToken cancellationToken = default)
    {
        return await _service.AddAsync(data, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<OperationalLevelAgreementDetails> GetAsync([FromRoute] int id,
        CancellationToken cancellationToken = default)
    {
        return await _service.GetAsync(id, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] int id,
        CancellationToken cancellationToken = default)
    {
        await _service.DeleteAsync(id, cancellationToken);
    }

    [HttpPut("{id}")]
    public Task<OperationalLevelAgreementDetails> PutAsync([FromRoute] int id,
        [FromBody] OperationalLevelAgreementData data, CancellationToken cancellationToken = default)
    {
        return _service.UpdateAsync(id, data, cancellationToken);
    }

    [HttpGet]
    public Task<OperationalLevelAgreementDetails[]> GetAsync([FromQuery] BaseFilter filter,
        CancellationToken cancellationToken = default)
    {
        return _service.ListAsync(filter, cancellationToken);
    }

    [HttpGet("{id}/Concluded")]
    public Task<SLAConcludedWithItem[]> ConcludedWithAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return _service.GetConcludedWithAsync(id, cancellationToken);
    }

    [HttpPost("{id}/Services/{serviceID}")]
    public Task PostServicesReferenceAsync([FromRoute] int id, [FromRoute] Guid serviceID,
        CancellationToken cancellationToken = default)
    {
        return _service.AddServiceReferenceAsync(id, serviceID, cancellationToken);
    }

    [HttpDelete("{id}/Services/{serviceID}")]
    public Task DeleteServicesReferenceAsync([FromRoute] int id, [FromRoute] Guid serviceID,
        CancellationToken cancellationToken = default)
    {
        return _service.RemoveServiceReferenceAsync(id, serviceID, cancellationToken);
    }

    [HttpGet("{id}/Services")]
    public Task<OperationalLevelAgreementServiceDetails[]> GetReferencedServicesAsync([FromRoute] int id,
        [FromQuery] BaseFilter filter, CancellationToken cancellationToken = default)
    {
        return _service.GetServiceReferenceAsync(id, filter, cancellationToken);
    }

    [HttpPost("{id}/Clone")]
    public Task CloneAsync([FromRoute] int id, [FromBody] OperationalLevelAgreementCloneData data,
        CancellationToken cancellationToken = default)
    {
        return _service.CloneAsync(id, data, cancellationToken);
    }
}