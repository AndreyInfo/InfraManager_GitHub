using InfraManager.BLL.MaintenanceWork.Folders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.MaintenanceWork;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MaintenanceFoldersController : ControllerBase
{
    private readonly IMaintenanceFolderBLL _maintenanceFolderBLL;

    public MaintenanceFoldersController(IMaintenanceFolderBLL maintenanceFolderBLL)
    {
        _maintenanceFolderBLL = maintenanceFolderBLL;
    }


    [HttpGet("{id:guid}")]
    public Task<MaintenanceFolderDetails> GetByIDAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => _maintenanceFolderBLL.DetailsAsync(id, cancellationToken);


    [HttpPost]
    public Task<MaintenanceFolderDetails> PostAsync([FromBody] MaintenanceFolderData model, CancellationToken cancellationToken)
       => _maintenanceFolderBLL.AddAsync(model, cancellationToken);


    [HttpPut("{id:guid}")]
    public Task<MaintenanceFolderDetails> PutAsync([FromRoute] Guid id
        , [FromBody] MaintenanceFolderData model
        , CancellationToken cancellationToken)
        => _maintenanceFolderBLL.UpdateAsync(id, model, cancellationToken);


    [HttpDelete("{id:guid}")]
    public Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => _maintenanceFolderBLL.DeleteAsync(id, cancellationToken);
}
