using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Import;
using InfraManager.ServiceBase.ImportService.DBService;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.IM.ImportService.Controllers.Database;

[Route("api/Database/UIDBSettings")]
[ApiController]
public class UIDBSettingsController : ControllerBase
{
    private readonly IImportApi _api;

    public UIDBSettingsController(IImportApi api)
    {
        _api = api;
    }


    [HttpGet("{id}")]
    public Task<UIDBSettingsOutputDetails> GetASync(Guid id, CancellationToken cancellationToken = default)
    {
        return _api.DbSettingsDetailsAsync(id, cancellationToken);
    }

    [HttpGet]
    public Task<UIDBSettingsOutputDetails[]> GetAsync([FromQuery] UIDBSettingsFilter filter,
        CancellationToken cancellationToken = default)
    {
        return _api.GetSettingsDetailsArrayAsync(filter, cancellationToken);
    }

    [HttpPost]
    public Task<UIDBSettingsOutputDetails> PostAsync([FromBody] UIDBSettingsData data,
        CancellationToken cancellationToken = default)
    {
        return _api.AddSettingsAsync(data, cancellationToken);
    }

    [HttpPut("{id}")]
    public Task<UIDBSettingsOutputDetails> PutAsync(Guid id, [FromBody] UIDBSettingsData data,
        CancellationToken cancellationToken = default)
    {
        return _api.UpdateSettingsAsync(id, data, cancellationToken);
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _api.SettingsDeleteAsync(id, cancellationToken);
    }
}