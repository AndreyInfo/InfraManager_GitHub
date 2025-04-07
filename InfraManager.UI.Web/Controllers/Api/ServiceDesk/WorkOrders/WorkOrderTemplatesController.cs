using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.WorkOrderTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.WorkOrders
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrderTemplatesController : ControllerBase
    {
        private readonly BLL.ServiceDesk.WorkOrders.IWorkOrderTemplateBLL _service;

        public WorkOrderTemplatesController(BLL.ServiceDesk.WorkOrders.IWorkOrderTemplateBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<WorkOrderTemplateDetails[]> GetDetailsArrayAsync(
            [FromQuery]WorkOrderTemplateLookupListFilter filter,
            [FromQuery]ClientPageFilter pageFilter,
            CancellationToken cancellationToken = default)
        {
            return string.IsNullOrWhiteSpace(filter?.SearchName)
                ? _service.GetDetailsArrayAsync(filter, cancellationToken)
                : _service.GetDetailsPageAsync(filter, pageFilter, cancellationToken);
        }
        
        [HttpGet("{id}")]
        public Task<WorkOrderTemplateDetails> GetDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _service.DetailsAsync(id, cancellationToken);
        }
            
    }
}
