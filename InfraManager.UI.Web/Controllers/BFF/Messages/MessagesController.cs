using System;
using InfraManager.BLL.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.UI.Web.Controllers.BFF.Messages;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageBLL _messageBLL;

    public MessagesController(IMessageBLL messageBLL)
    {
        _messageBLL = messageBLL;
    }

    [HttpGet("{id}")]
    public async Task<MessageDetails> GetByIDAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _messageBLL.GetByIDAsync(id, cancellationToken);
    }

    [HttpGet]
    public async Task<MessageDetails[]> GetByFilterAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
    {
        return await _messageBLL.GetListByFilterAsync(filter, cancellationToken);
    }

    [HttpPatch("{id}")]
    public async Task<MessageDetails> PatchAsync(Guid id, [FromBody] MessageData model, CancellationToken cancellationToken)
    {
        return await _messageBLL.UpdateAsync(id, model, cancellationToken);
    }
}
