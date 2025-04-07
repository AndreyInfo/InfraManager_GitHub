using Inframanager.BLL;
using InfraManager.DAL.Software;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Software.ModelCatalog;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SoftwareLanguagesController : ControllerBase
{
    private readonly IEnumBLL<SoftwareModelLanguage> _bll;

    public SoftwareLanguagesController(IEnumBLL<SoftwareModelLanguage> bll)
    {
        _bll = bll;
    }

    [HttpGet]
    public async Task<LookupItem<SoftwareModelLanguage>[]> GetAsync(CancellationToken cancellationToken = default) => await _bll.GetAllAsync(cancellationToken);

}
