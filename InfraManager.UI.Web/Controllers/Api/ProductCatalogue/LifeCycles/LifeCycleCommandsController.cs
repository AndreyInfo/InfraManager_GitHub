using InfraManager.BLL.Asset.LifeCycleCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue.LifeCycles;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LifeCycleCommandsController : ControllerBase
{
    private readonly ILifeCycleCommandFacade _lifeCycleCommandFacade;

    public LifeCycleCommandsController(ILifeCycleCommandFacade lifeCycleCommandStrategy)
    {
        _lifeCycleCommandFacade = lifeCycleCommandStrategy;
    }

    [HttpPut("PutOnControl/{id}")]
    public async Task<LifeCycleCommandAlertDetails> PutOnControl([FromRoute] Guid id
        , Guid operationID
        , [FromBody] LifeCycleCommandBaseData data
        , CancellationToken cancellationToken)
        => await _lifeCycleCommandFacade.ExecuteWithAlertAsync(id, operationID, data, cancellationToken);

    // TODO: Для след. команд
    [HttpPut("{operationID}/{id}")]
    public async Task Execute([FromRoute] Guid id
        ,[FromRoute] Guid operationID
        , [FromBody] LifeCycleCommandBaseData data
        , CancellationToken cancellationToken)
        => await _lifeCycleCommandFacade.ExecuteAsync(id, operationID, data, cancellationToken);
}
