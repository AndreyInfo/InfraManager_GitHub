using Inframanager.BLL;
using InfraManager.BLL.MaintenanceWork.Maintenances;
using InfraManager.DAL.MaintenanceWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.MaintenanceWork;

/// <summary>
/// Регламентные работы
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly IMaintenanceBLL _maintenanceFolderBLL;
    private readonly IEnumBLL<MaintenanceState> _enumStateBLL;
    private readonly IEnumBLL<MaintenanceMultiplicity> _enumMultiplicityBLL;

    public MaintenanceController(IMaintenanceBLL maintenanceFolderBLL
        , IEnumBLL<MaintenanceState> enumStateBLL
        , IEnumBLL<MaintenanceMultiplicity> enumMultiplicityBLL)
    {
        _maintenanceFolderBLL = maintenanceFolderBLL;
        _enumStateBLL = enumStateBLL;
        _enumMultiplicityBLL = enumMultiplicityBLL;
    }


    [HttpGet("{id:guid}")]
    public async Task<MaintenanceDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => await _maintenanceFolderBLL.DetailsAsync(id, cancellationToken);


    [HttpPost]
    public async Task<MaintenanceDetails> PostAsync([FromBody] MaintenanceData maintenance, CancellationToken cancellationToken)
        => await _maintenanceFolderBLL.AddAsync(maintenance, cancellationToken);


    [HttpPut("{id:guid}")]
    public async Task<MaintenanceDetails> PutAsync([FromRoute] Guid id
        , [FromBody] MaintenanceData data
        , CancellationToken cancellationToken)
        => await _maintenanceFolderBLL.UpdateAsync(id, data, cancellationToken);


    [HttpDelete("{id:guid}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => await _maintenanceFolderBLL.DeleteAsync(id, cancellationToken);


    [HttpGet]
    public async Task<MaintenanceDetails[]> ListAsync([FromQuery] MaintenanceFilter filter, CancellationToken cancellationToken)
        => await _maintenanceFolderBLL.GetByFolderIDAsync(filter, cancellationToken);


    [HttpGet("StateValuesList")]
    public async Task<LookupItem<MaintenanceState>[]> GetStateValuesAsync(CancellationToken cancellationToken)
        => await _enumStateBLL.GetAllAsync(cancellationToken);


    [HttpGet("MultiplicityValuesList")]
    public async Task<LookupItem<MaintenanceMultiplicity>[]> GetMultiplicityValuesAsync(CancellationToken cancellationToken)
        => await _enumMultiplicityBLL.GetAllAsync(cancellationToken);
}
