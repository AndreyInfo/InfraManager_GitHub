using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Import;
using InfraManager.ServiceBase.ImportService.DBService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InfraManager.IM.ImportService.Controllers.Database;

[Route("api/Database/UIDBConfiguration")]
[ApiController]
public class UIDBConfigurationController : ControllerBase
{
    private readonly IImportApi _api;

    public UIDBConfigurationController(IImportApi api)
    {
        _api = api;
    }


    [HttpGet("{id}")]
    public async Task<UIDBConfigurationOutputDetails> GetASync(Guid id, CancellationToken cancellationToken = default)
    {
        var detail = await _api.DbConfigurationDetailsAsync(id, cancellationToken);
        return detail;
    }

    [HttpGet]
    public Task<UIDBConfigurationOutputDetails[]> GetAsync([FromQuery] UIDBConfigurationFilter filter,
        CancellationToken cancellationToken = default)
    {
        return _api.GetDbConfigurationDetailsArrayAsync(filter, cancellationToken);
    }

    [HttpPost]
    public async Task<UIDBConfigurationOutputDetails> PostAsync([FromBody] UIDBConfigurationData data,
        CancellationToken cancellationToken = default)
    {
        return await _api.AddAsync(data, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<UIDBConfigurationOutputDetails> PutAsync(Guid id, [FromBody] UIDBConfigurationData data,
        CancellationToken cancellationToken = default)
    {
        return await _api.UpdateAsync(id, data, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _api.ConfigurationDeleteAsync(id, cancellationToken);
    }
}