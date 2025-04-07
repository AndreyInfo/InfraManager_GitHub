using InfraManager.ServiceBase.MailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Services.MailService
{
    [Route("bff/[controller]")]
    [ApiController]
    [Authorize]
    public class MailServiceController : ControllerBase
    {
        private readonly IMailServiceApi _mailService;

        public MailServiceController(IMailServiceApi mailService)
        {
            _mailService = mailService;
        }

        [HttpGet("ensure")]
        public Task<bool> EnsureAsync(CancellationToken cancellationToken = default)
        {
            return _mailService.EnsureAsync(cancellationToken);
        }
    }
}
