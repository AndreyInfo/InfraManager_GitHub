using InfraManager.BLL.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;


namespace InfraManager.UI.Web.Controllers.Api.Accounts
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserAccountsController : ControllerBase
    {
        private readonly IUserAccountBLL _userAccountBLL;

        public UserAccountsController(IUserAccountBLL userAccountBLL)
        {
            _userAccountBLL = userAccountBLL;
        }

        [HttpGet("{id}")]
        public async Task<UserAccountDetails> GetUserAccountAsync([FromRoute] int id, [FromQuery] bool isDecoded, CancellationToken cancellationToken) =>
            await _userAccountBLL.DetailsAsync(id, isDecoded, cancellationToken);

        [HttpPost]
        public async Task<UserAccountDetails> PostAsync([FromBody] UserAccountData userAccount, CancellationToken cancellationToken) 
            => await _userAccountBLL.AddAsync(userAccount, cancellationToken);

        [HttpPut("{id}")]
        public async Task UpdateUserAccountAsync([FromBody] UserAccountData userAccount, [FromRoute] int id, CancellationToken cancellationToken) =>
            await _userAccountBLL.UpdateAsync(id, userAccount, cancellationToken);

        [HttpDelete("{id}")]
        public async Task DeleteUserAccountAsync([FromRoute] int id, CancellationToken cancellationToken) =>
            await _userAccountBLL.DeleteAsync(id, cancellationToken);

        [HttpGet]
        public async Task<UserAccountDetails[]> ListAsync([FromQuery] UserAccountFilter filter, CancellationToken cancellationToken = default) =>
            await _userAccountBLL.ListAsync(filter, cancellationToken);
    }
}
