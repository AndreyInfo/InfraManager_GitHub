using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Catalog;
using IWorkOrderTemplateBLL = InfraManager.BLL.ServiceDesk.WorkOrderTemplates.IWorkOrderTemplateBLL;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System.Threading;
using InfraManager.BLL.ServiceDesk.WorkOrderTemplates;
using Inframanager.BLL;
using System.Linq;
using InfraManager.DAL.ServiceDesk.WorkOrders.Templates;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.WorkOrders;

//TODO разбить на три контроллера TemplateFolder and Template into BFF
[Authorize]
[ApiController]
[Route("api/workorder/template")]
public class WorkOrderTemplateController : ControllerBase
{
    private readonly IBasicCatalogBLL<WorkOrderTemplate, WorkOrderTemplateDetails, Guid, WorkOrderTemplateListItem> _templateCatalogBLL;
    private readonly IWorkOrderTemplateBLL _workOrderTemplateBLL;
    private readonly IEnumBLL<ExecutorAssignmentType> _enumBLL;

    public WorkOrderTemplateController(IBasicCatalogBLL<WorkOrderTemplate, WorkOrderTemplateDetails, Guid,WorkOrderTemplateListItem> templateCatalogBLL
                                       , IWorkOrderTemplateBLL workOrderTemplateBLL
        , IEnumBLL<ExecutorAssignmentType> enumBLL)
    {
        _workOrderTemplateBLL = workOrderTemplateBLL;
        _enumBLL = enumBLL;
        _templateCatalogBLL = templateCatalogBLL;
        _templateCatalogBLL.SetIncludeItems(c => c.Type);
        _templateCatalogBLL.SetIncludeItems(c => c.Priority);
        _templateCatalogBLL.SetIncludeItems(c => c.Folder);
    }


    public class GetTree
    {
        public Guid? ParentId { get; set; }
    }

    [HttpPost("tree")]
    public async Task<NodeWorkOrderTemplateTree[]> GetTreeAsync([FromBody] GetTree parent, [FromQuery] bool isFirst, CancellationToken cancellationToken)
    {
        return await _workOrderTemplateBLL.GetTreeAsync(parent.ParentId, isFirst, cancellationToken);
    }

    [HttpGet("path")]
    public async Task<NodeWorkOrderTemplateTree[]> GetPathItemAsync(Guid id, ObjectClass classID, CancellationToken cancellationToken)
    {
        return await _workOrderTemplateBLL.GetPathItemByIDAsync(id, classID, cancellationToken);
    }

    [HttpGet]
    public async Task<WorkOrderTemplateDetails[]> GetListTemplateAsync([FromQuery] WorkOrderTemplateFilter filter, CancellationToken cancellationToken)
    {
        return await _workOrderTemplateBLL.GetDataForTableAsync(filter, cancellationToken);
    }

    #region Template
    [HttpGet("item")]
    public async Task<WorkOrderTemplateDetails> GetTemplateAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _workOrderTemplateBLL.GetByIDAsync(id, cancellationToken);
    }

    [HttpPost]
    public async Task<Guid> AddAsync([FromBody] WorkOrderTemplateDetails model, CancellationToken cancellationToken)
    {
        return await _templateCatalogBLL.InsertAsync(model, cancellationToken);
    }

    [HttpPost("addAs")]
    public async Task<Guid> AddAsAsync([FromBody] WorkOrderTemplateDetails model, CancellationToken cancellationToken)
    {
        return await _workOrderTemplateBLL.AddAsAsync(model, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<Guid> UpdateAsync([FromBody] WorkOrderTemplateDetails model, Guid id, CancellationToken cancellationToken)
    {
        return await _templateCatalogBLL.UpdateAsync(id, model, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task RemoveTemplateAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _templateCatalogBLL.RemoveAsync(id, cancellationToken);
    }
    #endregion


    [HttpGet("folder/has/templates")]
    public async Task<bool> GetHasUsingTemplateIntoFolder(Guid folderId, CancellationToken cancellationToken)
    {
        return await _workOrderTemplateBLL.HasUsedTemplateAsync(folderId, cancellationToken);
    }


    [HttpGet("executor/assignment")]
    public async Task<LookupItem<ExecutorAssignmentType>[]> GetExecutorAssignmentTypes(CancellationToken cancellationToken)
    {
        var result = await _enumBLL.GetAllAsync(cancellationToken);

        //все остальные флаги фронтом обрабатываются как чекбоксы, поэтому не отправляются сейчас
        return result.Where(c => c.ID < ExecutorAssignmentType.FlagTTZ).ToArray();
    }
}
