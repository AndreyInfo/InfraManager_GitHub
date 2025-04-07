using Inframanager.BLL;
using InfraManager.DAL.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CryptographicAlgorithmsController : ControllerBase
    {
        private readonly IEnumBLL<CryptographicAlgorithms> _bll;

        public CryptographicAlgorithmsController(IEnumBLL<CryptographicAlgorithms> bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public Task<LookupItem<CryptographicAlgorithms>[]> GetAsync(CancellationToken cancellationToken = default) => _bll.GetAllAsync(cancellationToken);
    }
}
