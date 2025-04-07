using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.AccessManagement.Operations;
using InfraManager.DAL.Operations;

namespace InfraManager.UI.Web.Controllers.BFF.Access;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OperationsController : ControllerBase
{
    private readonly IOperationsBLL _operationsBLL;

    public OperationsController(IOperationsBLL operationsBLL)
    {
        _operationsBLL = operationsBLL;
    }


    [HttpGet]
    public async Task<GroupedOperationListItem[]> GetTableRoleAsync([FromQuery] OperationFilter filter,
        CancellationToken cancellationToken)
    {
        return await _operationsBLL.GetOperationsListAsync(filter, cancellationToken);
    }
    
    [HttpPut("{roleID}")]
    public async Task UpdateRolesAsync([FromRoute] Guid roleID, [FromBody] OperationData[] data, CancellationToken cancellationToken) =>
        await _operationsBLL.UpdateOperationsRolesAsync(roleID, data, cancellationToken);
}
