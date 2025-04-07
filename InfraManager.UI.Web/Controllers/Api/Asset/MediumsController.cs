using InfraManager.BLL.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Asset;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MediumsController : ControllerBase
{
    private readonly IMeduimBLL _mediumBLL;
    public MediumsController(IMeduimBLL mediumBLL)
    {
        _mediumBLL = mediumBLL;
    }

    [HttpGet]
    public async Task<MediumDetails[]> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await _mediumBLL.GetListAsync(cancellationToken);
    }
}
