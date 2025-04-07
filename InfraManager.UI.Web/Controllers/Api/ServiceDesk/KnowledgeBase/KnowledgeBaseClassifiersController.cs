using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.KnowledgeBase.KnowledgeBaseClassifiers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.KnowledgeBase;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class KnowledgeBaseClassifiersController : ControllerBase
{
    private readonly IKnowledgeBaseClassifier _service;

    public KnowledgeBaseClassifiersController(IKnowledgeBaseClassifier service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<KnowledgeBaseClassifierDetails[]> GetAsync([FromQuery] KnowledgeBaseClassifierFilter filter,
        CancellationToken cancellationToken = default)
    {
        return _service.GetDetailsArrayAsync(filter, cancellationToken);
    }

    [HttpGet("{id}")]
    public Task<KnowledgeBaseClassifierDetails> GetAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return _service.DetailsAsync(id, cancellationToken);
    }

    [HttpPost]
    public Task<KnowledgeBaseClassifierDetails> PostAsync([FromBody] KnowledgeBaseClassifierData data,
        CancellationToken cancellationToken = default)
    {
        return _service.AddAsync(data, cancellationToken);
    } 
    
    [HttpPut("{id}")]
    public Task<KnowledgeBaseClassifierDetails> PostAsync([FromRoute] Guid id, KnowledgeBaseClassifierData data,
        CancellationToken cancellationToken = default)
    {
        return _service.UpdateAsync(id, data, cancellationToken);
    } 
    
    [HttpDelete("{id}")]
    public Task DeleteAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return _service.DeleteAsync(id, cancellationToken);
    } 
}