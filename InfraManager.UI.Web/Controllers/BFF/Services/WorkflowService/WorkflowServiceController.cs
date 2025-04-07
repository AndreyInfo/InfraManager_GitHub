using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Workflow;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.ServiceBase.WorkflowService;
using InfraManager.Services.WorkflowService;
using InfraManager.UI.Web.Models.Services.Workflow;
using InfraManager.UI.Web.Models.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.Services.WorkflowService;

[Route("WorkflowApi")]
[ApiController]
[Authorize]
public class WorkflowServiceController : ControllerBase
{
    private readonly IWorkflowServiceApi _workflow;
    private readonly ICurrentUser _currentUser;
    private readonly IUserAccessBLL _access;

    public WorkflowServiceController(IWorkflowServiceApi workflow,
        ICurrentUser currentUser,
        IUserAccessBLL access)
    {
        _workflow = workflow;
        _currentUser = currentUser;
        _access = access;
    }
    
    #region WorkflowTracking
    
    [HttpGet("Tracking")]
    public async Task<WorkflowTrackingModel[]> GetWorkflowTrackingListAsync([FromQuery] BaseFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _workflow.GetWorkflowTrackingListAsync(filter.StartRecordIndex, filter.CountRecords,
            filter.SearchString, cancellationToken);
    }
    
    [HttpGet("Tracking/{id}")]
    public async Task<WorkflowTrackingModel> GetWorkflowTrackingAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _workflow.GetWorkflowTrackingAsync(id, cancellationToken);
    }
    
    [HttpGet("Tracking/{id}/Events")]
    public async Task<WorkflowEvent[]> GetWorkflowTrackingEvents([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _workflow.GetWorkflowTrackingEventsAsync(id, cancellationToken);
    }
    
    [HttpGet("Tracking/{id}/States")]
    public async Task<WorkflowStateTracking[]> GetWorkflowTrackingStates([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _workflow.GetWorkflowStateTrackingsAsync(id, cancellationToken);
    }
    
    #endregion

    #region Workflow

    [HttpGet("Workflow/{id}/DebugInfo")]
    public async Task<string> GetWorkflowDebugInfoAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _workflow.GetDebugInfoAsync(id, _currentUser.UserId, cancellationToken);
    }

    [HttpPost("Workflow/{id}/Restart")]
    public async Task RestartWorkflowAsync([FromRoute] Guid id, [FromBody] ClassRequestWorkflowModel request,
        CancellationToken cancellationToken = default)
    {
        await _workflow.RestartWorkflowAsync(request.EntityClassID, id, _currentUser.UserId, cancellationToken);
    }
    
    [HttpDelete("Workflow/{id}/DeleteWithClearing")]
    public async Task DeleteWithClearingAsync([FromRoute] Guid id, [FromBody] ClassRequestWorkflowModel request,
        CancellationToken cancellationToken = default)
    {
        await _workflow.DeleteWorkflowWithClearingAsync(request.EntityClassID, id, _currentUser.UserId,
            cancellationToken);
    }

    [HttpPost("Workflow/Export/{id}")]
    public async Task<FileResult> ExportAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _workflow.ExportAsync(id, cancellationToken);

        var response = File(new MemoryStream(Encoding.UTF8.GetBytes(result.packedString ?? "")),
            "application/octet-stream"); 
        
        Response.Headers.ContentDisposition =
            $"attachment; filename=wfScheme.wfs";
        
        return response;
    }

    [HttpPost("Workflow/Import")]
    public async Task ImportAsync([FromBody] WFImportModel request,
        CancellationToken cancellationToken = default)
    {
        await _workflow.ImportAsync(request.PackedWF, request.Name, request.Description, cancellationToken);
    }

    [HttpGet("Workflow/LastActions/{utcStartDate?}")]
    public async Task<WorkflowEvent[]> WorkflowLastActions([FromRoute] DateTime? utcStartDate,
        CancellationToken cancellationToken = default)
    {
        await CheckWfAccessAsync(OperationID.Workflow_Properties, cancellationToken);      
        return await _workflow.GetWorkflowLastActionsAsync(utcStartDate, cancellationToken);
    }

    [HttpGet("Workflow/ExternalEvents")]
    public async Task<ExternalEventModel[]> WorkflowExternalEvents(CancellationToken cancellationToken = default)
    {
        await CheckWfAccessAsync(OperationID.Workflow_Properties, cancellationToken);
        return await _workflow.GetWorkflowExternalEvents(cancellationToken);
    }

    [HttpPost("Workflow/InformExternalEvent")]
    public async Task InformExternalEventAsync(CancellationToken cancellationToken = default)
    {
        await CheckWfAccessAsync(OperationID.WorkflowScheme_Publish,cancellationToken);
        await _workflow.InformExternalEventAsync(cancellationToken);
    }

    [HttpPost("Workflow/RemoveRedundantWorkflow")]
    public async Task RemoveRedundantWorkflowAsync(CancellationToken cancellationToken = default)
    {
        await CheckWfAccessAsync(OperationID.Workflow_Delete, cancellationToken);
        await _workflow.RemoveRedundantWorkflowAsync(cancellationToken);
    }

    [HttpPost("Workflow/Restart")]
    public async Task RestartServiceAsync(CancellationToken cancellationToken = default)
    {
        await CheckWfAccessAsync(OperationID.WorkflowScheme_Publish,cancellationToken);
        await _workflow.RestartServiceAsync(cancellationToken);
    }

    [HttpPost("Workflow/EntityModifiedAll")]
    public async Task EntityModifiedAllAsync(CancellationToken cancellationToken = default)
    {
        await _workflow.EntityModifiedAllAsync(cancellationToken);
    }

    [HttpDelete("Workflow/DeleteEvent/{eventID}")]
    public async Task DeleteEntityOrEnvironmentEventAsync([FromRoute] Guid eventID,
        CancellationToken cancellationToken = default)
    {
        await CheckWfAccessAsync(OperationID.Workflow_Delete, cancellationToken);
        await _workflow.DeleteEntityOrEnvironmentEventAsync(eventID, cancellationToken);
    }
    
    [HttpPost]
    [Route("WorkflowScheme/Publish", Name = "Publish")]
    public async Task PublishWorkflowScheme([FromBody] WorkflowSchemeEntityRequestModel workflowScheme, CancellationToken cancellationToken = default)
    {
        await CheckWfAccessAsync(OperationID.WorkflowScheme_Publish,cancellationToken);
        await _workflow.PublishWorkflowSchemeAsync(workflowScheme.WorkflowSchemeID, cancellationToken);
    }
    
    [HttpPost]
    [Route("Workflow/RestartAs", Name = "RestartAs")]
    public async Task RestartAsAsync([FromBody] RestartAsRequestModel request, CancellationToken cancellationToken = default)
    {
        await _workflow.RestartAsAsync(request.WorkflowSchemeID, request.EntityClassID, request.EntityID,
            _currentUser.UserId, cancellationToken);
    }

    private async Task CheckWfAccessAsync(OperationID operation, CancellationToken cancellationToken = default) //TODO пока костыль, когда будут jwt(если и будут) чтобы текущего пользователя дергать из токена и проверять в bll wf права
    {
        if (!await _access.UserHasOperationAsync(_currentUser.UserId, operation,
                cancellationToken))
        {
            throw new AccessDeniedException(
                $"User with id = {_currentUser.UserId} has no access to {nameof(operation)} Workflow Scheme");
        }
    }
    
    #endregion
    
    #region WorkflowScheme
    
    [HttpGet("WorkflowSchemes/{Identifier}")]
    public async Task<WorkflowSchemeModel> GetPublishedSchemeByIdentifier([FromRoute] string identifier,
        CancellationToken cancellationToken = default)
    {
        return await _workflow.GetWorkFlowSchemeByIdentifierAsync(identifier, cancellationToken);
    }
    
    #endregion
}