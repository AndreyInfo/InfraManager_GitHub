using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.ServiceDesk;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using InfraManager.BLL.Catalog;
using InfraManager.DAL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

namespace InfraManager.UI.Web.Controllers.BFF.ServiceDesk;

[Authorize]
[ApiController]
[Route("bff/[controller]")]
public class WokrOrderTemplateFoldersController : ControllerBase
{
    private readonly IBasicCatalogBLL<WorkOrderTemplateFolder, WorkOrderTemplateFolderDetails, Guid, WorkOrderTemplateFolder> _folderCatalogBLL; //TODO think about what to do if no need to map column
    private readonly IWorkOrderTemplateFolderBLL _workOrderTemplateFolderBLL;
    public WokrOrderTemplateFoldersController(IBasicCatalogBLL<WorkOrderTemplateFolder, WorkOrderTemplateFolderDetails, Guid, WorkOrderTemplateFolder> folderCatalogBLL
                                              , IWorkOrderTemplateFolderBLL workOrderTemplateFolderBLL)
    {
        _folderCatalogBLL = folderCatalogBLL;
        _workOrderTemplateFolderBLL = workOrderTemplateFolderBLL;
    }

    [HttpGet("{id}")]
    public async Task<WorkOrderTemplateFolderDetails> GetFolderAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _folderCatalogBLL.GetByIDAsync(id, cancellationToken);
    }


    [HttpGet]
    public async Task<WorkOrderTemplateFolderDetails[]> GetListFolderAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
    {
        return await _folderCatalogBLL.GetByFilterAsync(filter, cancellationToken);
    }

    [HttpPost]
    public async Task<Guid> AddAsync([FromBody] WorkOrderTemplateFolderDetails model, CancellationToken cancellationToken)
    {
        return await _folderCatalogBLL.InsertAsync(model, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<Guid> UpdateAsync([FromBody] WorkOrderTemplateFolderDetails model, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _folderCatalogBLL.UpdateAsync(id, model, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task RemoveFoldersAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _workOrderTemplateFolderBLL.DeleteAsync(id, cancellationToken);
    }
}
