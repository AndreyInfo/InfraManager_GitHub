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
    public class NegotiationStatusesController : ControllerBase
    {
        private readonly IEnumBLL<NegotiationStatus> _bll;

        public NegotiationStatusesController(IEnumBLL<NegotiationStatus> bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public Task<LookupItem<NegotiationStatus>[]> GetAsync(CancellationToken cancellationToken = default)
        {
            return _bll.GetAllAsync(cancellationToken);
        }
    }
}
