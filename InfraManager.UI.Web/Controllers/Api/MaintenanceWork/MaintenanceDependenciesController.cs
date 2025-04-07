using InfraManager.BLL.MaintenanceWork.MaintenanceDependencies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace InfraManager.UI.Web.Controllers.Api.MaintenanceWork;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MaintenanceDependenciesController : ControllerBase
{
    private readonly IMaintenanceDependencyBLL _maintenanceDependencyBLL;

    public MaintenanceDependenciesController(IMaintenanceDependencyBLL maintenanceDependencyBLL)
    {
        _maintenanceDependencyBLL = maintenanceDependencyBLL;
    }


    [HttpGet]
    public Task<MaintenanceDependencyDetails[]> GetAsync([FromQuery] MaintenanceDependencyFilter filter, CancellationToken cancellationToken)
        => _maintenanceDependencyBLL.GetByMaintenanceIDAsync(filter, cancellationToken);


    [HttpPut]
    public Task<Guid> PutAsync([FromBody] MaintenanceDependencyData maintenance, CancellationToken cancellationToken)
        => _maintenanceDependencyBLL.UpdateAsync(maintenance, cancellationToken);


    [HttpPost]
    public Task<Guid> PostAsync([FromBody] MaintenanceDependencyData maintenance, CancellationToken cancellationToken)
        => _maintenanceDependencyBLL.AddAsync(maintenance, cancellationToken);


    [HttpDelete]
    public Task DeleteAsync(MaintenanceDependencyDeleteKey key, CancellationToken cancellationToken) 
        => _maintenanceDependencyBLL.DeleteAsync(key, cancellationToken);

}
