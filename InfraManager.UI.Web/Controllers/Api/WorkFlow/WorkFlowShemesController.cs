using InfraManager.BLL.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.WorkFlow
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkFlowShemesController : ControllerBase
    {
        private readonly IWorkFlowShemeBLL _workFlowShemeBLL;
        public WorkFlowShemesController(IWorkFlowShemeBLL workFlowShemeBLL)
        {
            _workFlowShemeBLL = workFlowShemeBLL;
        }

        [HttpGet("{identifier}")]
        public Task<WorkflowSchemeDetailsModel> GetAsync(string identifier)
        {
            return _workFlowShemeBLL.FindByIdentifierAsync(identifier, HttpContext.RequestAborted);
        }

        [HttpGet("AvailableTypes")]
        public WorkflowSchemeTypeModel[] AvailableTypes()
        {
            return _workFlowShemeBLL.GetAvailableTypes();
        }
    }
}
