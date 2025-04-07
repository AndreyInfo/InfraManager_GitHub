using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inframanager.BLL;
using InfraManager.DAL.Calendar;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace InfraManager.UI.Web.Controllers.BFF.Calendar;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExclusionsController : ControllerBase
{
    private readonly IEnumBLL<ExclusionType> _enumBLL;

    public ExclusionsController(IEnumBLL<ExclusionType> enumBLL)
    {
        _enumBLL = enumBLL;
    }

    [HttpGet("types")]
    public async Task<LookupItem<ExclusionType>[]> GetTypes(CancellationToken cancellationToken)
    {
        //TODO сделать метод с Expression, чтобы мог добавить филтрацию
        var result = await _enumBLL.GetAllAsync(cancellationToken);
        return result.Where(c => c.ID != ExclusionType.None).ToArray();
    }
}
