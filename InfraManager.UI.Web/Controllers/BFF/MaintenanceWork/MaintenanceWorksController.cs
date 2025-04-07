using InfraManager.BLL.MaintenanceWork;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace InfraManager.UI.Web.Controllers.BFF.MaintenanceWork;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MaintenanceWorksController : ControllerBase
{
    private readonly IMaintenanceWorkBLL _maintenanceWorkBLL;

    public MaintenanceWorksController(IMaintenanceWorkBLL maintenanceWorkBLL)
    {
        _maintenanceWorkBLL = maintenanceWorkBLL;
    }

    [HttpGet("folderTree")]
    public Task<MaintenanceNodeTreeDetails[]> GetFolderTreeAsync(Guid? id, CancellationToken cancellationToken)
        => _maintenanceWorkBLL.GetFolderTreeAsync(id, cancellationToken);


    [HttpGet("pathToElement")]
    public Task<MaintenanceNodeTreeDetails[]> GetPathToElementAsync(Guid id, ObjectClass classID, CancellationToken cancellationToken = default)
        => _maintenanceWorkBLL.GetPathToElementAsync(id, classID, cancellationToken);

}
