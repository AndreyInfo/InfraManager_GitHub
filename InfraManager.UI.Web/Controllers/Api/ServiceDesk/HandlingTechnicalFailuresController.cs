using InfraManager.BLL.ServiceDesk.HandlingTechnicalFailures;
using InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;
using InfraManager.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk;

[Obsolete("Use api/technicalFailureCategories")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HandlingTechnicalFailuresController : ControllerBase
{
    private readonly ITechnicalFailureCategoryBLL _service;

    public HandlingTechnicalFailuresController(ITechnicalFailureCategoryBLL service)
    {
        _service = service;
    }

    [HttpGet("{id::guid}")]
    public async Task<HandlingTechnicalFailureDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var technicalFailureCategory = (await _service.GetDetailsArrayAsync(new TechnicalFailureCategoryFilter { ServiceReferenceID = id }, cancellationToken))
            .FirstOrDefault();
        var reference = technicalFailureCategory.ServiceReferences.Single(x => x.IMObjID == id);

        return ToDetails(technicalFailureCategory, reference);
    }

    [HttpGet]
    public async Task<HandlingTechnicalFailureDetails[]> ListAsync([FromQuery] HandlingTechnicalFailureFilter filter, CancellationToken cancellationToken)
    {
        var categories = filter.CategoryID.HasValue
            ? new[] { (await _service.DetailsAsync(filter.CategoryID.Value, cancellationToken)) }
            : await _service.GetDetailsArrayAsync(new TechnicalFailureCategoryFilter { ServiceID = filter.ServiceID }, cancellationToken);

        return categories
            .SelectMany(
                cat => cat.ServiceReferences
                    .Where(x => !filter.ServiceID.HasValue || filter.ServiceID == x.ServiceID)
                    .Select(x => new { category = cat, reference = x }))
            .Select(x => ToDetails(x.category, x.reference))
            .ToArray();
    }

    private static HandlingTechnicalFailureDetails ToDetails(TechnicalFailureCategoryDetails details, ServiceReferenceDetails reference)
    {
        return new HandlingTechnicalFailureDetails
        {
            CategoryID = details.ID,
            CategoryName = details.Name,
            GroupID = reference.GroupID,
            ServiceID = reference.ServiceID,
            ID = reference.IMObjID
        };
    }

    [HttpPost]
    public async Task<HandlingTechnicalFailureDetails> PostAsync(
        [FromBody] HandlingTechnicalFailureData data, 
        CancellationToken cancellationToken)
    {
        var category = await _service.DetailsAsync(data.CategoryID, cancellationToken);
        var newReference =
            await _service.AddServiceReferenceAsync(
                data.CategoryID,
                new ServiceReferenceData { GroupID = data.GroupID, ServiceID = data.ServiceID },
                cancellationToken);

        return ToDetails(category, newReference);
    }

    [HttpPut("{id::guid}")]
    public async Task<HandlingTechnicalFailureDetails> PutAsync(
        Guid id,
        [FromBody]HandlingTechnicalFailureData data, 
        CancellationToken cancellationToken)
    {
        var category = await _service.DetailsAsync(data.CategoryID, cancellationToken);
        var reference = await _service.UpdateServiceReferenceAsync(
            data.CategoryID, 
            data.ServiceID, 
            new ServiceReferenceUpdatableData { GroupID = data.GroupID }, 
            cancellationToken);

        return ToDetails(category, reference);
    }

    [HttpDelete("{id::guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var category = (await _service.GetDetailsArrayAsync(new TechnicalFailureCategoryFilter { ServiceReferenceID = id }))
            .FirstOrDefault();
        
        if (category == null)
        {
            return NotFound();
        }

        var reference = category.ServiceReferences.First(x => x.IMObjID == id);
        await _service.DeleteServiceReferenceAsync(category.ID, reference.ServiceID, cancellationToken);

        return Ok();
    }
}
