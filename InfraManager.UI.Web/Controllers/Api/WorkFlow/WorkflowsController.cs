using InfraManager.BLL.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.ServiceBase.WorkflowService;

namespace InfraManager.UI.Web.Controllers.Api.WorkFlow
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowEntityBLL _service;

        public WorkflowsController(IWorkflowEntityBLL service)
        {
            _service = service;
        }

        [HttpGet("{classId}/{id}")]
        public Task<WorkflowDetails> DetailsAsync(ObjectClass classId, Guid id, CancellationToken cancellationToken = default)
        {
            return _service.DetailsAsync(new InframanagerObject(id, classId));
        }


        [HttpPut("/api/workflowstates/{classId}/{id}")]
        public Task SetStateAsync(ObjectClass classId, Guid id, [FromQuery]string stateName, CancellationToken cancellationToken = default)
        {
            return _service.EnqueueSetStateAsync(
                new WorkflowEntityData
                {
                    Id = id,
                    ClassId = classId,
                    EntityState = stateName
                },
                cancellationToken);
        }

        [HttpGet("transitionAllowed/{classId}/{id}")]
        public async Task<TransitionIsAllowedResult> IsTransitionAllowedAsync(ObjectClass classId, Guid id, [FromQuery] string stateName,
            CancellationToken cancellationToken = default)
        => await _service.TransitionIsAllowedAsync(id, classId, stateName, cancellationToken);
    }
}
