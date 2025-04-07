using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Import;
using InfraManager.ServiceBase.ImportService.DBService;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.IM.ImportService.Controllers.Database;

[Route("api/Database/UIDBFields")]
[ApiController]
public class UIDBFieldsController : ControllerBase
{

    private readonly IImportApi _api;

    public UIDBFieldsController(IImportApi api)
    {
        _api = api;
    }


    [HttpGet("{id}")]
    public Task<UIDBFieldsOutputDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _api.DbFieldsDetailsAsync(id, cancellationToken);
    }
    
    [HttpGet]
    public Task<UIDBFieldsOutputDetails[]> GetAsync([FromQuery] UIDBFieldsFilter filter,
        CancellationToken cancellationToken = default)
    {
        return _api.GetFieldsDetailsArrayAsync(filter, cancellationToken);
    }
    
    [HttpPost]
    public Task<UIDBFieldsOutputDetails> PostAsync([FromBody] UIDBFieldsData data,
        CancellationToken cancellationToken = default)
    {
        return _api.AddFieldsAsync(data, cancellationToken);
    }
    
    [HttpPut("{id}")]
    public Task<UIDBFieldsOutputDetails> PutAsync(Guid id, [FromBody] UIDBFieldsData data,
        CancellationToken cancellationToken = default)
    {
        return _api.UpdateFieldsAsync(id, data, cancellationToken);
    }
    
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _api.FieldsDeleteAsync(id, cancellationToken);
    }
}