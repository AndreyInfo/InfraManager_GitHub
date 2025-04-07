using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.KnowledgeBase;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class KBStatusesController : BaseApiController
{

    private readonly IKnowledgeBaseArticleStatusBLL _service; 
    public KBStatusesController(IKnowledgeBaseArticleStatusBLL service)
    {
        _service = service;
    }


    [HttpGet]
    public async Task<KBArticleStatusDetails[]> ListAsync([FromQuery] LookupListFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _service.GetDetailsArrayAsync(filter,cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<KBArticleStatusDetails> GetAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _service.DetailsAsync(id, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _service.RemoveAsync(id, cancellationToken);
    }

    [HttpPost]
    public async Task<KBArticleStatusDetails> PostAsync([FromBody] LookupData data, CancellationToken cancellationToken = default)
    {
        return await _service.AddAsync(data, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<KBArticleStatusDetails> PutAsync([FromRoute] Guid id, [FromBody] LookupData data,
        CancellationToken cancellationToken = default)
    {
        return await _service.UpdateAsync(id, data, cancellationToken);
    }
}