using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using InfraManager.UI.Web.ResourceMapping;
using System.Threading;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ObjectSummaryController : ControllerBase
    {
        private readonly IServiceMapper<ObjectClass, IObjectSummaryBLL> _summaryBllMapping;
        public ObjectSummaryController(IServiceMapper<ObjectClass, IObjectSummaryBLL> summaryBllMapping)
        {
            _summaryBllMapping = summaryBllMapping;
        }

        [HttpGet]
        [Route("{resource}/{objectID:guid}/summary")]
        public async Task<ActionResult<ObjectSummaryInfo>> GetServiceDeskState([FromRoute]WebApiResource resource, [FromRoute]Guid objectID, CancellationToken cancellationToken = default)
        {
            if (resource.TryGetObjectClass(out var classID))
            { 
                return await _summaryBllMapping.Map(classID).GetObjectSummaryAsync(objectID, cancellationToken);
            }

            return NotFound();
        }
    }
}
