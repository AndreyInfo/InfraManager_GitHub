using InfraManager.BLL.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Messages
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailMessagesController : ControllerBase
    {
        private readonly IMessageByEmailBLL _messageByEmailBLL;
        public EmailMessagesController(IMessageByEmailBLL messageByEmailBLL) =>_messageByEmailBLL = messageByEmailBLL;
        
        [HttpPost]
        public async Task<MessageByEmailDetails> AddAsync([FromBody] MessageByEmailData messageByEmailData, CancellationToken cancellationToken = default)
            => await _messageByEmailBLL.AddAsync(messageByEmailData, cancellationToken);
         
        [HttpGet("{id}")]
        public async Task<MessageByEmailDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
            => await _messageByEmailBLL.GetAsync(id, cancellationToken);

        [HttpGet("rules")]
        public async Task<byte[]> GetRules(CancellationToken cancellationToken = default)
            => await _messageByEmailBLL.GetRulesSettingsValueAsync(cancellationToken);
        
        [HttpPatch("{id}")]
        public async Task<MessageByEmailDetails> PatchAsync(Guid id, [FromBody] MessageByEmailData messageByEmailData, CancellationToken cancellationToken = default)
            => await _messageByEmailBLL.UpdateAsync(id, messageByEmailData, cancellationToken);
    }
}
