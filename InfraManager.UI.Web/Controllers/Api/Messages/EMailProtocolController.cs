using InfraManager.BLL.Messages;
using InfraManager.WebApi.Contracts.Models.EMailProtocol;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Messages
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EMailProtocolController : ControllerBase
    {
        private readonly IEMailProtocolBLL _eMailProtocolBLL;

        public EMailProtocolController(IEMailProtocolBLL eMailProtocolBLL)
        {
            _eMailProtocolBLL = eMailProtocolBLL;
        }

        [HttpPost]
        public async Task<NotificationReceiverDetails[]> GetEMailsAsync(EMailListRequest request, CancellationToken cancellationToken = default)
        {
            return await _eMailProtocolBLL.GetEMAilsAsync(request, cancellationToken);
        }
    }
}
