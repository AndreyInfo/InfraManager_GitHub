using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.OrganizationStructure.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Access;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ObjectAccessController : ControllerBase
{
    private readonly IObjectResponsibilityAccessBLL _objectResponsibilityAccessBLL;

    public ObjectAccessController(IObjectResponsibilityAccessBLL objectResponsibilityAccessBLL)
    {
        _objectResponsibilityAccessBLL = objectResponsibilityAccessBLL;
    }

    [HttpGet]
    public async Task<ItemResponsibilityTrees> GetListAsync([FromQuery] Guid ownerID, CancellationToken cancellationToken)
        => await _objectResponsibilityAccessBLL.GetAccessAsync(ownerID, cancellationToken);

    //TODO выплить совместно с фронтом, сейчас нет возможности использовать DeleteAsync, PutAsync, PostAsync
    [HttpPut]
    public async Task SaveAsync([FromBody] AccessElementsDetails[] models, CancellationToken cancellationToken)
        => await _objectResponsibilityAccessBLL.SaveAccess(models, cancellationToken);
}
