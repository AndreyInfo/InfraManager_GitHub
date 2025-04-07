using InfraManager.BLL.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Messages
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EMailTemplateController : ControllerBase
    {
        private readonly IEmailTemplateBLL _service;

        public EMailTemplateController(IEmailTemplateBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<EMailTemplateDetails> GetEMailsAsync([FromQuery] EMailTemplateRequest request, CancellationToken cancellationToken = default)
        {
            return await _service.GetEmailTemplateAsync(request, cancellationToken);
        }
    }
}
