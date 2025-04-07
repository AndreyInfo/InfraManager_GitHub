using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Owners;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.Owner;

[Obsolete("Use api/owners instead")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OwnerController : ControllerBase
{
    private IOwnerBLL OwnerBll { get; }
    public OwnerController(IOwnerBLL ownerBll)
    {
        OwnerBll = ownerBll;
    }
    
    [HttpGet("{id}")]
    public async Task<OwnerDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken) => 
        await OwnerBll.GetAsync(id,cancellationToken);

    [HttpPut]
    public async Task UpdateAsync([FromBody] OwnerDetails ownerDetails, CancellationToken cancellationToken) =>
        await OwnerBll.UpdateAsync(ownerDetails, cancellationToken);
}