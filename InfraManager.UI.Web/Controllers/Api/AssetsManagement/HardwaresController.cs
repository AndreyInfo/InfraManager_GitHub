using InfraManager.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.AssetsManagement.Hardware;

namespace InfraManager.UI.Web.Controllers.Api.AssetsManagement;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HardwaresController : ControllerBase
{
    private readonly IHardwareBLL _service;

    public HardwaresController(IHardwareBLL service)
    {
        _service = service;
    }

    /// <summary>
    /// Получить список оборудования асинхронно.
    /// </summary>
    /// <param name="filterBy">Фильтр.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns></returns>
    [HttpPost("Reports/AllHardwares")]
    public Task<HardwareListItem[]> GetAllHardwareListAsync(ListFilter filterBy, CancellationToken cancellationToken = default)
    {
        return _service.AllHardwareAsync(filterBy.ToAllHardwareListFilter(), cancellationToken);
    }

    [HttpGet("Reports/AssetSearch")]
    public Task<AssetSearchListItem[]> GetHardwareReportAsync([FromQuery] AssetSearchListFilter filterBy, CancellationToken cancellationToken = default)
    {
        return _service.GetReportAsync(filterBy.ToAssetSearchListFilter(), cancellationToken);
    }

    [HttpGet("Reports/ClientsHardware")]
    public Task<ClientsHardwareListItem[]> GetClientsHardwareListAsync([FromQuery] ClientsHardwareListFilter filterBy, CancellationToken cancellationToken = default)
    {
        return _service.GetClientsHardwareListAsync(filterBy.ToClientsHardwareListFilter(), cancellationToken);
    }
}