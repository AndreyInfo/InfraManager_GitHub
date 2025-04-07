using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IncidentResultsController : ControllerBase
{
    private readonly ILookupBLL<IncidentResultListItemModel, IncidentResultDetailsModel, IncidentResultModel, Guid> _service;

    public IncidentResultsController(
        ILookupBLL<IncidentResultListItemModel, IncidentResultDetailsModel, IncidentResultModel, Guid> service)
    {
        _service = service;
    }

    [HttpGet("{id:guid}")]
    public Task<IncidentResultDetailsModel> GetByIDAsync([FromRoute] Guid id, CancellationToken cancellationToken = default) 
        => _service.FindAsync(id, cancellationToken);

    [HttpGet]
    public Task<IncidentResultListItemModel[]> ListAsync(CancellationToken cancellationToken = default)
    {
        return _service.ListAsync(cancellationToken);
    }


    [HttpPost]
    public Task<IncidentResultDetailsModel> PostAsync(IncidentResultModel model, CancellationToken cancellationToken = default)
    {
        return _service.AddAsync(model, cancellationToken);
    }

    [HttpPut("{id:guid}")]
    public Task<IncidentResultDetailsModel> PutAsync([FromRoute] Guid id, [FromBody] IncidentResultModel model, CancellationToken cancellationToken = default)
    {
        return _service.UpdateAsync(id, model, cancellationToken);
    }

    [HttpDelete("{id:guid}")]
    public Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return _service.DeleteAsync(id, cancellationToken);
    }
}
