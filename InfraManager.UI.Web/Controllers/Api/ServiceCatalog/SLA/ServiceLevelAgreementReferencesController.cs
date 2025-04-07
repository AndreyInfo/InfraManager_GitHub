using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceCatalog.SLA;

[Route("api/SlaReferences")]
[ApiController]
[Authorize]
public class ServiceLevelAgreementReferencesController : ControllerBase
{
    private readonly IServiceLevelAgreementReference _slaReferenceBLL;
    
    public ServiceLevelAgreementReferencesController(IServiceLevelAgreementReference slaReferenceBll)
    {
        _slaReferenceBLL = slaReferenceBll;
    }
    
    [HttpDelete("{slaID}/{objectID}")]
    public async Task DeleteAsync([FromRoute] Guid slaID, [FromRoute] Guid objectID,
        CancellationToken cancellationToken = default)
    {
        await _slaReferenceBLL.DeleteAsync(slaID, objectID, cancellationToken);
    }

    [HttpGet("{slaID}/{objectID}")]
    public async Task<SLAReferenceDetails> GetAsync([FromRoute] Guid slaID, [FromRoute] Guid objectID,
        CancellationToken cancellationToken = default)
    {
        return await _slaReferenceBLL.GetAsync(slaID, objectID, cancellationToken);
    }

    [HttpPut("{slaID}/{objectID}")]
    public async Task UpdateAsync([FromRoute] Guid slaID, [FromRoute] Guid objectID,
        SLAReferenceData data, CancellationToken cancellationToken = default)
    {
        await _slaReferenceBLL.UpdateAsync(slaID, objectID, data, cancellationToken);
    }

    [HttpPost]
    public async Task AddAsync([FromBody] SLAReferenceDetails reference,
        CancellationToken cancellationToken = default)
    {
        await _slaReferenceBLL.InsertAsync(reference, cancellationToken);
    }
}