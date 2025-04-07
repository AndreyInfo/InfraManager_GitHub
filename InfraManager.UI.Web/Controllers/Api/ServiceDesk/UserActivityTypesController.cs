using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.UsersActivityType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserActivityTypesController : ControllerBase
{
    private readonly IUserActivityTypeBLL _service;

    public UserActivityTypesController(IUserActivityTypeBLL service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<UserActivityTypeDetails[]> DetailsArrayAsync([FromQuery] UserActivityTypeFilter filterBy, CancellationToken cancellationToken = default)
    {
        return await _service.GetDetailsArrayAsync(filterBy, cancellationToken);
    }
}