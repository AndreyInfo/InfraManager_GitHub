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
    public class HashAlgorithmsController : ControllerBase
    {
        private readonly IEnumBLL<HashAlgorithms> _bll;

        public HashAlgorithmsController(IEnumBLL<HashAlgorithms> bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public Task<LookupItem<HashAlgorithms>[]> GetAsync(CancellationToken cancellationToken = default) => _bll.GetAllAsync(cancellationToken);
    }
}
