using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.RFC;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RfcResultsController : ControllerBase
{
    private readonly ILookupBLL<ChangeRequestResultListItem, ChangeRequestResultDetailsModel, ChangeRequestResultModel, Guid> _service;

    public RfcResultsController(
        ILookupBLL<ChangeRequestResultListItem, ChangeRequestResultDetailsModel, ChangeRequestResultModel, Guid> service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<ChangeRequestResultListItem[]> ListAsync(CancellationToken cancellationToken = default)
    {
        return _service.ListAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    public Task<ChangeRequestResultDetailsModel> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return _service.FindAsync(id, cancellationToken);
    }

    [HttpPost]
    public Task<ChangeRequestResultDetailsModel> PostAsync([FromBody] ChangeRequestResultModel model, CancellationToken cancellationToken = default)
    {
        return _service.AddAsync(model, cancellationToken);
    }

    [HttpPut("{id:guid}")]
    public Task<ChangeRequestResultDetailsModel> PutAsync([FromRoute] Guid id, [FromBody] ChangeRequestResultModel model, CancellationToken cancellationToken = default)
    {
        return _service.UpdateAsync(id, model, cancellationToken);
    }

    [HttpDelete("{id:guid}")]
    public Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return _service.DeleteAsync(id, cancellationToken);
    }
}
