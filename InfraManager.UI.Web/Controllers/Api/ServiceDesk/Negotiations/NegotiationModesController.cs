using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Negotiations
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NegotiationModesController : ControllerBase
    {
        private readonly IEnumBLL<NegotiationMode> _bll;

        public NegotiationModesController(IEnumBLL<NegotiationMode> bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public Task<LookupItem<NegotiationMode>[]> GetAsync(CancellationToken cancellationToken = default)
        {
            return _bll.GetAllAsync(cancellationToken);
        }
    }
}
