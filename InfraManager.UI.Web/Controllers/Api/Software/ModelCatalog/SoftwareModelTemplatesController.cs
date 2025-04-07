using Inframanager.BLL;
using InfraManager.DAL.Software;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Software.ModelCatalog
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SoftwareModelTemplatesController : ControllerBase
    {
        private readonly IEnumBLL<SoftwareModelTemplate> _bll;

        public SoftwareModelTemplatesController(IEnumBLL<SoftwareModelTemplate> bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public Task<LookupItem<SoftwareModelTemplate>[]> GetAsync(CancellationToken cancellationToken = default) => _bll.GetAllAsync(cancellationToken);
    }
}
