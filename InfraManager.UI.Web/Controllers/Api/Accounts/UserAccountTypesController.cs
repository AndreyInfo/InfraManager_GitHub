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
    public class UserAccountTypesController : ControllerBase
    {
        private readonly IEnumBLL<UserAccountTypes> _bll;

        public UserAccountTypesController(IEnumBLL<UserAccountTypes> bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public Task<LookupItem<UserAccountTypes>[]> GetAsync(CancellationToken cancellationToken = default) => _bll.GetAllAsync(cancellationToken);
    }
}
