using AutoMapper;
using InfraManager.BLL.Messages;
using InfraManager.Core;
using InfraManager.Services.MailService;
using InfraManager.UI.Web.Models.Messages;
using InfraManager.WebApi.Contracts.Models.EMailProtocol;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Messages
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SendEMailController : ControllerBase
    {
        private readonly ISendEMailBLL _sendEMailBLL;
        private readonly IMapper _mapper;

        public SendEMailController(ISendEMailBLL sendEMailBLL, IMapper mapper)
        {
            _sendEMailBLL = sendEMailBLL;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<bool> SendEMailsAsync([FromBody]SendEMailRequest request, CancellationToken cancellationToken = default)
        {
            return await _sendEMailBLL.SendEMailAsync(_mapper.Map<SendEMailData>(request), cancellationToken);
        }
    }
}
